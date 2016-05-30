using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.support
{
    class TrafficLightFeatureExtractor
    {
        static int jumlah_warna = 0;

        Bitmap processed_image;
        private Bitmap preserved_image;

        public TrafficLightFeatureExtractor()
        {
        }

        public TrafficLightFeatureExtractor(Bitmap image)
        {
            preserved_image = new Bitmap(image);
            processed_image = new Bitmap(image);
        }

        public void printExtracted()
        {
            //Console.WriteLine("TL: " + top_left);
            //Console.WriteLine("RB: " + bottom_right);
        }

        public List<TrafficLightInstance> generateFromBitmap(ref Bitmap bmp, bool is_positive)
        {
            Point top_left = new Point(0, 0);
            Point bottom_right = new Point(9000, 9000);

            List<TrafficLightInstance> tl_instances = new List<TrafficLightInstance>();

            processed_image = bmp; //new Bitmap(bmp);
            preserved_image = new Bitmap(bmp);
            bottom_right.X = processed_image.Width - 1;
            bottom_right.Y = processed_image.Height - 1;
            Point p = new Point();
            //bool found = false;
            int color_condition = 0;
            int class_addition = is_positive ? 0 : 3;

            // cari titik pertama
            for (int x = 0; x < processed_image.Width; x++) {
                for (int y = 0; y < processed_image.Height * 1 / 2; y++) {
                    TrafficLightInstance traffic_light = new TrafficLightInstance();
                    Color current_color = processed_image.GetPixel(x, y);
                    if (isRed(current_color) || isGreen(current_color) || isYellow(current_color)) {
                        // red condition
                        if (isRed(processed_image.GetPixel(x, y))) {
                            p.X = x;
                            p.Y = y;
                            color_condition = 0;
                        }
                        // green condition
                        else if (isGreen(processed_image.GetPixel(x, y))) {
                            p.X = x;
                            p.Y = y;
                            color_condition = 1;
                        }
                        // yellow condition
                        else if (isYellow(processed_image.GetPixel(x, y))) {
                            p.X = x;
                            p.Y = y;
                            color_condition = 2;
                        }

                        top_left.X = bottom_right.X = p.X;
                        top_left.Y = bottom_right.Y = p.Y;

                        // telusuri untuk mengisi traffic_light_features, maksimal sebesar 60px x 60px
                        floodFind(p.X, p.Y, color_condition, ref top_left, ref bottom_right);

                        string color_condition_string = "";
                        if (color_condition == 0) {
                            color_condition_string = "stop";
                        } else if (color_condition == 1) {
                            color_condition_string = "go";
                        } else if (color_condition == 2) {
                            color_condition_string = "warning";
                        }

                        int train_status = is_positive ? 0 : 1;
                        traffic_light = nineCellsProcessor(preserved_image, top_left, bottom_right, color_condition_string, train_status);
                        if (traffic_light != null) {
                            traffic_light.tl_class = color_condition + class_addition;
                            tl_instances.Add(traffic_light);
                            //traffic_light.print();
                        }

                    }
                }
            }

            return tl_instances;
        }

        public void floodFind(int x0, int y0, int color_class, ref Point top_left, ref Point bottom_right)
        {
            Stack<Point> stack = new Stack<Point>();
            Point starting_point = new Point(x0, y0);
            stack.Push(starting_point);
            while (stack.Count > 0) {
                Point p = stack.Pop();
                int x = p.X;
                int y = p.Y;

                if (y < 0 || y > processed_image.Height - 1 || x < 0 || x > processed_image.Width - 1)
                    continue;

                //Console.WriteLine("processing: (" + x + ", " + y + ")");

                bool color_condition = false;
                switch (color_class) {
                    case 0:
                        color_condition = isRed(processed_image.GetPixel(x, y));
                        break;
                    case 1:
                        color_condition = isGreen(processed_image.GetPixel(x, y));
                        break;
                    case 2:
                        color_condition = isYellow(processed_image.GetPixel(x, y));
                        break;
                }

                if (color_condition) {
                    jumlah_warna++;
                    stack.Push(new Point(x + 1, y));
                    stack.Push(new Point(x - 1, y));
                    stack.Push(new Point(x, y + 1));
                    stack.Push(new Point(x, y - 1));

                    // update boundaries
                    if (x < top_left.X) {
                        top_left.X = x;
                    }
                    if (x > bottom_right.X) {
                        bottom_right.X = x + 1;
                    }
                    if (y < top_left.Y) {
                        top_left.Y = y;
                    }
                    if (y > bottom_right.Y) {
                        bottom_right.Y = y + 1;
                    }

                    // update color
                    Color current = processed_image.GetPixel(x, y);

                    //Color alpha_current = Color.FromArgb(0, current);
                    processed_image.SetPixel(x, y, Color.White);
                    //Console.WriteLine("nilai a: " + processed_image.GetPixel(x, y).A);
                }
            }

            return;
        }

        public static bool isRed(Color p)
        {
            return p != Color.White && p.R > 110 && p.R - p.G > 100 && p.R - p.B > 100;
        }

        public static bool isGreen(Color p)
        {
            return p != Color.White && p.G > 110 && p.G - p.R > 69;
        }

        public static bool isYellow(Color p)
        {
            return p != Color.White && p.G > 100 && p.R > 100 && p.G - p.B > 50 && p.R - p.B > 50;
        }

        public Bitmap getPreservedImage()
        {
            return preserved_image;
        }

        public Bitmap getProcessedImage()
        {
            return processed_image;
        }

        public static TrafficLightInstance nineCellsProcessor(Bitmap image, Point top_left, Point bottom_right, string status, int train_status)
        {
            TrafficLightInstance tl_instance = new TrafficLightInstance();
            tl_instance.x = top_left.X;
            tl_instance.y = top_left.Y;

            tl_instance.height = bottom_right.Y - top_left.Y;
            tl_instance.width = bottom_right.X - top_left.X;

            float proportion = tl_instance.height / tl_instance.width;

            // jika negative train, dan proporsinya mau kotak, dan ukurannya kecil, return null, karena bisa bikin false positive
            if (train_status == 1 && proportion > 0.75 && proportion < 1.35 && tl_instance.height < 5 && tl_instance.width < 5) {
                return null;
            }

            if (tl_instance.height < 2 || tl_instance.width < 2) {
                return null;
            } else if (train_status == 2 && !(proportion > 0.85 && proportion < 1.15)) {
                return null;
            }

            if (status.StartsWith("go")) {
                tl_instance.tl_class = 1;
            } else if (status.StartsWith("stop")) {
                tl_instance.tl_class = 0;
            } else {
                tl_instance.tl_class = 2;
            }

            int current_cell_number = 0;

            Bitmap resized = ImageUtil.ResizeImage(ImageUtil.CropImage(image, top_left.X, top_left.Y, bottom_right.X - top_left.X, bottom_right.Y - top_left.Y), 3, 3);
            resized.Save("Instance Images\\Resized\\" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".bmp");

            // iterasi X titik

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (resized.GetPixel(i, j) == Color.White) {
                        return null;
                    }

                    // generate mean 9 cell
                    HSB temp_color = new HSB();
                    temp_color.H = (resized.GetPixel(i, j).GetHue() + 40) % 360;
                    temp_color.S = resized.GetPixel(i, j).GetSaturation();
                    temp_color.B = resized.GetPixel(i, j).GetBrightness();

                    // masukkan ke instance
                    tl_instance.colors[current_cell_number] = temp_color;

                    //Console.WriteLine("cellno: " + current_cell_number + " HSB: (" + meanH + ", " + meanS + ", " + meanB + ")");
                    current_cell_number++;
                }
            }

            return tl_instance;
        }
    }
}
