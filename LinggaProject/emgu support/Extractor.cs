using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinggaProject.emgu_support
{
    class Extractor
    {
        public static Hsv[] extractCells(string filename)
        {
            Image<Hsv, Byte> img = new Image<Hsv, Byte>(filename);
            return extractCellsFromImage(img);
        }

        public static Hsv[] extractCellsFromImage(Image<Hsv, Byte> img)
        {
            Hsv[] cells = new Hsv[GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT];
            img = img.Resize(GlobalConstant.INSTANCE_CELL_WIDTH, GlobalConstant.INSTANCE_CELL_HEIGHT, Emgu.CV.CvEnum.Inter.Cubic);

            //Debug.WriteLine("Image size: " + img.Width + ", " + img.Height);
            for (int iy = 0; iy < img.Height; iy++) {
                for (int ix = 0; ix < img.Width; ix++) {
                    cells[iy * GlobalConstant.INSTANCE_CELL_WIDTH + ix] = img[ix, iy];
                    //Debug.WriteLine("Processing: " + (iy * GlobalConstant.INSTANCE_CELL_WIDTH + ix));
                    //Debug.WriteLine("  H: " + img[ix, iy].Hue);
                    //Debug.WriteLine("  S: " + img[ix, iy].Satuation);
                    //Debug.WriteLine("  V: " + img[ix, iy].Value);
                }
            }
            return cells;
        }
    }
}
