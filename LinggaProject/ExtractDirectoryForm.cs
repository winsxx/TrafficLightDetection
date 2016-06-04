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

        private void ExtractFromDirectory()
        {
            DatasetGenerator generator = new DatasetGenerator("..\\..\\..\\data\\training\\", this);
            generator.generateFromImagesInDirectory("Real Positive");
            generator.generateFromImagesInDirectory("Real Negative");
            generator.makeArff("training.numeric.manual.arff");
        }

        private void ExtractDirectoryForm_Load(object sender, EventArgs e)
        {
            ExtractFromDirectory();
        }
    }
}
