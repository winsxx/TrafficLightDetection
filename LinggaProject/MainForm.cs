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
    public partial class MainForm : Form
    {
        ExtractLisaForm lisaForm;
        public MainForm()
        {
            lisaForm = new ExtractLisaForm();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lisaForm.Show();
        }
    }
}
