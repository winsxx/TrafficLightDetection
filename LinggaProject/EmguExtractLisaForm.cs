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
    public partial class EmguExtractLisaForm : Form
    {
        LisaController lisaController;
        public EmguExtractLisaForm()
        {
            InitializeComponent();
            lisaController = new LisaController();
        }

        private void selectLisaFolderButton_Click(object sender, EventArgs e)
        {
            lisaFolderDialog.SelectedPath = "D:\\LISA_TL_dayTrain\\dayClip1";
            DialogResult result = lisaFolderDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                lisaController.extractFromFolder(lisaFolderDialog.SelectedPath, int.Parse(nbInstances.Text));
            }
        }
    }
}
