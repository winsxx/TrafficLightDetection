﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LinggaProject.emgu_support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinggaProject
{
    public partial class EmguImageTestForm : Form
    {
        Tester tester;
        public EmguImageTestForm()
        {
            tester = new Tester();
            InitializeComponent();
        }

        private void browseImageButton_Click(object sender, EventArgs e)
        {
            DialogResult result = testImageDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                Image<Hsv, Byte> img = new Image<Hsv, byte>(testImageDialog.FileName);
                foreach (KeyValuePair<Rectangle, int> classifiedInstance in tester.imageTestingFromFile(testImageDialog.FileName)) {
                    Debug.WriteLine("detected" + classifiedInstance.Value);
                    Hsv color = new Hsv();
                    Rectangle rect = classifiedInstance.Key;
                    switch (classifiedInstance.Value) {
                        case 0:
                            color = new Hsv(179, 255, 255);
                            break;
                        case 1:
                            color = new Hsv(65, 255, 255);
                            break;
                        case 2:
                            color = new Hsv(30, 255, 255);
                            break;
                        default:
                            color = new Hsv(0, 0, 0);
                            break;
                    }
                    // only print detected
                    CvInvoke.Rectangle(img, rect, color.MCvScalar, 1, LineType.FourConnected);
                }

                testImageBox.Image = img;

                //Image<Bgr, Byte> imgBgr = new Image<Bgr, byte>(testImageDialog.FileName);

                //Image<Gray, Byte> imgR = imgBgr.InRange(new Bgr(0, 0, 200),
                //                              new Bgr(100, 100, 256));
                //Image<Gray, Byte> imgY = imgBgr.InRange(new Bgr(0, 100, 200),
                //                                new Bgr(80, 256, 256));
                //Image<Gray, Byte> imgG = imgBgr.InRange(new Bgr(150, 150, 0),
                //                                new Bgr(256, 256, 80));
                //Image<Gray, Byte> imgPro = imgR + imgY + imgG;

                //testImageBox.Image = imgPro;
            }
        }
    }
}
