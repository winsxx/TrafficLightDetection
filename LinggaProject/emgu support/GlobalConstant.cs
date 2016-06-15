using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.emgu_support
{
    class GlobalConstant
    {
        public static int INSTANCE_CELL_WIDTH = 9;
        public static int INSTANCE_CELL_HEIGHT = 9;
        public static int NUMBER_OF_TRAINING_DATA = 100;
        public static string CLASSIFIER_TYPE = "bayes";
        public static Hsv BLANK = new Hsv(0, 0, 0);

        public const int CLASS_RED = 0;
        public const int CLASS_GREEN = 1;
        public const int CLASS_YELLOW = 2;
        public const int CLASS_FALSE_RED = 3;
        public const int CLASS_FALSE_GREEN = 4;
        public const int CLASS_FALSE_YELLOW = 5;
    }
}
