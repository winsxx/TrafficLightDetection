using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.support
{
    class TrafficLightInstance
    {
        // posisi
        public int x;
        public int y;
        // ukuran
        public float height;
        public float width;
        // color
        public HSV[] colors;
        public int tl_class;
        // classstring
        private string[] class_string;

        public TrafficLightInstance()
        {
            class_string = new string[3] { "red", "green", "yellow" };
            colors = new HSV[Constants.size * Constants.size];
        }

        public void print()
        {
            Console.WriteLine("height: " + height + " width: " + width + " class: " + tl_class);
            for (int i = 0; i < Constants.size * Constants.size; i++) {
                Console.WriteLine(i + " (" + colors[i].H + ", " + colors[i].S + ", " + colors[i].V + ")");
            }
        }

        public string toString()
        {
            return "TL: " + x + ", " + y + ": " + class_string[tl_class];
        }

        public int getCenterX()
        {
            return (int)(x + 0.5 * width);
        }

        public int getCenterY()
        {
            return (int)(y + 0.5 * height);
        }
    }
}
