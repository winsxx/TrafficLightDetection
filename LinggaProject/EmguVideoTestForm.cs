using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LinggaProject.emgu_support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinggaProject
{
    public partial class EmguVideoTestForm : Form
    {
        Capture cap;
        Tester tester;
        private bool captureInProgress = false;

        public EmguVideoTestForm()
        {
            InitializeComponent();
            tester = new Tester();
        }

        private void browseVideoButton_Click(object sender, EventArgs e)
        {
            DialogResult result = testVideoDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                Debug.WriteLine("select");
                cap = new Capture(testVideoDialog.FileName);
            }
        }

        void processFrameUpdateGUI (object sender, EventArgs e)
        {
            try {
                Image<Bgr, Byte> img1 = cap.QueryFrame().ToImage<Bgr, Byte>();
                //testVideoBox.Image = img1;
                //return;
                Image<Hsv, Byte> img = img1.Convert<Hsv, Byte>();

                Dictionary<Rectangle, int> classifiedInstances = tester.imageTesting(img);
                foreach (KeyValuePair<Rectangle, int> classifiedInstance in classifiedInstances) {
                    Hsv color = new Hsv();
                    Rectangle rect = classifiedInstance.Key;
                    switch (classifiedInstance.Value) {
                        case 0:
                            color = new Hsv(179, 255, 255);
                            break;
                        case 1:
                            color = new Hsv(65, 255, 255);
                            break;
                        case 2:
                            color = new Hsv(30, 255, 255);
                            break;
                        default:
                            color = new Hsv(0, 0, 0);
                            break;
                    }
                    // only print detected
                    CvInvoke.Rectangle(img, rect, color.MCvScalar, 1);
                }
                testVideoBox.Image = img;
            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void EmguVideoTestForm_Load(object sender, EventArgs e)
        {
            if (cap == null) {
                try {
                    cap = new Capture();
                } catch (NullReferenceException excpt) {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (cap != null) {
                if (captureInProgress) {  //if camera is getting frames then stop the capture and set button Text
                                          // "Start" for resuming capture
                    //btnStart.Text = "Start!"; //
                    Application.Idle -= processFrameUpdateGUI;
                } else {
                    Debug.WriteLine("cap not null, capture not in progress.");
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture
                    //btnStart.Text = "Stop";
                    Application.Idle += processFrameUpdateGUI;
                }
                captureInProgress = !captureInProgress;
            }
        }
    }
}
