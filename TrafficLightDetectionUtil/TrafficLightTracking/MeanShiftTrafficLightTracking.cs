using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TrafficLightDetectionUtil
{
    class MeanShiftTrafficLightTracking : TrafficLightTracking
    {
        public Rectangle[] Track(Image<Bgr, byte> prevFrame, Image<Bgr, byte> currentFrame, Rectangle[] prevBoundingBox)
        {
            throw new NotImplementedException();
        }

        public Rectangle[] ContinueTrack(Image<Bgr, byte> currentFrame)
        {
            throw new NotImplementedException();
        }
    }
}
