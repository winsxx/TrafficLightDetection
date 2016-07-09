using System;
using System.IO;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV;
using System.Diagnostics;
using Emgu.CV.Util;
using Emgu.CV.ML.MlEnum;

namespace LinggaProject.emgu_support
{
    class Trainer
    {
        // BINARY CLASS
        // create training data matrix
        Matrix<float> trainDataRed;
        Matrix<int> trainClassesRed;
        int numberOfRed = 0;

        Matrix<float> trainDataGreen;
        Matrix<int> trainClassesGreen;
        int numberOfGreen = 0;

        Matrix<float> trainDataYellow;
        Matrix<int> trainClassesYellow;
        int numberOfYellow = 0;

        // MULTI CLASS
        Matrix<float> trainData;
        Matrix<int> trainClasses;
        int numberOfData = 0;

        EmguBaseForm form;

        public Trainer(EmguBaseForm form)
        {
            this.form = form;
            if (GlobalConstant.IS_BINARY) {
                // RED
                trainDataRed = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
                trainClassesRed = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
                // GREEN
                trainDataGreen = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
                trainClassesGreen = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
                // YELLOW
                trainDataYellow = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
                trainClassesYellow = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
            } else {
                trainData = new Matrix<float>(GlobalConstant.NUMBER_OF_TRAINING_DATA, GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT * 3);
                trainClasses = new Matrix<int>(GlobalConstant.NUMBER_OF_TRAINING_DATA, 1);
            }
        }

