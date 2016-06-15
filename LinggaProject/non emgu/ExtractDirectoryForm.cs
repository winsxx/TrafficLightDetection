using LinggaProject.support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using weka.classifiers;

namespace LinggaProject
{
    public partial class ExtractDirectoryForm : BaseForm
    {
        public ExtractDirectoryForm()
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

        private void extractFromDirectory()
        {
            BackgroundWorker extractBw = new BackgroundWorker();
            extractBw.DoWork += (send, args) => {
                DatasetGenerator generator = new DatasetGenerator("..\\..\\..\\data\\training\\", this);
                generator.generateFromImagesInDirectory("Real Positive");
                generator.generateFromImagesInDirectory("Real Negative");
                generator.makeArff("traffic.manual.numeric.arff");
            };

            extractBw.RunWorkerAsync();
        }

        public void classifyTest(string test_set_path)
        {
            try {
                weka.classifiers.Classifier tr = Constants.useClassifier();
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

        private void makeModel()
        {
            BackgroundWorker makeModelBw = new BackgroundWorker();
            makeModelBw.DoWork += (send, args) => {
                // Make instance
                weka.core.Instances instances = new weka.core.Instances(new java.io.FileReader("traffic.manual.numeric.arff"));
                instances.setClassIndex(instances.numAttributes() - 1);
                addExplanationText("Instance made");

                // Randomize instance order
                weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
                myRandom.setInputFormat(instances);
                instances = weka.filters.Filter.useFilter(instances, myRandom);
                addExplanationText("Instance randomized");

                // Training
                weka.classifiers.Classifier cl = Constants.useClassifier();
                cl.buildClassifier(instances);
                addExplanationText("Classifier made");

                // save model
                weka.core.SerializationHelper.write("model", cl);
                addExplanationText("Model saved");
            };

            makeModelBw.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            extractFromDirectory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            makeModel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string filename = openFileDialog1.FileName;
                classifyTest(filename);
                Console.WriteLine(result); // <-- For debugging use.
            }
        }
    }
}
