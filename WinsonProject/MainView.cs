using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using TrafficLightDetectionUtil;
using System.Threading;

namespace WinsonProject
{
    public partial class MainView : Form
    {
        private Capture capture;
        private bool captureInProgress;
        private TrafficLightColorSegmentation tlColorSegmentation;
        private TrafficLightTracking tlTracking;
        private TrafficLightElimination tlPositionEliminate;
        private System.Windows.Forms.Timer frameRateTimer;
        private String videoPath = "../../../data/singapore01.mp4";

        private Image<Bgr, byte> prevFrame;
        private Rectangle[] prevTrafficLight;
        private int trackingCountDown;
        private bool useTracking;

        private const int TL_REDETECT_CYCLE = 22;

        public MainView()
        {
            InitializeComponent();
            tlColorSegmentation = new BasicTrafficLightColorSegmentation(5);
            tlTracking = new LucasKanadeTrafficLightTracking();
            tlPositionEliminate = new TrafficLightEliminationByPosition();

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
                if(trackingCountDown == TL_REDETECT_CYCLE)
                {
                    currentTrafficLight = tlTracking.Track(prevFrame, imageFrame, prevTrafficLight);
                }
                else
                {
                    currentTrafficLight = tlTracking.ContinueTrack(imageFrame);
                } 
                trackingCountDown--;
            } else
            {
                trackingCountDown = TL_REDETECT_CYCLE;

                #region traffic light detection
                imageFrame.ROI = new Rectangle(0, 0, 640, 180);
                TrafficLightSegmentationResult[] results = tlColorSegmentation.DoColorSegmentation(imageFrame);
                imageFrame.ROI = new Rectangle(0, 0, 640, 360);
                #endregion

                #region traffic light elimination
                results = tlPositionEliminate.Eliminate(imageFrame, results);
                #endregion

                var trafficLightCandidate = new List<Rectangle>();
                foreach (TrafficLightSegmentationResult result in results)
                {
                    trafficLightCandidate.Add(result.Region);
                }
                currentTrafficLight = trafficLightCandidate.ToArray();
                
            }

            var drawFrame = imageFrame.Clone();
            #region draw rectangle
            foreach (Rectangle tlRect in currentTrafficLight){
                drawFrame.Draw(tlRect, new Bgr(255, 0, 255), -1);
                if (tlTracking is LucasKanadeTrafficLightTracking)
                {
                    List<List<PointF>> tes = ((LucasKanadeTrafficLightTracking)tlTracking).getTrackPointLists();
                    foreach (var l in tes)
                    {
                        foreach (var p in l)
                        {
                            drawFrame.Draw(new CircleF(p, 1), new Bgr(0, 255, 0), 1);
                        }
                    }
                }
            }
            #endregion

            #region prepare for next frame
            prevFrame = imageFrame;
            prevTrafficLight = currentTrafficLight;
            #endregion
            Thread.Sleep(10);
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
                    var fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                    frameRateTimer = new System.Windows.Forms.Timer();
                    frameRateTimer.Interval = (int)(1000.0 / fps);
                    frameRateTimer.Start();
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
                    frameRateTimer.Tick -= ProcessFrame;
                } else
                {
                    StartButton.Text = "Stop";
                    frameRateTimer.Tick += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

    }
}
