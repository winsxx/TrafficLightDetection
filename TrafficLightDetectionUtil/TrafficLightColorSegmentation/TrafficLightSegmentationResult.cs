using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightDetectionUtil
{
    public class TrafficLightSegmentationResult
    {
        public Rectangle Region {get; set;}
        public TrafficLightColorType ColorLabel { get; set; }
    }
}
