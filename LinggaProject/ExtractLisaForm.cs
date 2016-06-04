using LinggaProject.support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using weka.classifiers;

namespace LinggaProject
{
    public partial class ExtractLisaForm : BaseForm
    {
        public ExtractLisaForm()
        {
            InitializeComponent();
        }

        public override void addExplanationText(string text)
        {
            if (InvokeRequired) {
                this.Invoke(new Action<string>(addExplanationText), new object[] { text });
                return;
            }
            explanationBox.AppendText(text + System.Environment.NewLine);
        }

        private void startExtractionButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker trainBw = new BackgroundWorker();
            trainBw.DoWork += (send, args) => {
                setElementStatus(startExtractionButton, false);
                
                addExplanationText("Extraction Start");

                DatasetGenerator generator = new DatasetGenerator("D:\\LISA_TL_dayTrain\\", this);
                addExplanationText("root path initialized");

                int nb_training = 20;

                // Positive and Negative Training
                generator.generate("dayClip1\\frameAnnotationsBULB.csv", "dayClip1\\frames\\", nb_training / 4);
                generator.generate("dayClip2\\frameAnnotationsBULB.csv", "dayClip2\\frames\\", nb_training / 4);
                generator.generate("dayClip3\\frameAnnotationsBULB.csv", "dayClip3\\frames\\", nb_training / 4);
                generator.generate("dayClip4\\frameAnnotationsBULB.csv", "dayClip4\\frames\\", nb_training / 4);
                generator.makeArff("traffic.numeric.arff");
            };
            trainBw.RunWorkerCompleted += (send, args) => {
                addExplanationText("ARFF Generation finished, traffic.numeric.arff saved");
                setElementStatus(startExtractionButton, true);
            };

            trainBw.RunWorkerAsync();
            
        }

        private void startTrainingButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker trainBw = new BackgroundWorker();
            trainBw.DoWork += (send, args) => {
                setElementStatus(startTrainingButton, false);
                // Make instance
                weka.core.Instances instances = new weka.core.Instances(new java.io.FileReader("traffic.numeric.arff"));
                instances.setClassIndex(instances.numAttributes() - 1);
                addExplanationText("Instance made");

                // Randomize instance order
                weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
                myRandom.setInputFormat(instances);
                instances = weka.filters.Filter.useFilter(instances, myRandom);
                addExplanationText("Instance randomized");

                // Training
                weka.classifiers.Classifier cl = new weka.classifiers.trees.J48();
                cl.buildClassifier(instances);
                addExplanationText("Classifier made");

                // save model
                weka.core.SerializationHelper.write("model", cl);
                addExplanationText("Model saved");
            };
            trainBw.RunWorkerCompleted += (send, args) => {
                setElementStatus(startTrainingButton, true);
                classifyTest("traffic.numeric.arff");
            };

            trainBw.RunWorkerAsync();
        }

        public void setElementStatus(Control element, bool isEnabled)
        {
            if (InvokeRequired) {
                this.Invoke(new Action<Control, bool>(setElementStatus), new object[] { element, isEnabled });
                return;
            }
            element.Enabled = isEnabled;
        }

        public void classifyTest(string test_set_path)
        {
            try {
                weka.classifiers.trees.J48 tr = new weka.classifiers.trees.J48();
                tr.setBinarySplits(true);
                weka.classifiers.Classifier cl = tr;

                // load model
                cl = (weka.classifiers.Classifier)weka.core.SerializationHelper.read("model");
                addExplanationText("Model Loaded, start making classifier");

                // make instance
                weka.core.Instances testsetinstances = new weka.core.Instances(new java.io.FileReader(test_set_path));
                testsetinstances.setClassIndex(testsetinstances.numAttributes() - 1);
                addExplanationText("Test Set Instance Made");

                // classify
                Evaluation evaluation = new Evaluation(testsetinstances);
                evaluation.evaluateModel(cl, testsetinstances);

                string explanation = evaluation.toSummaryString("\n----- TestSet Evaluation Summary -----\n\n", false) + '\n' +
                    evaluation.toMatrixString() + '\n';

                addExplanationText(explanation);
            } catch (java.lang.Exception ex) {
                ex.printStackTrace();
            }
        }
    }
}
