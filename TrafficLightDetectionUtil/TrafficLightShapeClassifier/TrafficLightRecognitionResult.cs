using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightDetectionUtil.TrafficLightShapeClassifier
{
    public class TrafficLightRecognitionResult
    {
        public Rectangle Region { get; set; }
        public TrafficLightShapeType ShapeLabel { get; set; }

        public TrafficLightRecognitionResult()
        {

        }

        public TrafficLightRecognitionResult(Rectangle reg, TrafficLightShapeType shape)
        {
            Region = reg;
            ShapeLabel = shape;
        }
    }
}
