using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;

namespace TrafficLightDetectionUtil
{
    public class LucasKanadeTrafficLightTracking : TrafficLightTracking
    {
        GFTTDetector _featureDetector;
        RectangleF[] _prevBoundingBox;
        List<List<PointF>> _prevPointLists;
        Image<Bgr, byte> _prevFrame;

        public LucasKanadeTrafficLightTracking()
        {
            _featureDetector = new GFTTDetector(maxCorners: 100, qualityLevel: 0.3, minDistance: 1, blockSize: 3);
        }

        public List<List<PointF>> getTrackPointLists()
        {
            return _prevPointLists;
        }

        public Rectangle[] Track(Image<Bgr, byte> prevFrame, Image<Bgr, byte> currentFrame, Rectangle[] prevBoundingBox)
        {
            var prevGrayFrame = prevFrame.Convert<Gray, byte>();
            var currentGrayFrame = currentFrame.Convert<Gray, byte>();
            var mask = new Image<Gray, byte>(prevFrame.Width, prevFrame.Height);
            List<RectangleF> nextBoundingBox = new List<RectangleF>();
            List<List<PointF>> listOfPointList = new List<List<PointF>>();

            foreach (Rectangle prevRect in prevBoundingBox)
            {
                #region feature detection
                CvInvoke.Rectangle(mask, prevRect, new MCvScalar(255, 255, 255));
                MKeyPoint[] keyPoints = _featureDetector.Detect(prevGrayFrame, mask: mask);
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
                    CvInvoke.CalcOpticalFlowPyrLK(prevGrayFrame, currentGrayFrame, pointList.ToArray(), new Size(15, 15), 3,
                        new MCvTermCriteria(20, 0.03), out nextPtsArray, out status, out err);

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

                    if (nextPts.Count > 0)
                    {
                        listOfPointList.Add(nextPts);
                        var nextRect = PredictRegion(prevRect, prevPts, nextPts);
                        nextBoundingBox.Add(nextRect);
                    }
                }
                #endregion

            }

            _prevPointLists = listOfPointList;
            _prevBoundingBox = nextBoundingBox.ToArray();
            _prevFrame = currentFrame;
            return Array.ConvertAll(nextBoundingBox.ToArray(), 
                x => new Rectangle((int)x.Left, (int)x.Top, (int)x.Width, (int)x.Height));
        }

        public Rectangle[] ContinueTrack(Image<Bgr, byte> currentFrame)
        {
            var currentGrayFrame = currentFrame.Convert<Gray, byte>();
            var prevGrayFrame = _prevFrame.Convert<Gray, byte>();
            List<RectangleF> nextBoundingBox = new List<RectangleF>();
            List<List<PointF>> listOfPointList = new List<List<PointF>>();

            var boundingBoxAndKeyPointsList = 
                _prevBoundingBox.Zip(_prevPointLists, (bb, kpl) => new { BoundingBox = bb, KeyPointList = kpl});
            foreach (var boundBoxAndKeyPoints in boundingBoxAndKeyPointsList)
            {
                #region tracking
                PointF[] nextPtsArray = null;
                byte[] status = null;
                float[] err = null;

                CvInvoke.CalcOpticalFlowPyrLK(prevGrayFrame, currentGrayFrame, boundBoxAndKeyPoints.KeyPointList.ToArray(), 
                    new Size(15, 15), 2, new MCvTermCriteria(10, 0.03), out nextPtsArray, out status, out err);

                List<PointF> prevPts = new List<PointF>();
                List<PointF> nextPts = new List<PointF>();
                for (var i = 0; i < status.Length; i++)
                {
                    if (status[i] > 0)
                    {
                        prevPts.Add(boundBoxAndKeyPoints.KeyPointList[i]);
                        nextPts.Add(nextPtsArray[i]);
                    }
                }

                if (nextPts.Count > 0)
                {
                    listOfPointList.Add(nextPts);
                    var nextRect = PredictRegion(boundBoxAndKeyPoints.BoundingBox, prevPts, nextPts);
                    nextBoundingBox.Add(nextRect);
                }
                #endregion
            }
            _prevPointLists = listOfPointList;
            _prevBoundingBox = nextBoundingBox.ToArray();
            _prevFrame = currentFrame;
            return Array.ConvertAll(nextBoundingBox.ToArray(),
                x => new Rectangle((int)x.Left, (int)x.Top, (int)x.Width, (int)x.Height));
        }

