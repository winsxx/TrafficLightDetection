using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;
using System.Collections;
using System.Drawing;

namespace TrafficLightDetectionUtil
{
    public class BasicTrafficLightColorSegmentation : TrafficLightColorSegmentation
    {
        private static Hsv GREEN_LOWER_LIMIT = new Hsv(79, 150, 120);
        private static Hsv GREEN_UPPER_LIMIT = new Hsv(91, 255, 255);
        private static Hsv RED1_LOWER_LIMIT = new Hsv(1, 100, 200);
        private static Hsv RED1_UPPER_LIMIT = new Hsv(30, 225, 255);
        private static Hsv RED2_LOWER_LIMIT = new Hsv(167, 100, 200);
        private static Hsv RED2_UPPER_LIMIT = new Hsv(179, 255, 255);
        private static Hsv YELLOW_LOWER_LIMIT = new Hsv(2, 150, 120);
        private static Hsv YELLOW_UPPER_LIMIT = new Hsv(30, 255, 255);

        private Mat kernel;

        public BasicTrafficLightColorSegmentation(uint morphClosingKernelSize)
        {
            // Make sure kernel size id odd
            uint kernelSize = morphClosingKernelSize / 2 * 2 + 1;
            kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Ellipse, 
                new Size((int)kernelSize, (int)kernelSize), new Point(-1, -1));
        }

        public TrafficLightSegmentationResult[] doColorSegmentation(Image<Bgr, byte> image)
        {
            Image<Hsv, Byte> hsvImage = image.Convert<Hsv, Byte>();

            TrafficLightSegmentationResult[] greenSegmen = 
                colorDetectionAndLabeling(hsvImage, GREEN_LOWER_LIMIT, GREEN_UPPER_LIMIT, TrafficLightColorType.Green);
            TrafficLightSegmentationResult[] yellowSegmen =
                colorDetectionAndLabeling(hsvImage, YELLOW_LOWER_LIMIT, YELLOW_UPPER_LIMIT, TrafficLightColorType.Yellow);
            TrafficLightSegmentationResult[] red1Segmen =
                colorDetectionAndLabeling(hsvImage, RED1_LOWER_LIMIT, RED1_UPPER_LIMIT, TrafficLightColorType.Red);
            TrafficLightSegmentationResult[] red2Segmen =
                colorDetectionAndLabeling(hsvImage, RED2_LOWER_LIMIT, RED2_UPPER_LIMIT, TrafficLightColorType.Red);
            List<TrafficLightSegmentationResult> list = new List<TrafficLightSegmentationResult>();
            list.AddRange(greenSegmen);
            list.AddRange(yellowSegmen);
            list.AddRange(red1Segmen);
            list.AddRange(red2Segmen);

            return list.ToArray();
        }

        private TrafficLightSegmentationResult[] colorDetectionAndLabeling(
            Image<Hsv, Byte> hsvImage,
            Hsv lowerBound,
            Hsv upperBound, 
            TrafficLightColorType label)
        {
            // Find hsv color betweern lower bound and upper bound
            Image<Gray, Byte> mask = hsvImage.InRange(lowerBound, upperBound);

            mask = mask.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Close, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar());

            // Find bounding box for each blob
            CvBlobDetector blobDetector = new CvBlobDetector();
            CvBlobs resultingImageBlobs = new CvBlobs();
            uint numBlobsFound = blobDetector.Detect(mask, resultingImageBlobs);

            ArrayList colorDetectionResult = new ArrayList();
            foreach (CvBlob blob in resultingImageBlobs.Values)
            {
                TrafficLightSegmentationResult segResult = new TrafficLightSegmentationResult();
                segResult.region = blob.BoundingBox;
                segResult.colorLabel = label;
            }

            return (TrafficLightSegmentationResult[]) colorDetectionResult.ToArray(typeof(TrafficLightSegmentationResult));
        }
    }
}
