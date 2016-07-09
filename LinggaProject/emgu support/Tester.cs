using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.ML;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LinggaProject.emgu_support
{
    class Tester
    {
        Image<Hsv, Byte> originalImage;
        Image<Hsv, Byte> processedImage;
        Image<Gray, Byte> processedImageR;
        Image<Gray, Byte> processedImageG;
        Image<Gray, Byte> processedImageY;
        List<Instance> rangeDetected;
        IStatModel redModel;
        IStatModel greenModel;
        IStatModel yellowModel;
        IStatModel model;

        public Tester()
        {
            // Initialization
            rangeDetected = new List<Instance>();
            // Load Classifier
            if (GlobalConstant.CLASSIFIER_TYPE == "svm") {
                redModel = new SVM();
                greenModel = new SVM();
                yellowModel = new SVM();
                model = new SVM();
            } else if (GlobalConstant.CLASSIFIER_TYPE == "mlp") {
                redModel = new ANN_MLP();
                greenModel = new ANN_MLP();
                yellowModel = new ANN_MLP();
                model = new ANN_MLP();
            } else if (GlobalConstant.CLASSIFIER_TYPE == "bayes") {
                redModel = new NormalBayesClassifier();
                greenModel = new NormalBayesClassifier();
                yellowModel = new NormalBayesClassifier();
                model = new NormalBayesClassifier();
            } else if (GlobalConstant.CLASSIFIER_TYPE == "rforest") {
                redModel = new RTrees();
                greenModel = new RTrees();
                yellowModel = new RTrees();
                model = new RTrees();
            }

            if (GlobalConstant.IS_BINARY) {
                FileStorage fsr = new FileStorage(GlobalConstant.CLASSIFIER_TYPE + "_red_model.xml", FileStorage.Mode.Read);
                redModel.Read(fsr.GetFirstTopLevelNode());
                fsr = new FileStorage(GlobalConstant.CLASSIFIER_TYPE + "_green_model.xml", FileStorage.Mode.Read);
                greenModel.Read(fsr.GetFirstTopLevelNode());
                fsr = new FileStorage(GlobalConstant.CLASSIFIER_TYPE + "_yellow_model.xml", FileStorage.Mode.Read);
                yellowModel.Read(fsr.GetFirstTopLevelNode());
                fsr.ReleaseAndGetString();
            } else {
                FileStorage fsr = new FileStorage(GlobalConstant.CLASSIFIER_TYPE + "_model.xml", FileStorage.Mode.Read);
                model.Read(fsr.GetFirstTopLevelNode());
            }
        }

        public Dictionary<Rectangle, int> imageTesting(Image<Hsv, Byte> originalImage)
        {
            int nbDetected = 0;
            processedImage = originalImage.Copy();
            //// Red range
            Image<Gray, Byte> processedImageR1 = processedImage.InRange(new Hsv(0, 128, 128), new Hsv(15, 255, 255));
            Image<Gray, Byte> processedImageR2 = processedImage.InRange(new Hsv(170, 128, 128), new Hsv(180, 255, 255));
            processedImageR = processedImageR1 + processedImageR2;
            ////processedImageR = processedImage.Convert<Bgr, Byte>().InRange(new Bgr(0, 0, 90), new Bgr(100, 100, 255));
            //// Green range
            ////processedImageG = processedImage.Convert<Bgr, Byte>().InRange(new Bgr(80, 80, 0), new Bgr(255, 255, 90));
            processedImageG = processedImage.InRange(new Hsv(40, 128, 128), new Hsv(95, 255, 255));
            //// Yellow range
            ////processedImageY = processedImage.Convert<Bgr, Byte>().InRange(new Bgr(0, 160, 200), new Bgr(80, 255, 255));
            processedImageY = processedImage.InRange(new Hsv(16, 100, 128), new Hsv(40, 255, 255));

            int height = processedImage.Height / 2;
            int width = processedImage.Width;

            for (int iy = 0; iy < height; iy++) {
                for (int ix = 0; ix < width; ix++) {
                    int colorRange = pointColorRange(iy, ix);
                    if (colorRange >= 0 && colorRange < 3) {
                        Image<Gray, Byte> process;
                        switch (colorRange) {
                            case GlobalConstant.CLASS_RED:
                                process = processedImageR;
                                break;
                            case GlobalConstant.CLASS_GREEN:
                                process = processedImageG;
                                break;
                            case GlobalConstant.CLASS_YELLOW:
                                process = processedImageY;
                                break;
                            default:
                                process = processedImageR;
                                break;
                        }


                        Rectangle rect = new Rectangle();
                        CvInvoke.FloodFill(process, null, new Point(ix, iy), new Hsv().MCvScalar, out rect, new MCvScalar(20, 20, 20), new MCvScalar(20, 20, 20));
                        if (rect.Width >= 3 && rect.Height >= 3 && rect.Width < 15 && rect.Height < 15) {
                            Image<Hsv, Byte> cropped = originalImage.Copy();
                            cropped.ROI = rect;
                            cropped = cropped.Copy();
                            cropped = cropped.Resize(GlobalConstant.INSTANCE_CELL_WIDTH, GlobalConstant.INSTANCE_CELL_HEIGHT, Emgu.CV.CvEnum.Inter.Linear);
                            Hsv[] cells = Extractor.extractCellsFromImage(cropped);
                            Hsv[] n = new Hsv[GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT];

                            Instance i = new Instance(rect, cells, colorRange);
                            rangeDetected.Add(i);

                            nbDetected++;
                        }

                    }
                }
            }
            return classifyRangeDetected(rangeDetected);
        }

        public Dictionary<Rectangle, int> imageTestingFromFile(string filename)
        {
            originalImage = new Image<Hsv, Byte>(filename);
            originalImage.SmoothGaussian(3);
            return imageTesting(originalImage);
        }

        private int pointColorRange(int iy, int ix)
        {
            if (isRed(iy, ix)) {
                return GlobalConstant.CLASS_RED;
            } else if (isGreen(iy, ix)) {
                return GlobalConstant.CLASS_GREEN;
            } else if (isYellow(iy, ix)) {
                return GlobalConstant.CLASS_YELLOW;
            } else {
                return -1;
            }
        }

        private bool isRed(int iy, int ix)
        {
            return processedImageR.Data[iy, ix, 0] > 100;// && !processedImage[iy, ix].Equals(GlobalConstant.BLANK);
        }

        private bool isGreen(int iy, int ix)
        {
            return processedImageG.Data[iy, ix, 0] > 100;// && !processedImage[iy, ix].Equals(GlobalConstant.BLANK);
        }

        private bool isYellow(int iy, int ix)
        {
            return processedImageY.Data[iy, ix, 0] > 100;// && !processedImage[iy, ix].Equals(GlobalConstant.BLANK);
        }

        private Dictionary<Rectangle, int> classifyRangeDetected (List<Instance> rangeDetected)
        {
            Dictionary<Rectangle, int> classifiedRange = new Dictionary<Rectangle, int>();

            foreach (Instance instance in rangeDetected) {
                Hsv[] cells = instance.cells;
                Matrix<float> mat = new Matrix<float>(1, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
                int it = 0;
                foreach (Hsv cell in cells) {
                    //Parallel.ForEach(cells, cell => {
                    mat[0, it * 3 + 0] = (float)(((cell.Hue + GlobalConstant.RED_SHIFT) % 180) / GlobalConstant.HUE_NORMALIZATION_FACTOR);
                    mat[0, it * 3 + 1] = (float)(cell.Satuation / GlobalConstant.SATURATION_NORMALIZATION_FACTOR);
                    mat[0, it * 3 + 2] = (float)(cell.Value / GlobalConstant.VALUE_NORMALIZATION_FACTOR);
                    it++;
                    //});
                }

                int predictedClass = -1;
                if (GlobalConstant.IS_BINARY) {
                    switch (instance.positiveClass) {
                        case GlobalConstant.CLASS_RED:
                            predictedClass = (int)redModel.Predict(mat, null);
                            break;
                        case GlobalConstant.CLASS_GREEN:
                            predictedClass = (int)greenModel.Predict(mat, null);
                            break;
                        case GlobalConstant.CLASS_YELLOW:
                            predictedClass = (int)yellowModel.Predict(mat, null);
                            break;
                    }
                } else {
                    predictedClass = (int)model.Predict(mat, null);
                }
                if (predictedClass != -1 && !classifiedRange.ContainsKey(instance.rect)) {
                    classifiedRange.Add(instance.rect, predictedClass);
                }
            }
            rangeDetected.Clear();

            return classifiedRange;
        }
    }
}
