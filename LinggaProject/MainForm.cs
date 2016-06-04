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

namespace LinggaProject
{
    public partial class MainForm : BaseForm
    {
        ExtractLisaForm lisaForm;
        ExtractDirectoryForm dirForm;
        public MainForm()
        {
            lisaForm = new ExtractLisaForm();
            dirForm = new ExtractDirectoryForm();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lisaForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dirForm.Show();
        }
    }
}
