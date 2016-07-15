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
    public partial class EmguTrainingGeneratorForm : EmguBaseForm
    {
        LisaController lisaController;
        public EmguTrainingGeneratorForm()
        {
            InitializeComponent();
            lisaController = new LisaController(this);
        }

        private void selectLisaFolderButton_Click(object sender, EventArgs e)
        {
            lisaFolderDialog.SelectedPath = "C:\\Users\\jelink\\Documents\\TrafficLightDetection\\data\\training\\Random";
            DialogResult result = lisaFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (send, args) => {
                    setElementStatus(selectLisaFolderButton, false);
                    lisaController.randomPick(lisaFolderDialog.SelectedPath);
                };
                bw.RunWorkerCompleted += (send, args) => {
                    setElementStatus(selectLisaFolderButton, true);
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
