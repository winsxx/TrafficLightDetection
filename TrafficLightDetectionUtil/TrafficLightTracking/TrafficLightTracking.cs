using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace TrafficLightDetectionUtil
{
    public interface TrafficLightTracking
    {
        Rectangle[] Track(
            Image<Bgr, byte> prevFrame,
            Image<Bgr, byte> currentFrame,
            Rectangle[] prevBoundingBox);

        Rectangle[] ContinueTrack(Image<Bgr, byte> currentFrame);
    }
}
