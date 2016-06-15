using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.support
{
    class Constants
    {
        public static int size = 9;
        private static int code = 1;

        public static weka.classifiers.Classifier useClassifier()
        {
            switch (code) {
                case 0:
                    Console.WriteLine("use MLP");
                    return new weka.classifiers.functions.MultilayerPerceptron();
                case 1:
                    Console.WriteLine("use RandomForest");
                    return new weka.classifiers.trees.RandomForest();
                default:
                    Console.WriteLine("use J48");
                    weka.classifiers.trees.J48 tree = new weka.classifiers.trees.J48();
                    tree.setBinarySplits(true);
                    return tree;
            }
        }
    }
}
