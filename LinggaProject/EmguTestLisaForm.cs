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
    public partial class EmguTestLisaForm : Form
    {
        LisaController lisaController;
        public EmguTestLisaForm()
        {
            InitializeComponent();
            lisaController = new LisaController();
        }

        private void selectLisaFolderButton_Click(object sender, EventArgs e)
        {
            lisaFolderDialog.SelectedPath = "C:\\Users\\jelink\\Documents\\TrafficLightDetection\\LinggaProject\\bin\\Debug\\Extracted";
            DialogResult result = lisaFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                lisaController.testFromFolder(lisaFolderDialog.SelectedPath);
            }
        }
    }
}