        public void manualDatasetGenerate (string folderPath)
        {
            form.addExplanationText("Trainer::buildClassifier " + folderPath, false);
            string[] classNamedDirectories = Directory.GetDirectories(folderPath);
            int redDatumCounter = 0;
            int greenDatumCounter = 0;
            int yellowDatumCounter = 0;
            int datumCounter = 0;

            foreach (string directory in classNamedDirectories) {
                form.addExplanationText("=> " + directory, true);
                string[] files = Directory.GetFiles(directory);
                try {
                    foreach (string file in files) {
                        form.addExplanationText("  => " + file, true);
                        string classIndexStr = directory.Remove(0, directory.LastIndexOf('\\') + 1);
                        int classIndexInt = Int32.Parse(classIndexStr);
                        Hsv[] cells = Extractor.extractCells(file);

                        // set class and trainData attributes
                        if (GlobalConstant.IS_BINARY) {
                            // Red classifier
                            if (classIndexInt % 3 == 0) {
                                trainClassesRed.GetRow(redDatumCounter).SetValue(classIndexInt);
                                for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                                    trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 0).SetValue(((cells[i].Hue + GlobalConstant.RED_SHIFT) % 180) / GlobalConstant.HUE_NORMALIZATION_FACTOR);
                                    trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / GlobalConstant.SATURATION_NORMALIZATION_FACTOR);
                                    trainDataRed.GetRow(redDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / GlobalConstant.VALUE_NORMALIZATION_FACTOR);
                                }
                                redDatumCounter++;
                            }
                            // Green classifier
                            else if (classIndexInt % 3 == 1) {
                                trainClassesGreen.GetRow(greenDatumCounter).SetValue(classIndexInt);
                                for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                                    trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 0).SetValue(((cells[i].Hue + GlobalConstant.RED_SHIFT) % 180) / GlobalConstant.HUE_NORMALIZATION_FACTOR);
                                    trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / GlobalConstant.SATURATION_NORMALIZATION_FACTOR);
                                    trainDataGreen.GetRow(greenDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / GlobalConstant.VALUE_NORMALIZATION_FACTOR);
                                }
                                greenDatumCounter++;
                            }
                            // Yellow classifier
                            else if (classIndexInt % 3 == 2) {
                                trainClassesYellow.GetRow(yellowDatumCounter).SetValue(classIndexInt);
                                for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                                    trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 0).SetValue(((cells[i].Hue + GlobalConstant.RED_SHIFT) % 180) / GlobalConstant.HUE_NORMALIZATION_FACTOR);
                                    trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / GlobalConstant.SATURATION_NORMALIZATION_FACTOR);
                                    trainDataYellow.GetRow(yellowDatumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / GlobalConstant.VALUE_NORMALIZATION_FACTOR);
                                }
                                yellowDatumCounter++;
                            }
                        } else {
                            trainClasses.GetRow(datumCounter).SetValue(classIndexInt);
                            for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH; i++) {
                                trainData.GetRow(datumCounter).GetCol(i * 3 + 0).SetValue(((cells[i].Hue + GlobalConstant.RED_SHIFT) % 180) / GlobalConstant.HUE_NORMALIZATION_FACTOR);

                                trainData.GetRow(datumCounter).GetCol(i * 3 + 1).SetValue(cells[i].Satuation / GlobalConstant.SATURATION_NORMALIZATION_FACTOR);

                                trainData.GetRow(datumCounter).GetCol(i * 3 + 2).SetValue(cells[i].Value / GlobalConstant.VALUE_NORMALIZATION_FACTOR);
                            }
                            datumCounter++;
                        }
                    }
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            }

            if (GlobalConstant.IS_BINARY) {
                // BINARY
                numberOfRed = redDatumCounter;
                numberOfGreen = greenDatumCounter;
                numberOfYellow = yellowDatumCounter;

                // Train the Classifier
                // Red classifier
                Matrix<float> trainDataRed1 = trainDataRed.GetRows(0, numberOfRed, 1);
                Matrix<int> trainClassesRed1 = trainClassesRed.GetRows(0, numberOfRed, 1);
                IStatModel classifier = buildClassifier(trainDataRed1, trainClassesRed1);
                if (classifier != null) {
                    classifier.Save(GlobalConstant.CLASSIFIER_TYPE + "_red_model.xml");
                }
                // Green classifier
                Matrix<float> trainDataGreen1 = trainDataGreen.GetRows(0, numberOfGreen, 1);
                Matrix<int> trainClassesGreen1 = trainClassesGreen.GetRows(0, numberOfGreen, 1);
                classifier = buildClassifier(trainDataGreen1, trainClassesGreen1);
                if (classifier != null) {
                    classifier.Save(GlobalConstant.CLASSIFIER_TYPE + "_green_model.xml");
                }
                // Yellow classifier
                Matrix<float> trainDataYellow1 = trainDataYellow.GetRows(0, numberOfYellow, 1);
                Matrix<int> trainClassesYellow1 = trainClassesYellow.GetRows(0, numberOfYellow, 1);
                classifier = buildClassifier(trainDataYellow1, trainClassesYellow1);
                if (classifier != null) {
                    classifier.Save(GlobalConstant.CLASSIFIER_TYPE + "_yellow_model.xml");
                }
            } else {
                // NOT BINARY
                numberOfData = datumCounter;

                // Train the Classifier
                // Single classifier
                Matrix<float> trainData1 = trainData.GetRows(0, numberOfData, 1);
                Matrix<int> trainClasses1 = trainClasses.GetRows(0, numberOfData, 1);

                printDataset(trainClasses1, trainData1);

                IStatModel classifier = buildClassifier(trainData1, trainClasses1);
                if (classifier != null) {
                    classifier.Save(GlobalConstant.CLASSIFIER_TYPE + "_model.xml");
                }
            }
        }

        private void printDataset(Matrix<int> trainClasses, Matrix<float> trainData)
        {
            form.addExplanationText("Print multi dataset with " + numberOfData + " instances.", true);
            for (int j = 0; j < numberOfData; j++) {
                string exp = "";
                exp += trainClasses[j, 0] + ". ";
                for (int i = 0; i < GlobalConstant.INSTANCE_CELL_HEIGHT * GlobalConstant.INSTANCE_CELL_WIDTH * 3; i++) {
                    //if (i % 3 == 0) {
                        exp += trainData.Data[j, i] + ",";
                    //}
                }
                form.addExplanationText(exp, true);
            }
        }

        private Emgu.CV.ML.IStatModel buildClassifier(Matrix<float> trainData, Matrix<int> trainClasses)
        {
            bool trained;
            switch (GlobalConstant.CLASSIFIER_TYPE) {
                case "bayes":
                    TrainData tdBayes = new TrainData(trainData, DataLayoutType.RowSample, trainClasses);
                    NormalBayesClassifier classifier = new NormalBayesClassifier();

                    trained = classifier.Train(tdBayes);
                    if (trained) {
                        form.addExplanationText("Trained with naive bayes", true);
                        return classifier;
                    }
                    return null;
                case "rforest":
                    try {
                        RTrees rTrees = new RTrees();
                        rTrees.CVFolds = 1;
                        rTrees.MaxDepth = 5;
                        TrainData td = new TrainData(trainData, DataLayoutType.RowSample, trainClasses);
                        trained = rTrees.Train(td);
                        if (trained) {
                            form.addExplanationText("Trained with random forest", true);
                            return rTrees;
                        }
                    } catch (Exception e) {
                        Debug.WriteLine(e.StackTrace);
                    }
                    return null;
                case "svm":
                    try {
                        TrainData tdSvm = new TrainData(trainData, DataLayoutType.RowSample, trainClasses);
                        SVM svm = new SVM();
                        svm.SetKernel(SVM.SvmKernelType.Rbf);

                        trained = svm.TrainAuto(tdSvm);
                        if (trained) {
                            form.addExplanationText("Trained with svm", true);
                            return svm;
                        }
                    } catch (CvException e) {
                        Debug.WriteLine(e.StackTrace);
                    }
                    return null;
                case "mlp":
                    // Make cols as big as classes
                    int nbRows = trainClasses.Rows;
                    Matrix<float> trainClassesMlp = new Matrix<float>(nbRows, 6);
                    for (int i=0; i<nbRows; i++) {
                        switch (trainClasses[i, 0]) {
                            case GlobalConstant.CLASS_RED:
                                for (int j=0; j<6; j++) {
                                    if (j==GlobalConstant.CLASS_RED) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                            case GlobalConstant.CLASS_GREEN:
                                for (int j = 0; j < 6; j++) {
                                    if (j == GlobalConstant.CLASS_GREEN) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                            case GlobalConstant.CLASS_YELLOW:
                                for (int j = 0; j < 6; j++) {
                                    if (j == GlobalConstant.CLASS_YELLOW) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                            case GlobalConstant.CLASS_FALSE_RED:
                                for (int j = 0; j < 6; j++) {
                                    if (j == GlobalConstant.CLASS_FALSE_RED) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                            case GlobalConstant.CLASS_FALSE_GREEN:
                                for (int j = 0; j < 6; j++) {
                                    if (j == GlobalConstant.CLASS_FALSE_GREEN) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                            case GlobalConstant.CLASS_FALSE_YELLOW:
                                for (int j = 0; j < 6; j++) {
                                    if (j == GlobalConstant.CLASS_FALSE_YELLOW) {
                                        trainClassesMlp[i, j] = 1;
                                    } else {
                                        trainClassesMlp[i, j] = 0;
                                    }
                                }
                                break;
                        }
                    }

                    //for (int iy=0; iy<trainClassesMlp.Rows; iy++) {
                    //    Debug.Write(trainClasses[iy, 0] + ": ");
                    //    for (int ix=0; ix<trainClassesMlp.Cols; ix++) {
                    //        Debug.Write(trainClassesMlp[iy, ix] + ", ");
                    //    }
                    //    Debug.WriteLine("");
                    //}

                    TrainData tdMlp = new TrainData(trainData, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, trainClassesMlp);

                    Matrix<int> layerSize = new Matrix<int>(new int[] { trainData.Cols, 80, 6 });
                    ANN_MLP network = new ANN_MLP();
                    network.SetActivationFunction(ANN_MLP.AnnMlpActivationFunction.SigmoidSym);
                    network.TermCriteria = new MCvTermCriteria(3000, 1.0e-6);
                    network.SetLayerSizes(layerSize);

                    network = ActivationFunctionHardFix(network);

                    try {
                        trained = network.Train(tdMlp);
                        if (trained) {
                            form.addExplanationText("Trained with mlp", true);
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

        private ANN_MLP ActivationFunctionHardFix(ANN_MLP network)
        {
            string tmpFile = Path.GetTempPath() + "TempAnnForActivationParametersFix.tmp";
            network.Save(tmpFile);
            StreamReader reader = new StreamReader(tmpFile);
            string configContent = reader.ReadToEnd();
            reader.Close();

            configContent = configContent.Replace("min_val: 0.", "min_val: -0.95");
            configContent = configContent.Replace("max_val: 0.", "max_val: 0.95");
            configContent = configContent.Replace("min_val1: 0.", "min_val1: -0.98");
            configContent = configContent.Replace("max_val1: 0.", "max_val1: 0.98");

            StreamWriter writer = new StreamWriter(tmpFile, false);
            writer.Write(configContent);
            writer.Close();

            network.Read(new FileStorage(tmpFile, FileStorage.Mode.Read).GetFirstTopLevelNode());
            File.Delete(tmpFile);

            return network;
        }
    }
}
