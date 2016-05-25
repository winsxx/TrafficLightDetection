using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TrafficLightDetectionUtil
{
    public class TrafficLightEliminationBySimpleSize : TrafficLightElimination
    {
        public TrafficLightSegmentationResult[] Eliminate(Image<Bgr, byte> frame, TrafficLightSegmentationResult[] trafficLightSegmentationResults)
        {
            var passEliminationList = new List<TrafficLightSegmentationResult>();
            foreach (TrafficLightSegmentationResult tlResult in trafficLightSegmentationResults)
            {
                var w = tlResult.Region.Width;
                var h = tlResult.Region.Height;
                double ratio = w / (double)h;
                if (w > 1 && w < 30 && h > 1 && h < 30 && ratio > 0.5 && ratio < 2.0)
                {
                    passEliminationList.Add(tlResult);
                }
            }
            return passEliminationList.ToArray();
        }
    }
}
