using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;

namespace TrafficLightDetectionUtil
{
    public class LucasKanadeTrafficLightTracking : TrafficLightTracking
    {
        GFTTDetector featureDetector;

        public LucasKanadeTrafficLightTracking()
        {
            featureDetector = new GFTTDetector(maxCorners: 100, qualityLevel: 0.3, minDistance: 1, blockSize: 3);
        }

        public Rectangle[] Track(Image<Bgr, byte> prevFrame, Image<Bgr, byte> currentFrame, Rectangle[] prevBoundingBox)
        {
            var prevGrayFrame = prevFrame.Convert<Gray, byte>();
            var currentGrayFrame = currentFrame.Convert<Gray, byte>();
            var mask = new Image<Gray, byte>(prevFrame.Width, prevFrame.Height);
            List<Rectangle> nextBoundingBox = new List<Rectangle>();

            foreach (Rectangle prevRect in prevBoundingBox)
            {
                #region feature detection
                CvInvoke.Rectangle(mask, prevRect, new MCvScalar(255, 255, 255));
                MKeyPoint[] keyPoints = featureDetector.Detect(prevGrayFrame, mask: mask);
                CvInvoke.Rectangle(mask, prevRect, new MCvScalar(0, 0, 0));
                List<PointF> pointList = new List<PointF>();
                foreach (MKeyPoint keypoint in keyPoints)
                {
                    pointList.Add(keypoint.Point);
                }
                #endregion

                #region tracking
                if (pointList.Count > 0)
                {
                    PointF[] nextPtsArray = null;
                    byte[] status = null;
                    float[] err = null;
                    CvInvoke.CalcOpticalFlowPyrLK(prevGrayFrame, currentGrayFrame, pointList.ToArray(), new Size(15, 15), 2,
                        new MCvTermCriteria(10, 0.03), out nextPtsArray, out status, out err);

                    List<PointF> prevPts = new List<PointF>();
                    List<PointF> nextPts = new List<PointF>();
                    for (var i = 0; i < status.Length; i++)
                    {
                        if (status[i] > 0)
                        {
                            prevPts.Add(pointList[i]);
                            nextPts.Add(nextPtsArray[i]);
                        }
                    }

                    var nextRect = PredictRegion(prevRect, prevPts, nextPts);
                    nextBoundingBox.Add(nextRect);
                }
                else
                {
                    nextBoundingBox.Add(prevRect);
                }
                #endregion

            }

            return nextBoundingBox.ToArray();
        }

        private Rectangle PredictRegion(Rectangle rect, List<PointF> prevPoints, List<PointF> nextPoints)
        {
            var nextPrevPoints = prevPoints.Zip(nextPoints, (prev, next) => new { Prev = prev, Next = next });
            var dxList = new List<float>();
            var dyList = new List<float>();
            foreach (var nextPrevPoint in nextPrevPoints)
            {
                dxList.Add( nextPrevPoint.Next.X - nextPrevPoint.Prev.X );
                dyList.Add( nextPrevPoint.Next.Y - nextPrevPoint.Next.Y );
            }

            var meanDx = 0.0;
            var meanDy = 0.0;
            if (nextPrevPoints.Count() > 0)
            {
                meanDx = dxList.Average();
                meanDy = dyList.Average();
            } 

            return new Rectangle(rect.Left + (int) meanDx, rect.Top + (int) meanDy, rect.Width, rect.Height);
        }
    }
}
