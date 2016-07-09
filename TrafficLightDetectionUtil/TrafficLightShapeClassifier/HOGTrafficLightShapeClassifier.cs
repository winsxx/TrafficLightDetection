using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace TrafficLightDetectionUtil.TrafficLightShapeClassifier
{
    public class HOGTrafficLightShapeClassifier : TrafficLightShapeClassifier
    {
        private HOGDescriptor hog;  // HOG descriptor for feature extraction
        private int descriptorSize; // number of HOG features
        private SVM svm;            // svm classifier for classifying
        
        public HOGTrafficLightShapeClassifier()
        {
            hog = new HOGDescriptor(
                new Size(40, 40),   // win_size
                new Size(8, 8),     // block_size
                new Size(4, 4),     // block_stride
                new Size(4, 4),     // cell_size
                    9,              // nbins
                    1,              // deriv_aperture
                    -1,             // win_sigma
                    0.2,            // L2HysThreshold
                    true            // gamma correction
            );

            descriptorSize = Convert.ToInt32(hog.DescriptorSize);

            svm = new SVM();
            FileStorage fsr = new FileStorage("../../data/svm.xml", FileStorage.Mode.Read);
            svm.Read(fsr.GetFirstTopLevelNode());
        }

        private float[] GetHOGVector(Image<Bgr, byte> im)
        {
            Image<Bgr, byte> imageOfInterest = im.Resize(40, 40, Inter.Linear);
            float[] result = hog.Compute(imageOfInterest);
            return result;
        }

        public TrafficLightRecognitionResult[] Classify(Image<Bgr, byte> frame, TrafficLightSegmentationResult[] trafficLightSegmentationResults)
        {
            TrafficLightRecognitionResult[] trafficLightRecognitionResults = new TrafficLightRecognitionResult[trafficLightSegmentationResults.Length];

            for(int i = 0; i < trafficLightSegmentationResults.Length; ++i) {
                frame.ROI = trafficLightSegmentationResults[i].Region;
                float[] HOGDescriptor = GetHOGVector(frame);

                // Copy the features to prediction matrix
                Matrix<float> test = new Matrix<float>(1, descriptorSize);
                for (int j = 0; j < test.Cols; ++j)
                {
                    test[0, j] = HOGDescriptor[j];
                }
                frame.ROI = Rectangle.Empty;

                // Predict the shape of traffic light
                float prediction = svm.Predict(test);
                TrafficLightShapeType shape = TrafficLightShapeType.Unknown;

                switch ((int)prediction)
                {
                    case 0 : shape = TrafficLightShapeType.Circle; break;
                    case 1 : shape = TrafficLightShapeType.Left_Arrow; break;
                    case 2 : shape = TrafficLightShapeType.Right_Arrow; break;
                    case 3 : shape = TrafficLightShapeType.Unknown; break;
                }

                trafficLightRecognitionResults[i] = new TrafficLightRecognitionResult(
                    trafficLightSegmentationResults[i].Region,
                    shape
                );
            }

            return trafficLightRecognitionResults;
        }
    }
}
