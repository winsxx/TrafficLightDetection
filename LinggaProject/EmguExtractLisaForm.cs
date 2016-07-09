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
    public partial class EmguExtractLisaForm : EmguBaseForm
    {
        LisaController lisaController;
        public EmguExtractLisaForm()
        {
            InitializeComponent();
            lisaController = new LisaController(this);
        }

        private void selectLisaFolderButton_Click(object sender, EventArgs e)
        {
            lisaFolderDialog.SelectedPath = "D:\\LISA_TL_dayTrain\\dayClip1";
            DialogResult result = lisaFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (send, args) => {
                    setElementStatus(selectLisaFolderButton, false);
                    lisaController.extractFromFolder(lisaFolderDialog.SelectedPath, int.Parse(nbInstances.Text));
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
