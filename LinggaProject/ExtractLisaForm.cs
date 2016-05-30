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
    public partial class ExtractLisaForm : Form
    {
        public ExtractLisaForm()
        {
            InitializeComponent();
        }

        public void addExplanationText(string text)
        {
            if (InvokeRequired) {
                this.Invoke(new Action<string>(addExplanationText), new object[] { text });
                return;
            }
            explanationBox.AppendText(text + System.Environment.NewLine);
        }

        private void startExteractionButton_Click(object sender, EventArgs e)
        {
            addExplanationText("Extraction Start");
        }
    }
}
