using java.lang;
using java.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.core;

namespace LinggaProject.support
{
    class DatasetGenerator
    {
        private string path;
        private List<CSVInstance> csv_instances;
        private List<TrafficLightInstance> tl_instances;
        private List<string> processed_images;
        private ExtractLisaForm mainForm;

        public DatasetGenerator(string path, ExtractLisaForm mainForm)
        {
            csv_instances = new List<CSVInstance>();
            tl_instances = new List<TrafficLightInstance>();
            processed_images = new List<string>();
            if (!path.EndsWith("\\")) {
                path += "\\";
            }
            this.path = path;
            this.mainForm = mainForm;
        }

        /* Train positive and negative data */
        public void generate(string subpath, string imagepath, int number_of_instances)
        {
            string info = "GENERATE: POSITIVE INSTANCE";
            mainForm.addExplanationText(info);

            if (subpath.StartsWith("\\")) {
                subpath = subpath.Substring(1);
            }
            info = "source: " + path + subpath;
            mainForm.addExplanationText(info);

            try {
                // BACA DARI CSV
                //StreamReader sr = new StreamReader(path + subpath);
                var fs = new FileStream(path + subpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fs)) {
                    string line;
                    string[] row = new string[6];
                    int nb = -1;
                    //int nbYes = 0;
                    processed_images = new List<string>();
                    while ((line = sr.ReadLine()) != null) {
                        nb++;
                        if (nb == 0) {
                            continue;
                        }
                        if (nb > number_of_instances && number_of_instances > 0) break;

                        row = line.Split(';');

                        CSVInstance instance = new CSVInstance();
                        instance.filename = row[0].Replace("/", "\\");
                        instance.status = row[1];

                        instance.upper_left = new System.Drawing.Point(Integer.parseInt(row[2]), Integer.parseInt(row[3]));
                        instance.lower_right = new System.Drawing.Point(Integer.parseInt(row[4]), Integer.parseInt(row[5]));

                        csv_instances.Add(instance);
                    }
                    generateTlInstances(imagepath);
                    negativeTrain(number_of_instances);
                }
            } catch (IOException e) {
                mainForm.addExplanationText("IO exception");
            }
        }

        /* Generate TrafficLightInstances from CSVInstances, in files stored in path+subpath */
        public void generateTlInstances(string subpath)
        {
            if (subpath.StartsWith("\\")) {
                subpath = subpath.Substring(1);
            }

            // JADIKAN TRAFFIC LIGHT FEATURES
            int nb_instance = 0;
            Bitmap image = null;
            Bitmap temp_image = null;
            string full_path = "";
            TrafficLightFeatureExtractor extractor = null;

            foreach (CSVInstance instance in csv_instances) {
                TrafficLightInstance tl_instance;

                if (image == null) {
                    mainForm.addExplanationText("Image null in training");
                }
                int index_of_slash = instance.filename.IndexOf("\\") + 1;
                string full_path_temp = path + subpath + instance.filename.Substring(index_of_slash);
                if (full_path_temp.Equals(full_path)) {
                    // do nothing
                } else {
                    string info = "> Processing Image: " + full_path_temp;
                    mainForm.addExplanationText(info);

                    if (image != null) {
                        temp_image = new Bitmap(image);
                    }
                    image = new Bitmap(full_path_temp);
                    extractor = new TrafficLightFeatureExtractor(image);
                }

                bool color_found = false;
                for (int i = instance.upper_left.X; i <= instance.lower_right.X && !color_found; i++) {
                    for (int j = instance.upper_left.Y; j <= instance.lower_right.Y && !color_found; j++) {
                        if (TrafficLightFeatureExtractor.isRed(image.GetPixel(i, j)) || TrafficLightFeatureExtractor.isGreen(image.GetPixel(i, j)) || TrafficLightFeatureExtractor.isYellow(image.GetPixel(i, j))) {
                            color_found = true;
                            int color_condition = -1;
                            if (TrafficLightFeatureExtractor.isRed(image.GetPixel(i, j))) {
                                color_condition = 0;
                                instance.status = "stop";
                            } else if (TrafficLightFeatureExtractor.isGreen(image.GetPixel(i, j))) {
                                color_condition = 1;
                                instance.status = "go";
                            } else if (TrafficLightFeatureExtractor.isYellow(image.GetPixel(i, j))) {
                                color_condition = 2;
                                instance.status = "warning";
                            }

                            if (color_condition == -1) {
                                continue;
                            }

                            // telusuri untuk mengisi traffic_light_features, maksimal sebesar 60px x 60px
                            instance.upper_left.X = instance.lower_right.X = i;
                            instance.upper_left.Y = instance.lower_right.Y = j;
                            extractor.floodFind(i, j, color_condition, ref instance.upper_left, ref instance.lower_right);

                            tl_instance = TrafficLightFeatureExtractor.nineCellsProcessor(image, instance.upper_left, instance.lower_right, instance.status, 0);

                            // DEBUG
                            //Bitmap data = ImageUtil.CropImage(image, instance.upper_left.X, instance.upper_left.Y, instance.lower_right.X - instance.upper_left.X, instance.lower_right.Y - instance.upper_left.Y);
                            //if (data == null) {
                                //continue;
                            //}
                            //data.Save("Instance Images\\Positive\\" + nb_instance + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".bmp");

                            if (tl_instance != null) {
                                // debug
                                //tl_instance.print();
                                // add to instances
                                tl_instances.Add(tl_instance);
                                nb_instance++;
                                // whiten the image
                                for (int x = instance.upper_left.X; x < instance.lower_right.X; x++) {
                                    for (int y = instance.upper_left.Y; y < instance.lower_right.Y; y++) {
                                        image.SetPixel(x, y, Color.White);
                                    }
                                }
                            }
                        } else {
                            continue;
                        }
                    }
                }
                if (!full_path_temp.Equals(full_path)) {
                    string new_path = "Processing Images\\" + full_path.Substring(full_path.LastIndexOf("\\") + 1);
                    if (temp_image != null) {
                        temp_image.Save(new_path);
                        processed_images.Add(@new_path);
                    }
                    full_path = full_path_temp;
                }
            }
            csv_instances = new List<CSVInstance>();
        }

