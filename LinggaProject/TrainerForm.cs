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
    public partial class TrainerForm : Form
    {
        Trainer trainer;

        public TrainerForm()
        {
            trainer = new Trainer();
            InitializeComponent();
        }

        private void selectManualTrainingFolderButton_Click(object sender, EventArgs e)
        {
            selectManualTrainingFolderDialog.SelectedPath = "C:\\Users\\jelink\\Documents\\TrafficLightDetection\\data\\training\\Real";
            DialogResult result = selectManualTrainingFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                trainer.manualDatasetGenerate(selectManualTrainingFolderDialog.SelectedPath);
            }
        }
    }
}
