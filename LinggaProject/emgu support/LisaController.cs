using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.emgu_support
{
    class LisaController
    {
        Dictionary<string, int> positiveProcessedImages = new Dictionary<string, int>();
        List<CSVInstance> csvInstances = new List<CSVInstance>();
        Image<Gray, Byte> processedImageR;
        Image<Gray, Byte> processedImageG;
        Image<Gray, Byte> processedImageY;
        List<Instance> rangeDetected;

        public void extractFromFolder (string folderPath, int limit)
        {
            string csvPath = folderPath + "\\frameAnnotationsBULB.csv";
            string imageFolderPath = folderPath + "\\frames";
            try {
                var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fs)) {
                    string line;
                    string[] row = new string[6];
                    int nb = -1;

                    positiveProcessedImages = new Dictionary<string, int>();

                    while ((line = sr.ReadLine()) != null) {
                        nb++;
                        if (nb == 0) {
                            continue;
                        }
                        if (nb > limit && limit > 0) break;

                        row = line.Split(';');

                        CSVInstance instance = new CSVInstance();
                        instance.filename = row[0].Replace("/", "\\");

                        int x0 = int.Parse(row[2]);
                        int y0 = int.Parse(row[3]);
                        int x1 = int.Parse(row[4]);
                        int y1 = int.Parse(row[5]);
                        instance.rect = new Rectangle(new Point(x0, y0), new Size(x1 - x0, y1 - y0));

                        string classString = row[1];
                        if (classString.StartsWith("go")) {
                            instance.classIndex = GlobalConstant.CLASS_GREEN;
                        } else if (classString.StartsWith("stop")) {
                            instance.classIndex = GlobalConstant.CLASS_RED;
                        } else if (classString.StartsWith("warning")) {
                            instance.classIndex = GlobalConstant.CLASS_YELLOW;
                        } else {
                            continue;
                        }

                        csvInstances.Add(instance);
                    }
                    extractInstanceFiles(imageFolderPath);
                    extractNegativeInstanceFiles();
                }
            } catch (IOException e) {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void extractInstanceFiles(string imageFolderPath)
        {
            // JADIKAN TRAFFIC LIGHT FEATURES
            //int nbInstance = 0;
            Image<Bgr, Byte> image = null;
            Image<Gray, Byte> processedImageR;
            Image<Gray, Byte> processedImageG;
            Image<Gray, Byte> processedImageY;
            string imageFullPath = "";
            int id = 0;

            foreach (CSVInstance instance in csvInstances) {
                int indexOfSlash = instance.filename.IndexOf("\\") + 1;
                string imageFullPathTemp = imageFolderPath + "\\" + instance.filename.Substring(indexOfSlash);

                if (imageFullPathTemp.Equals(imageFullPath)) {
                    // do nothing
                    Debug.WriteLine(instance);
                } else {
                    if (image != null) { // Bukan pertama kali
                        image.Save("Processing\\" + instance.filename.Substring(indexOfSlash));
                    }

                    imageFullPath = imageFullPathTemp;

                    Debug.WriteLine("Extracting From: " + imageFullPathTemp);
                    Debug.WriteLine(instance);
                    image = new Image<Bgr, Byte>(imageFullPathTemp);
                    id = 0;
                }

                // LAKUKAN IN RANGE PER IMAGE
                Image<Hsv, Byte> imageHsv = image.Convert<Hsv, Byte>();
                Image<Gray, Byte> processedImageR1 = imageHsv.InRange(new Hsv(0, 128, 128), new Hsv(15, 255, 255));
                Image<Gray, Byte> processedImageR2 = imageHsv.InRange(new Hsv(170, 128, 128), new Hsv(180, 255, 255));
                processedImageR = processedImageR1 + processedImageR2;
                processedImageG = imageHsv.InRange(new Hsv(40, 128, 128), new Hsv(95, 255, 255));
                processedImageY = imageHsv.InRange(new Hsv(16, 100, 128), new Hsv(40, 255, 255));

                Image<Hsv, Byte> croppedImage = imageHsv.Copy();
                Image<Bgr, Byte> croppedImageBgr = image.Copy();
                Rectangle floodedRect = new Rectangle();
                Point instanceRectCenter = new Point(instance.rect.X + instance.rect.Size.Width / 2, instance.rect.Y + instance.rect.Size.Height / 2);

                // LAKUKAN FLOOD FILL HANYA PADA CHANNEL IMAGE BERDASARKAN INSTANCE.CLASSINDEX
                switch (instance.classIndex) {
                    case GlobalConstant.CLASS_RED:
                        CvInvoke.FloodFill(processedImageR, null, instanceRectCenter, new Hsv(0, 0, 255).MCvScalar, out floodedRect, new MCvScalar(20, 20, 20), new MCvScalar(20, 20, 20));
                        break;
                    case GlobalConstant.CLASS_GREEN:
                        CvInvoke.FloodFill(processedImageG, null, instanceRectCenter, new Hsv(0, 0, 255).MCvScalar, out floodedRect, new MCvScalar(20, 20, 20), new MCvScalar(20, 20, 20));
                        break;
                    case GlobalConstant.CLASS_YELLOW:
                        CvInvoke.FloodFill(processedImageY, null, instanceRectCenter, new Hsv(0, 0, 255).MCvScalar, out floodedRect, new MCvScalar(20, 20, 20), new MCvScalar(20, 20, 20));
                        break;
                }

                // LAKUKAN INI HANYA JIKA HASIL FLOODEDRECT > 3 DAN < 15
                if (floodedRect.Width >= 3 && floodedRect.Height >= 3 && floodedRect.Width < 15 && floodedRect.Height < 15) {
                    croppedImageBgr.ROI = floodedRect;
                    croppedImageBgr = croppedImageBgr.Copy();
                    CvInvoke.cvResetImageROI(croppedImageBgr);

                    croppedImageBgr.Convert<Bgr, Byte>().Save("Extracted\\" + instance.classIndex + "\\" + instance.filename.Substring(indexOfSlash) + "_" + id + ".bmp");
                    id++;

                    // kemudian putihkan bagian yang sudah terproses
                    image.Draw(floodedRect, new Bgr(255, 255, 255), -1);
                }
            }
            csvInstances = new List<CSVInstance>();
        }

        private void extractNegativeInstanceFiles()
        {
            Debug.WriteLine("Negative Instance");
            string[] processingImages = Directory.GetFiles("Processing\\");
            foreach (string imagePath in processingImages) {
                Debug.WriteLine("Extracting Negative From: " + imagePath);
                int id = 0;

                Image<Bgr, Byte> imageBgr = new Image<Bgr, byte>(imagePath);
                Image<Hsv, Byte> imageHsv = imageBgr.Convert<Hsv, Byte>();
                Image<Gray, Byte> process;

                Image<Gray, Byte> processedImageR1 = imageHsv.InRange(new Hsv(0, 128, 128), new Hsv(15, 255, 255));
                Image<Gray, Byte> processedImageR2 = imageHsv.InRange(new Hsv(170, 128, 128), new Hsv(180, 255, 255));

                processedImageR = processedImageR1 + processedImageR2;
                processedImageG = imageHsv.InRange(new Hsv(40, 128, 128), new Hsv(95, 255, 255));
                processedImageY = imageHsv.InRange(new Hsv(16, 100, 128), new Hsv(40, 255, 255));

                for (int iy=0; iy<imageHsv.Height; iy++) {
                    for (int ix=0; ix<imageHsv.Width; ix++) {
                        int classIndex = pointColorRange(iy, ix);
                        if (classIndex != -1) {
                            //Debug.WriteLine("found: " + classIndex);
                            switch (classIndex) {
                                case GlobalConstant.CLASS_FALSE_RED:
                                    process = processedImageR;
                                    break;
                                case GlobalConstant.CLASS_FALSE_GREEN:
                                    process = processedImageG;
                                    break;
                                case GlobalConstant.CLASS_FALSE_YELLOW:
                                    process = processedImageY;
                                    break;
                                default:
                                    continue;
                            }

                            Rectangle rect = new Rectangle();
                            CvInvoke.FloodFill(process, null, new Point(ix, iy), new Hsv().MCvScalar, out rect, new MCvScalar(20, 20, 20), new MCvScalar(20, 20, 20));
                            if (rect.Width >= 3 && rect.Height >= 3) {
                                Image<Hsv, Byte> cropped = imageHsv.Copy();
                                cropped.ROI = rect;
                                cropped = cropped.Copy();

                                int indexOfSlash = imagePath.IndexOf("\\") + 1;
                                cropped.Convert<Bgr, Byte>().Save("Extracted\\" + classIndex + "\\" + imagePath.Substring(indexOfSlash) + "_" + id + ".bmp");
                                id++;
                            }
                        }
                    }
                }
            }
        }

        private int pointColorRange(int iy, int ix)
        {
            if (isRed(iy, ix)) {
                return GlobalConstant.CLASS_FALSE_RED;
            } else if (isGreen(iy, ix)) {
                return GlobalConstant.CLASS_FALSE_GREEN;
            } else if (isYellow(iy, ix)) {
                return GlobalConstant.CLASS_FALSE_YELLOW;
            } else {
                return -1;
            }
        }

        private bool isRed(int iy, int ix)
        {
            return processedImageR.Data[iy, ix, 0] > 100;
        }

        private bool isGreen(int iy, int ix)
        {
            return processedImageG.Data[iy, ix, 0] > 100;
        }

        private bool isYellow(int iy, int ix)
        {
            return processedImageY.Data[iy, ix, 0] > 100;
        }
    }

    class CSVInstance
    {
        public int id;
        public string filename;
        public Rectangle rect;
        public int classIndex;

        public override string ToString()
        {
            return filename + ": " + classIndex + " in " + rect;
        }
    }
}
