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
            int filled = 0;
            Hsv[] cells = new Hsv[GlobalConstant.INSTANCE_CELL_WIDTH * GlobalConstant.INSTANCE_CELL_HEIGHT];
            img = img.Resize(GlobalConstant.INSTANCE_CELL_WIDTH, GlobalConstant.INSTANCE_CELL_HEIGHT, Emgu.CV.CvEnum.Inter.Cubic);

            int width = GlobalConstant.INSTANCE_CELL_WIDTH;
            int height = GlobalConstant.INSTANCE_CELL_HEIGHT;

            int completed = width * height;

            Parallel.For(0, height,
                iy => {
                    Parallel.For(0, width,
                       ix => {
                           cells[iy * GlobalConstant.INSTANCE_CELL_WIDTH + ix] = img[ix, iy];
                           filled++;
                           //Debug.WriteLine(filled);
                       });
                });
            return cells;
        }
    }
}