        private RectangleF PredictRegion(RectangleF rect, List<PointF> prevPoints, List<PointF> nextPoints)
        {
            var nextPrevPoints = prevPoints.Zip(nextPoints, (prev, next) => new { Prev = prev, Next = next });
            var dxList = new List<float>();
            var dyList = new List<float>();
            foreach (var nextPrevPoint in nextPrevPoints)
            {
                dxList.Add( nextPrevPoint.Next.X - nextPrevPoint.Prev.X );
                dyList.Add( nextPrevPoint.Next.Y - nextPrevPoint.Prev.Y );
            }

            float meanDx = 0.0f;
            float meanDy = 0.0f;
            if (nextPrevPoints.Count() > 0)
            {
                meanDx = dxList.Average();
                meanDy = dyList.Average();
            }

            return new RectangleF(rect.Left + meanDx, rect.Top + meanDy, rect.Width, rect.Height);
        }

        private List<bool> CheckIsOutlierIqr(List<PointF> prevPoints, List<PointF> nextPoints)
        {
            var nextPrevPoints = prevPoints.Zip(nextPoints, (prev, next) => new { Prev = prev, Next = next });
            var dxList = new List<float>();
            var dyList = new List<float>();
            foreach (var nextPrevPoint in nextPrevPoints)
            {
                dxList.Add(nextPrevPoint.Next.X - nextPrevPoint.Prev.X);
                dyList.Add(nextPrevPoint.Next.Y - nextPrevPoint.Prev.Y);
            }

            var xOutlierThreshold = OutlierRangeIqr(dxList);
            var yOutlierThreshold = OutlierRangeIqr(dyList);

            List<bool> isOutlier = new List<bool>();
            for (var i = 0; i < nextPoints.Count; i++)
            {
                if (dxList[i] < xOutlierThreshold.Item1 || dxList[i] > xOutlierThreshold.Item2 ||
                    dyList[i] < yOutlierThreshold.Item1 || dyList[i] > yOutlierThreshold.Item2)
                {
                    isOutlier.Add(true);
                    Console.WriteLine("Found ");
                }
                else
                {
                    isOutlier.Add(false);
                }
            }

            return isOutlier;
        }

        private Tuple<float, float> OutlierRangeIqr(List<float> numbers)
        {
            numbers.Sort();
            int size = numbers.Count;
            int mid = size / 2;

            float q1 = 0;
            float q2 = 0;
            float q3 = 0;

            if (size == 1)
            {
                q1 = numbers[0];
                q2 = numbers[0];
                q3 = numbers[0];
            }
            else if (size % 2 == 0)
            {
                q2 = (numbers[mid - 1] + numbers[mid]) / 2;
                int midMid = mid / 2;
                if (mid % 2 == 0)
                {
                    q1 = (numbers[midMid - 1] + numbers[midMid]) / 2;
                    q3 = (numbers[mid + midMid - 1] + numbers[mid + midMid]) / 2;
                }
                else 
                {
                    q1 = numbers[midMid];
                    q3 = numbers[mid + midMid];
                }
            }
            else
            {
                q2 = numbers[mid];
                if ((size-1) % 4 == 0)
                {
                    int n = (size - 1) / 4;
                    q1 = (numbers[n - 1] * .25f) + (numbers[n] * .75f);
                    q3 = (numbers[3 * n] * .75f) + (numbers[3 * n + 1] * .25f);
                } else if( (size-3) % 4 == 0)
                {
                    int n = (size - 3) / 4;
                    q1 = (numbers[n] * .75f) + (numbers[n + 1] * .25f);
                    q3 = (numbers[3 * n + 1] * .25f) + (numbers[3 * n + 2] * .75f);
                }
            }

            var iqr = q3 - q1;
            return new Tuple<float, float>(q1 - 1.5f * iqr, q3 + 1.5f * iqr);
        }
    }
}
