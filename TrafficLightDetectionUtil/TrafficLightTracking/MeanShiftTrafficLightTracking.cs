using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace TrafficLightDetectionUtil
{
    public class MeanShiftTrafficLightTracking : TrafficLightTracking
    {
        List<Mat> _histList;
        List<TrafficLightSegmentationResult> _prevBoundingBox;

        public TrafficLightSegmentationResult[] Track(Image<Bgr, byte> prevFrame, Image<Bgr, byte> currentFrame, TrafficLightSegmentationResult[] prevBoundingBox)
        {
            #region find histogram
            var prevFrameHsv = prevFrame.Convert<Hsv, byte>();
            var currentFrameHsv = currentFrame.Convert<Hsv, byte>();
            _prevBoundingBox = new List<TrafficLightSegmentationResult>(prevBoundingBox);
            _histList = new List<Mat>();
            var mask = new Image<Gray, byte>(prevFrame.Width, prevFrame.Height);
            VectorOfMat vm1 = new VectorOfMat();
            vm1.Push(prevFrameHsv);
            foreach (var prevRect in _prevBoundingBox)
            {
                Mat roiHist = new Mat();
                CvInvoke.Rectangle(mask, prevRect.Region, new MCvScalar(255, 255, 255));
                CvInvoke.CalcHist(vm1, new int[] { 0 }, mask, roiHist, new int[] { 180 }, new float[] { 0, 180 }, false);
                CvInvoke.Rectangle(mask, prevRect.Region, new MCvScalar(0, 0, 0));
                CvInvoke.Normalize(roiHist, roiHist, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
                _histList.Add(roiHist);
            }
            #endregion

            #region tracking
            VectorOfMat vm2 = new VectorOfMat();
            vm2.Push(currentFrameHsv);
            for (var i=0; i<_prevBoundingBox.Count; i++)
            {
                Mat backProj = new Mat();
                CvInvoke.CalcBackProject(vm2, new int[] { 0 }, _histList[i], backProj, new float[] { 0, 180 }, 1);
                Rectangle boundingBox = _prevBoundingBox[i].Region;
                CvInvoke.MeanShift(backProj, ref boundingBox, new MCvTermCriteria(10, 1));
                _prevBoundingBox[i].Region = boundingBox;
            }
            #endregion

            return _prevBoundingBox.ToArray();
        }

        public TrafficLightSegmentationResult[] ContinueTrack(Image<Bgr, byte> currentFrame)
        {
            var currentFrameHsv = currentFrame.Convert<Hsv, byte>();
            #region tracking
            VectorOfMat vm2 = new VectorOfMat();
            vm2.Push(currentFrameHsv);
            for (var i = 0; i < _prevBoundingBox.Count; i++)
            {
                Mat backProj = new Mat();
                CvInvoke.CalcBackProject(vm2, new int[] { 0 }, _histList[i], backProj, new float[] { 0, 180 }, 1);
                Rectangle boundingBox = _prevBoundingBox[i].Region;
                CvInvoke.MeanShift(backProj, ref boundingBox, new MCvTermCriteria(10, 1));
                _prevBoundingBox[i].Region = boundingBox;
            }
            #endregion

            return _prevBoundingBox.ToArray();
        }
    }
}
