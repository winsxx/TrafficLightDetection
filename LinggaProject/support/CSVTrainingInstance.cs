using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.support
{
    class CSVTrainingInstance
    {
        public string filename;
        public string status;
        public Point upper_left;
        public Point lower_right;

        public void print()
        {
            Console.WriteLine("file: " + filename + " boundary: (" + upper_left.X + ", " + upper_left.Y + "), (" +
                lower_right.X + ", " + lower_right.Y + ") go: " + status);
        }
    }
}
