using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using TrafficLightDetectionUtil;
using System.Threading;
using System.Diagnostics;

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
        private String videoPath = "../../../data/singapore02.mp4";

        private Image<Bgr, byte> prevFrame;
        private TrafficLightSegmentationResult[] prevTrafficLight;
        private int trackingCountDown;
        private bool useTracking;
        private int numOfFrameProcessed;
        private double avgElapsedTime;

        private const int TL_REDETECT_CYCLE = 22;

        public MainView()
        {
            InitializeComponent();
            tlColorSegmentation = new BasicTrafficLightColorSegmentation(5);
            tlTracking = new LucasKanadeTrafficLightTracking();
            tlPositionEliminate = new TrafficLightEliminationByPosition();

            prevFrame = null;
            prevTrafficLight = null;
            useTracking = true;
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var captureFrame = capture.QueryFrame();
            if(captureFrame == null)
            {
                StartButton.Text = "Start";
                frameRateTimer.Tick -= ProcessFrame;
                captureInProgress = false;
                capture = new Emgu.CV.Capture(videoPath);
                trackingCountDown = 0;
                numOfFrameProcessed = 0;
                avgElapsedTime = 0.0;
                return;
            }
            Image<Bgr, Byte> imageFrame = captureFrame.ToImage<Bgr, Byte>();
            var oldImageFrame = imageFrame;
            imageFrame = imageFrame.Resize(640, 360, Emgu.CV.CvEnum.Inter.Linear);
            oldImageFrame.Dispose();

            TrafficLightSegmentationResult[] currentTrafficLight = null;
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

                currentTrafficLight = results;
            }

            var drawFrame = imageFrame.Clone();
            #region draw rectangle
            foreach (TrafficLightSegmentationResult tlRect in currentTrafficLight){
                Bgr boxColor;
                switch (tlRect.ColorLabel)
                {
                    case TrafficLightColorType.Yellow:
                        boxColor = new Bgr(0, 255, 255);
                        break;
                    case TrafficLightColorType.Red:
                        boxColor = new Bgr(0, 0, 255);
                        break;
                    case TrafficLightColorType.Green:
                        boxColor = new Bgr(0, 255, 0);
                        break;
                    default:
                        boxColor = new Bgr(0, 0, 0);
                        break;
                }
                drawFrame.Draw(tlRect.Region, boxColor, -1);
                if (tlTracking is LucasKanadeTrafficLightTracking && trackingCountDown!=TL_REDETECT_CYCLE)
                {
                    List<List<PointF>> pointList = ((LucasKanadeTrafficLightTracking)tlTracking).getTrackPointLists();
                    foreach (var l in pointList)
                    {
                        foreach (var p in l)
                        {
                            drawFrame.Draw(new CircleF(p, 1), new Bgr(255, 0, 0), 1);
                        }
                    }
                }
            }
            #endregion

            #region prepare for next frame
            prevFrame = imageFrame;
            prevTrafficLight = currentTrafficLight;
            #endregion
            CamImageBox.Image = drawFrame;

            stopwatch.Stop();

            numOfFrameProcessed++;
            avgElapsedTime = avgElapsedTime + (stopwatch.ElapsedMilliseconds - avgElapsedTime) / numOfFrameProcessed;
            infoTextBox.Text = "";
            infoTextBox.AppendText("Frame elapsed time   : " + stopwatch.ElapsedMilliseconds + " ms\n");
            infoTextBox.AppendText("Average elapsed time : " + avgElapsedTime.ToString("0.000") + " ms\n");
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
                    trackingCountDown = 0;
                    numOfFrameProcessed = 0;
                    avgElapsedTime = 0.0;
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
