using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightDetectionUtil.emgu_support
{
    public class GlobalConstant
    {
        public static int INSTANCE_CELL_WIDTH = 9;
        public static int INSTANCE_CELL_HEIGHT = 9;
        public static int NUMBER_OF_TRAINING_DATA = 5000;
        public static string CLASSIFIER_TYPE = "rforest";
        public static Hsv BLANK = new Hsv(0, 0, 0);
        public static bool IS_BINARY = false;
        public static int RED_SHIFT = 20;
        public static float RANDOM_TRAINING_SUBSET_SIZE = 0.3f;
        public static int RANDOM_TRAINING_SUBSET_MIN_SIZE = 15;

        public static int HUE_NORMALIZATION_FACTOR = 1;
        public static int SATURATION_NORMALIZATION_FACTOR = 1;
        public static int VALUE_NORMALIZATION_FACTOR = 1;

        public const int CLASS_RED = 0;
        public const int CLASS_GREEN = 1;
        public const int CLASS_YELLOW = 2;
        public const int CLASS_FALSE_RED = 3;
        public const int CLASS_FALSE_GREEN = 4;
        public const int CLASS_FALSE_YELLOW = 5;
    }
}
