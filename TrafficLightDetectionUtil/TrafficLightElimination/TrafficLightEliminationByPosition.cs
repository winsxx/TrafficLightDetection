using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using weka.core.converters;
using weka.core;
using weka.classifiers.trees;

namespace TrafficLightDetectionUtil
{
    public class TrafficLightEliminationByPosition : TrafficLightElimination
    {
        RandomForest rf;
        Instances trainData;

        public TrafficLightEliminationByPosition()
        {
            var source = new ConverterUtils.DataSource("../../../data/location_sampled.arff");
            trainData = source.getDataSet();

            trainData.setClassIndex(0);
            rf = new RandomForest();
            rf.setNumTrees(100);
            rf.buildClassifier(trainData);
        }

        public TrafficLightSegmentationResult[] Eliminate(Image<Bgr, byte> frame, TrafficLightSegmentationResult[] trafficLightSegmentationResults)
        {
            var passEliminationList = new List<TrafficLightSegmentationResult>();
            int tlTruePredIndex = trainData.attribute(0).indexOfValue("1");
            Instances unlabeled = new Instances(trainData, 0);
            foreach (TrafficLightSegmentationResult tlResult in trafficLightSegmentationResults)
            {
                var w = tlResult.Region.Width;
                var h = tlResult.Region.Height;
                var fw = frame.Size.Width;
                var fh = frame.Size.Height;
                var t = tlResult.Region.Top;
                double ratio = w / (double)h;
                
                if( w > 1 && w <30 && h > 1 && h < 30 && ratio > 0.5 && ratio < 2.0)
                {
                    Instance ins = new DenseInstance(6);
                    // label as missing value
                    ins.setMissing(0);
                    // boundary height
                    ins.setValue(1, h / (double)fh);
                    // boundary width
                    ins.setValue(2, w / (double)fw);
                    // boundary center y
                    ins.setValue(3, (t + (h-1)/2) / (double)fh);
                    // boundary size
                    ins.setValue(4, Math.Max(w, h) / (double)fh);
                    // boundary ratio
                    ins.setValue(5, ratio);
                    unlabeled.add(ins);

                    var predValue = rf.classifyInstance(unlabeled.lastInstance());
                    if ((int) predValue == tlTruePredIndex)
                    {
                        passEliminationList.Add(tlResult);
                    }
                }
            }
            return passEliminationList.ToArray();
        }
    }
}
