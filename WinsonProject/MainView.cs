using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using TrafficLightDetectionUtil;

namespace WinsonProject
{
    public partial class MainView : Form
    {
        private Capture capture;
        private bool captureInProgress;
        private TrafficLightColorSegmentation tlColorSegmentation;
        private TrafficLightTracking tlTracking;
        private String videoPath = "../../../data/singapore01.mp4";

        private Image<Bgr, byte> prevFrame;
        private Rectangle[] prevTrafficLight;
        private int trackingCountDown;
        private bool useTracking;

        private const int TL_REDETECT_CYCLE = 30;

        public MainView()
        {
            InitializeComponent();
            tlColorSegmentation = new BasicTrafficLightColorSegmentation(5);
            tlTracking = new LucasKanadeTrafficLightTracking();

            prevFrame = null;
            prevTrafficLight = null;
            trackingCountDown = 0;
            useTracking = true;
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>();
            imageFrame = imageFrame.Resize(640, 360, Emgu.CV.CvEnum.Inter.Linear);

            Rectangle[] currentTrafficLight = null;
            if (useTracking && trackingCountDown > 0)
            {
                trackingCountDown--;
                currentTrafficLight = tlTracking.Track(prevFrame, imageFrame, prevTrafficLight);
            } else
            {
                trackingCountDown = TL_REDETECT_CYCLE;

                #region traffic light detection
                imageFrame.ROI = new Rectangle(0, 0, 640, 180);
                TrafficLightSegmentationResult[] results = tlColorSegmentation.DoColorSegmentation(imageFrame);
                imageFrame.ROI = new Rectangle(0, 0, 640, 360);

                var trafficLightCandidate = new List<Rectangle>();
                foreach (TrafficLightSegmentationResult result in results)
                {
                    trafficLightCandidate.Add(result.Region);
                }
                currentTrafficLight = trafficLightCandidate.ToArray();
                #endregion
            }

            var drawFrame = imageFrame.Clone();
            #region draw rectangle
            foreach (Rectangle tlRect in currentTrafficLight){
                drawFrame.Draw(tlRect, new Bgr(255, 0, 255), 1);
            }
            #endregion

            #region prepare for next frame
            prevFrame = imageFrame;
            prevTrafficLight = currentTrafficLight;
            #endregion

            CamImageBox.Image = drawFrame;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    capture = new Emgu.CV.Capture(videoPath);
                }
                catch (NullReferenceException except)
                {
                    MessageBox.Show(except.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                if (captureInProgress)
                {
                    StartButton.Text = "Start";
                    Application.Idle -= ProcessFrame;
                } else
                {
                    StartButton.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

    }
}
