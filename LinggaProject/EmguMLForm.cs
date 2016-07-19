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
    public partial class EmguMLForm : Form
    {
        EmguTrainerForm trainerForm;
        EmguImageTestForm imageTestForm;
        EmguVideoTestForm videoTestForm;
        MainForm nonEmguMainForm;
        EmguExtractLisaForm extractLisaForm;
        EmguTesterForm testLisaForm;
        EmguTrainingGeneratorForm trainingGeneratorForm;
        EmguVideoFrameExtractor videoExtractorFrame;

        public EmguMLForm()
        {
            InitializeComponent();
        }

        private void buildClassifierButton_Click(object sender, EventArgs e)
        {
            trainerForm = new EmguTrainerForm();
            trainerForm.Show();
        }

        private void imageTestButton_Click(object sender, EventArgs e)
        {
            imageTestForm = new EmguImageTestForm();
            imageTestForm.Show();
        }

        private void videoTestButton_Click(object sender, EventArgs e)
        {
            videoTestForm = new EmguVideoTestForm();
            videoTestForm.Show();
        }

        private void nonEmguButton_Click(object sender, EventArgs e)
        {
            nonEmguMainForm = new MainForm();
            nonEmguMainForm.Show();
        }

        private void generateLisaDatasetButton_Click(object sender, EventArgs e)
        {
            extractLisaForm = new EmguExtractLisaForm();
            extractLisaForm.Show();
        }

        private void automatedLisaTestButton_Click(object sender, EventArgs e)
        {
            testLisaForm = new EmguTesterForm();
            testLisaForm.Show();
        }

        private void randomizeTrainingSetButton_Click(object sender, EventArgs e)
        {
            trainingGeneratorForm = new EmguTrainingGeneratorForm();
            trainingGeneratorForm.Show();
        }

        private void extractVideoFrameButton_Click(object sender, EventArgs e)
        {
            videoExtractorFrame = new EmguVideoFrameExtractor();
            videoExtractorFrame.Show();
        }
    }
}
