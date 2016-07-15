using Emgu.CV;
using Emgu.CV.Structure;
using LinggaProject.emgu_support;
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
    public partial class EmguVideoFrameExtractor : EmguBaseForm
    {
        Capture _capture;
        int fps = 60;
        int halfFps;

        public EmguVideoFrameExtractor()
        {
            halfFps = fps / 2;
            InitializeComponent();
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

        private void browseVideoButton_Click(object sender, EventArgs e)
        {
            DialogResult result = videoDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (send, args) => {
                    setElementStatus(browseVideoButton, false);
                    processAllFrames(videoDialog.FileName);
                };
                bw.RunWorkerCompleted += (send, args) => {
                    setElementStatus(browseVideoButton, true);
                };

                bw.RunWorkerAsync();
            }
        }

        private void processAllFrames(string filename)
        {
            if (Directory.Exists("VideoFrames")) {
                Directory.Delete("VideoFrames", true);
            }

            Directory.CreateDirectory("VideoFrames");

            _capture = new Capture(filename);
            Image<Bgr, Byte> frame;
            int i = 0;

            do {
                frame = _capture.QueryFrame().ToImage<Bgr, Byte>();
                if (i % halfFps == 0) {
                    string saveFilename = "VideoFrames/frame_" + i + ".bmp";
                    frame.Save(saveFilename);
                    addExplanationText("Saving : " + saveFilename, true);
                }
                i++;
            } while (frame != null);
        }
    }
}
