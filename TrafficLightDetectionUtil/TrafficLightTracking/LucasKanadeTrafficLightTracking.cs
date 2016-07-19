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
        TrafficLightColorType[] _prevColors;
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

        public TrafficLightSegmentationResult[] Track(Image<Bgr, byte> prevFrame, Image<Bgr, byte> currentFrame, TrafficLightSegmentationResult[] prevBoundingBox)
        {
            var prevGrayFrame = prevFrame.Convert<Gray, byte>();
            var currentGrayFrame = currentFrame.Convert<Gray, byte>();
            var mask = new Image<Gray, byte>(prevFrame.Width, prevFrame.Height);
            List<RectangleF> nextBoundingBox = new List<RectangleF>();
            List<List<PointF>> listOfPointList = new List<List<PointF>>();
            List<TrafficLightColorType> nextColors = new List<TrafficLightColorType>();

            foreach (TrafficLightSegmentationResult prevResult in prevBoundingBox)
            {
                #region feature detection
                var padWidth = prevResult.Region.Width * 20/100;
                var padHeight = prevResult.Region.Height * 20/100;

                #region give padding
                int left = prevResult.Region.Left - padWidth;
                if (left < 0) left = 0;
                int top = prevResult.Region.Top - padHeight;
                if (top < 0) top = 0;
                int right = prevResult.Region.Right + padWidth;
                if (right >= prevFrame.Width) right = prevFrame.Width - 1;
                int bottom = prevResult.Region.Bottom + padHeight;
                if (top >= prevFrame.Height) bottom = prevFrame.Height - 1;
                Rectangle maskRegion = new Rectangle(left, top, right - left + 1, bottom - top + 1);
                #endregion

                CvInvoke.Rectangle(mask, maskRegion, new MCvScalar(255, 255, 255));
                MKeyPoint[] keyPoints = _featureDetector.Detect(prevGrayFrame, mask: mask);
                CvInvoke.Rectangle(mask, maskRegion, new MCvScalar(0, 0, 0));
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
                        var nextRect = PredictRegion(prevResult.Region, prevPts, nextPts);
                        nextBoundingBox.Add(nextRect);
                        nextColors.Add(prevResult.ColorLabel);
                    }
                }
                #endregion

            }

            _prevPointLists = listOfPointList;
            _prevBoundingBox = nextBoundingBox.ToArray();
            _prevFrame = currentFrame;
            _prevColors = nextColors.ToArray();

            return createResults(_prevBoundingBox, _prevColors);
        }

        public TrafficLightSegmentationResult[] ContinueTrack(Image<Bgr, byte> currentFrame)
        {
            var currentGrayFrame = currentFrame.Convert<Gray, byte>();
            var prevGrayFrame = _prevFrame.Convert<Gray, byte>();
            List<RectangleF> nextBoundingBox = new List<RectangleF>();
            List<List<PointF>> listOfPointList = new List<List<PointF>>();
            List<TrafficLightColorType> nextColors = new List<TrafficLightColorType>();

            for (var bbIndex=0; bbIndex < _prevBoundingBox.Length; bbIndex++)
            {
                #region tracking
                PointF[] nextPtsArray = null;
                byte[] status = null;
                float[] err = null;

                CvInvoke.CalcOpticalFlowPyrLK(prevGrayFrame, currentGrayFrame, _prevPointLists[bbIndex].ToArray(), 
                    new Size(15, 15), 2, new MCvTermCriteria(10, 0.03), out nextPtsArray, out status, out err);

                List<PointF> prevPts = new List<PointF>();
                List<PointF> nextPts = new List<PointF>();
                for (var i = 0; i < status.Length; i++)
                {
                    if (status[i] > 0)
                    {
                        prevPts.Add(_prevPointLists[bbIndex][i]);
                        nextPts.Add(nextPtsArray[i]);
                    }
                }

                if (nextPts.Count > 0)
                {
                    listOfPointList.Add(nextPts);
                    var nextRect = PredictRegion(_prevBoundingBox[bbIndex], prevPts, nextPts);
                    nextBoundingBox.Add(nextRect);
                    nextColors.Add(_prevColors[bbIndex]);
                }
                #endregion
            }
            _prevPointLists = listOfPointList;
            _prevBoundingBox = nextBoundingBox.ToArray();
            _prevFrame = currentFrame;
            _prevColors = nextColors.ToArray();
            return createResults(_prevBoundingBox, _prevColors);
        }

        private TrafficLightSegmentationResult[] createResults(RectangleF[] rects, TrafficLightColorType[] colors)
        {
            var results = new TrafficLightSegmentationResult[rects.Length];
            for(var i = 0; i <results.Length; i++)
            {
                results[i] = new TrafficLightSegmentationResult(
                    new Rectangle((int)rects[i].Left, (int)rects[i].Top, (int)rects[i].Width, (int)rects[i].Height), colors[i]);
            }
            return results;
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
