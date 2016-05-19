﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;

namespace TrafficLightDetectionUtil
{
    public interface TrafficLightElimination
    {
        TrafficLightSegmentationResult[] Eliminate(
            Image<Bgr, byte> frame,
            TrafficLightSegmentationResult[] trafficLightSegmentationResults);
    }
}
