using System;
using System.IO;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV;
using System.Diagnostics;
using Emgu.CV.Util;

namespace LinggaProject.emgu_support
{
    class Trainer
    {
        // create training data matrix with 100 instance and 243 attributes
        Matrix<float> trainDataRed = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
        Matrix<int> trainClassesRed = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
        int numberOfRed = 0;

        Matrix<float> trainDataGreen = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
        Matrix<int> trainClassesGreen = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
        int numberOfGreen = 0;

        Matrix<float> trainDataYellow = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
        Matrix<int> trainClassesYellow = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
        int numberOfYellow = 0;

        public void manualDatasetGenerate (string folderPath)
        {
            Debug.WriteLine("Trainer::buildClassifier " + folderPath);
            string[] classNamedDirectories = Directory.GetDirectories(folderPath);
            int redDatumCounter = 0;
            int greenDatumCounter = 0;
            int yellowDatumCounter = 0;

            foreach (string directory in classNamedDirectories) {
                Debug.WriteLine("=> " + directory);
                string[] files = Directory.GetFiles(directory);
                foreach (string file in files) {
                    Debug.WriteLine("  => " + file);
                    string classIndexStr = directory.Remove(0, directory.LastIndexOf('\\') + 1);
                    int classIndexInt = Int32.Parse(classIndexStr);
                    Hsv[] cells = Extractor.extractCells(file);

                    // set class and trainData attributes
                    // Red classifier
                    if (classIndexInt % 3 == 0) {
                        trainClassesRed.GetRow(redDatumCounter).SetValue(classIndexInt);
                        for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                            trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 0).SetValue((cells[i].Hue + 20) % 180);
                            trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / 255);
                            trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / 255);
                        }
                        redDatumCounter++;
                    }
                    // Green classifier
                    else if (classIndexInt % 3 == 1) {
                        trainClassesGreen.GetRow(greenDatumCounter).SetValue(classIndexInt);
                        for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                            trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 0).SetValue((cells[i].Hue + 20) % 180);
                            trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / 255);
                            trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / 255);
                        }
                        greenDatumCounter++;
                    }
                    // Yellow classifier
                    else if (classIndexInt % 3 == 2) {
                        trainClassesYellow.GetRow(yellowDatumCounter).SetValue(classIndexInt);
                        for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                            trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 0).SetValue((cells[i].Hue + 20) % 180);
                            trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / 255);
                            trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / 255);
                        }
                        yellowDatumCounter++;
                    }
                }
            }
            numberOfRed = redDatumCounter;
            numberOfGreen = greenDatumCounter;
            numberOfYellow = yellowDatumCounter;

            TrainData tdRed = new TrainData(trainDataRed, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, trainClassesRed);
            TrainData tdGreen = new TrainData(trainDataGreen, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, trainClassesGreen);
            TrainData tdYellow = new TrainData(trainDataYellow, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, trainClassesYellow);

            // Train the Classifier
            // Red classifier
            IStatModel classifier = buildClassifier(tdRed, numberOfRed);
            if (classifier != null) {
                classifier.Save("red_model.xml");
            }
            // Green classifier
            classifier = buildClassifier(tdGreen, numberOfGreen);
            if (classifier != null) {
                classifier.Save("green_model.xml");
            }
            // Yellow classifier
            classifier = buildClassifier(tdYellow, numberOfYellow);
            if (classifier != null) {
                classifier.Save("yellow_model.xml");
            }
        }

        private void printRedDataset ()
        {
            Debug.WriteLine("Print red dataset with " + numberOfRed + " instances.");
            for (int j = 0; j < numberOfRed; j++) {
                Debug.Write(trainClassesRed[j, 0] + ". ");
                for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH * 3; i++) {
                    Debug.Write(trainDataRed.Data[j, i] + ",");
                }
                Debug.WriteLine("");
            }
        }

        private Emgu.CV.ML.IStatModel buildClassifier(TrainData td, int nbInput)
        {
            bool trained;
            switch (GlobalConstant.CLASSIFIER_TYPE) {
                case "bayes":
                    NormalBayesClassifier classifier = new NormalBayesClassifier();

                    trained = classifier.Train(td);
                    if (trained) {
                        Debug.WriteLine("trained with naive bayes");
                        return classifier;
                    }
                    return null;
                case "svm":
                    SVM svm = new SVM();
                    svm.SetKernel(SVM.SvmKernelType.Linear);
                    svm.Type = SVM.SvmType.CSvc;
                    svm.C = 1;

                    trained = svm.TrainAuto(td);
                    if (trained) {
                        Debug.WriteLine("trained with svm");
                        return svm;
                    }
                    return null;
                case "mlp":
                    Debug.WriteLine("using mlp");
                    ANN_MLP network = new ANN_MLP();
                    Matrix<int> layerSize = new Matrix<int>(new int[] { GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3, 5, 1 });
                    Mat layerSizeMat = layerSize.Mat;
                    network.SetLayerSizes(layerSizeMat);
                    network.SetActivationFunction(ANN_MLP.AnnMlpActivationFunction.SigmoidSym, 0, 0);
                    network.TermCriteria = new MCvTermCriteria(10, 1.0e-8);
                    network.SetTrainMethod(ANN_MLP.AnnMlpTrainMethod.Backprop, 0.1, 0.1);

                    Debug.WriteLine("start mlp");
                    try {
                        trained = network.Train(td, (int)Emgu.CV.ML.MlEnum.AnnMlpTrainingFlag.Default);
                        Debug.WriteLine("set trained");
                        if (trained) {
                            Debug.WriteLine("trained with mlp");
                            return network;
                        }
                        return null;
                    } catch (CvException e) {
                        Debug.WriteLine(e);
                        return null;
                    }
                default:
                    return null;
            }
        }
    }
}
