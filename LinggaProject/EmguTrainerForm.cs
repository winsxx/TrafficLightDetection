using LinggaProject.emgu_support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinggaProject
{
    public partial class EmguTrainerForm : EmguBaseForm
    {
        Trainer trainer;

        public EmguTrainerForm()
        {
            trainer = new Trainer(this);
            InitializeComponent();
        }

        private void selectManualTrainingFolderButton_Click(object sender, EventArgs e)
        {
            selectManualTrainingFolderDialog.SelectedPath = "C:\\Users\\jelink\\Documents\\TrafficLightDetection\\data\\training\\Real";
            DialogResult result = selectManualTrainingFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (send, args) => {
                    setElementStatus(selectManualTrainingFolderButton, false);
                    trainer.manualDatasetGenerate(selectManualTrainingFolderDialog.SelectedPath);
                };
                bw.RunWorkerCompleted += (send, args) => {
                    setElementStatus(selectManualTrainingFolderButton, true);
                };

                bw.RunWorkerAsync();
            }

        }

        public override void addExplanationText(string text, bool isAppend)
        {
            if (InvokeRequired) {
                this.Invoke(new Action<string, bool>(addExplanationText), new object[] { text, isAppend });
                return;
            }

            if (isAppend) {
                explanationText.AppendText(Environment.NewLine + text);
            } else {
                explanationText.Text = text;
            }
        }
    }
}
