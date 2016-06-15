using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.emgu_support
{
    class Instance
    {
        public Rectangle rect;
        public Hsv[] cells;
        public int positiveClass;

        public Instance (Rectangle rect, Hsv[] cells, int positiveClass)
        {
            this.rect = rect;
            this.cells = cells;
            this.positiveClass = positiveClass;
        }
    }
}
