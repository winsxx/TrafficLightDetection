using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;

namespace TrafficLightDetectionUtil.TrafficLightShapeClassifier
{
    public interface TrafficLightShapeClassifier
    {
        TrafficLightRecognitionResult[] Classify(
            Image<Bgr, byte> frame,
            TrafficLightSegmentationResult[] trafficLightSegmentationResult);
    }
}