        /* train negative data, which captured in isRed, isGreen, or isYellow but not stated as traffic light in CSV */
        private void negativeTrain(int nb)
        {
            string info = "GENERATOR: NEGATIVE";
            mainForm.addExplanationText(info);
            TrafficLightFeatureExtractor extractor = new TrafficLightFeatureExtractor();
            int nb_cur = 0;
            int nb_img = 0;
            foreach (string filename in processed_images) {
                nb_img++;
                info = "> Processing Image: " + filename;
                mainForm.addExplanationText(info);
                try {
                    Bitmap image = new Bitmap(filename);
                    List<TrafficLightInstance> current_image_tls = extractor.generateFromBitmap(ref image, false);
                    Bitmap original_image = new Bitmap(filename);
                    //image.Save(filename);

                    int null_instances = 0;
                    foreach (TrafficLightInstance tl_instance in current_image_tls) {
                        if (tl_instance == null) {
                            null_instances++;
                            continue;
                        } else {
                            tl_instances.Add(tl_instance);
                            nb_cur++;

                            // DEBUG
                            Bitmap data = ImageUtil.CropImage(original_image, tl_instance.x, tl_instance.y, (int)tl_instance.width, (int)tl_instance.height);
                            //data.Save("Instance Images\\Negative\\" + nb_cur + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".bmp");
                        }
                    }
                } catch (System.Exception e) {
                    Console.WriteLine(e.StackTrace);
                    info = "Exception when negative training";
                    mainForm.addExplanationText(info);
                    break;
                }
                if (nb != -1 && nb_img > nb) break;
            }
        }

        /* Make ARFF based on given filename */
        /* Precondition: tl_instances has been filled */
        public void makeArff(string filename)
        {
            Instances data;
            FastVector atts = initArff();

            data = new Instances("trafficlights", atts, 0);

            string info = "Make arff with " + tl_instances.Count + " instances";
            mainForm.addExplanationText(info);

            int nb_null_instances = 0;

            foreach (TrafficLightInstance tl_instance in tl_instances) {
                if (tl_instance == null) {
                    nb_null_instances++;
                    continue;
                }

                double[] vals = new double[data.numAttributes()];
                for (int i = 0; i < data.numAttributes(); i++) {
                    for (int j = 0; j < Constants.size * Constants.size; j++) {
                        vals[0 + j * 3] = tl_instance.colors[j].H;
                        vals[1 + j * 3] = tl_instance.colors[j].S;
                        vals[2 + j * 3] = tl_instance.colors[j].B;
                    }
                    vals[Constants.size * Constants.size * 3] = tl_instance.tl_class;
                }
                data.add(new Instance(1.0, vals));
            }

            info = "Training finished";
            mainForm.addExplanationText(info);

            File.WriteAllText(filename, data.toString());
        }

        /* Initialize ARFF structure for training and testing purpose */
        public static FastVector initArff()
        {
            FastVector atts = new FastVector();
            for (int i = 0; i < Constants.size * Constants.size; i++) {
                atts.addElement(new weka.core.Attribute(i + "_h"));
                atts.addElement(new weka.core.Attribute(i + "_s"));
                atts.addElement(new weka.core.Attribute(i + "_b"));
            }
            // - nominal
            FastVector classes = new FastVector();
            classes.addElement("red");          //0
            classes.addElement("green");        //1
            classes.addElement("yellow");       //2
            classes.addElement("false_red");    //3
            classes.addElement("false_green");  //4
            classes.addElement("false_yellow"); //5
            atts.addElement(new weka.core.Attribute("class", classes));

            return atts;
        }
    }
}
