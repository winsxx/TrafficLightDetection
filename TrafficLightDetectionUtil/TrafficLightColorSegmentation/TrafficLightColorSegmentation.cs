using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightDetectionUtil
{
    public interface TrafficLightColorSegmentation
    {
        TrafficLightSegmentationResult[] DoColorSegmentation(Image<Bgr, Byte> image);
    }
}
