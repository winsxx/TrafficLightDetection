using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace WinsonProject
{
    public partial class MainView : Form
    {
        //// Var ////
        Capture cap = null;
        bool process = false;
        bool cam = true;
        bool v1 = true;
        Image<Bgr, Byte> imgOri;
        Image<Gray, Byte> imgPro;
        Image<Gray, Byte> imgR;
        Image<Gray, Byte> imgY;
        Image<Gray, Byte> imgG;
        double totalCaptured = 0;
        double totalTime = 0;
        double averageTime;
        double frames = 0;
        double accTime = 0;
        double fps;
        int accTreshold = 25;
        String Video = "../../../data/singapore01.mp4";
        String Video2 = "../../../data/singapore01.mp4";
        int y;
        int min_rad = 5;
        int max_rad = 30;
        public MainView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (cam == true)
            {
                try { cap = new Capture(); }
                catch (NullReferenceException except)
                {
                    tb1.Text = except.Message;
                    return;
                }
            }

            else
            {
                try { cap = new Capture(@Video); }
                catch (NullReferenceException except)
                {
                    tb1.Text = except.Message;
                    return;
                }

            }
            Application.Idle += processFrameUpdateGUI;
            process = true;
        }

        void processFrameUpdateGUI(object sender, EventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            imgOri = cap.QueryFrame().ToImage<Bgr, byte>();
            if (imgOri == null) return;                                         // if didn't get the frame, 
            y = imgOri.Height / 2;

            label1.BackColor = Color.FromArgb(240, 240, 240);
            label2.BackColor = Color.FromArgb(240, 240, 240);
            label3.BackColor = Color.FromArgb(240, 240, 240);

            if (cam == false)
            {
                imgR = imgOri.InRange(new Bgr(0, 0, 200),
                                              new Bgr(100, 100, 256));
                imgY = imgOri.InRange(new Bgr(0, 100, 200),
                                                new Bgr(80, 256, 256));
                imgG = imgOri.InRange(new Bgr(150, 150, 0),
                                                new Bgr(256, 256, 80));
                imgPro = imgR + imgY + imgG;
            }

            else
            {
                /*
                imgR = imgOri.InRange(new Bgr(130, 130, 150),
                                                   new Bgr(150, 150, 240));
                imgY = imgOri.InRange(new Bgr(100, 150, 150),
                                                new Bgr(150, 200, 200));
                imgG = imgOri.InRange(new Bgr(160, 200, 200),
                                                new Bgr(180, 251, 251));
                */
                imgR = imgOri.InRange(new Bgr(0, 0, 150),
                                                    new Bgr(100, 100, 256));
                imgY = imgOri.InRange(new Bgr(0, 110, 150),
                                                new Bgr(120, 256, 256));
                imgG = imgOri.InRange(new Bgr(100, 150, 60),
                                                new Bgr(200, 256, 140));
                imgPro = imgR + imgY + imgG;

            }

            imgPro = imgPro.SmoothGaussian(7);

            CircleF[] cR = imgR.HoughCircles(new Gray(100),                // canny treshold
                                               new Gray(accTreshold),                // accumulator treshold
                                               2,                           // accumulator resolution
                                               imgPro.Height / 4,           // 
                                               min_rad,                          // min radius circles
                                               max_rad)[0];                     // max radius circle, get circles from the first channel

            foreach (CircleF circle in cR)
            {
                watch.Stop();
                CvInvoke.Circle(imgOri, new Point((int)circle.Center.X, (int)circle.Center.Y), 3, new MCvScalar(0, 255, 0), -1, LineType.AntiAlias, 0);

                if (circle.Center.Y <= y)
                {
                    imgOri.Draw(circle, new Bgr(0, 0, 255), 3);
                    tb1.AppendText("Lampu merah terdeteksi, awas berhenti. \t" + "radius : " + circle.Radius.ToString() + "\t" + "x : " + circle.Center.X.ToString() + "\t y : " + circle.Center.Y.ToString() + "\t time : " + watch.Elapsed.TotalMilliseconds + " miliseconds" + " \n");
                    label1.BackColor = Color.FromArgb(255, 0, 0);
                    totalCaptured += 1;
                    totalTime += watch.Elapsed.TotalMilliseconds;
                }
            }

            CircleF[] cY = imgY.HoughCircles(new Gray(100),                // canny treshold
                                               new Gray(accTreshold),      // accumulator treshold
                                               2,                          // accumulator resolution
                                               imgPro.Height / 4,          // 
                                               min_rad,                    // min radius circles
                                               max_rad)[0];                // max radius circle, get circles from the first channel

            foreach (CircleF circle in cY)
            {
                watch.Stop();
                CvInvoke.Circle(imgOri, new Point((int)circle.Center.X, (int)circle.Center.Y), 3, new MCvScalar(0, 255, 0), -1, LineType.AntiAlias, 0);

                if (circle.Center.Y <= y)
                {
                    imgOri.Draw(circle, new Bgr(0, 0, 255), 3);
                    tb1.AppendText("Lampu kuning terdeteksi, harap berhati-hati. \t" + "x : " + circle.Center.X.ToString() + "\t y : " + circle.Center.Y.ToString() + "\t time : " + watch.Elapsed.TotalMilliseconds + " miliseconds" + " \n");
                    label2.BackColor = Color.FromArgb(255, 200, 0);
                    totalCaptured += 1;
                    totalTime += watch.Elapsed.TotalMilliseconds;
                }
            }
            CircleF[] cG = imgG.HoughCircles(new Gray(100),                // canny treshold
                                               new Gray(accTreshold),      // accumulator treshold
                                               2,                          // accumulator resolution
                                               imgPro.Height / 4,          // 
                                               min_rad,                    // min radius circles
                                               max_rad)[0];                // max radius circle, get circles from the first channel

            foreach (CircleF circle in cG)
            {
                watch.Stop();
                CvInvoke.Circle(imgOri, new Point((int)circle.Center.X, (int)circle.Center.Y), 3, new MCvScalar(0, 255, 0), -1, LineType.AntiAlias, 0);

                if (circle.Center.Y <= y)
                {
                    imgOri.Draw(circle, new Bgr(0, 0, 255), 3);
                    tb1.AppendText("Lampu hijau terdeteksi, silahkan maju. \t" + "radius : " + circle.Radius.ToString() + "\t" + "x : " + circle.Center.X.ToString() + "\t y : " + circle.Center.Y.ToString() + "\t time : " + watch.Elapsed.TotalMilliseconds + " miliseconds" + " \n");
                    label3.BackColor = Color.FromArgb(0, 255, 255);
                    totalCaptured += 1;
                    totalTime += watch.Elapsed.TotalMilliseconds;
                }
            }

            accTime += watch.Elapsed.TotalMilliseconds;
            frames += 1;

            ib1.Image = imgOri;
            ib2.Image = imgPro;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (process == true)
            {
                Application.Idle -= processFrameUpdateGUI;
                process = false;
                fps = frames / accTime * 1000;
                button1.Text = "Resume";
                averageTime = totalTime / totalCaptured;
                tb1.AppendText("Rata-rata waktu yang dibutuhkan : \t" + averageTime + " miliseconds \n");
                tb1.AppendText("kecepatan proses : \t" + fps + " fps \n");
            }
            else
            {
                Application.Idle += processFrameUpdateGUI;
                process = true;
                button1.Text = "Pause";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cap != null)
            {
                cap.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tb1.Text = "";
            try { cap = new Capture(@Video); }
            catch (NullReferenceException except)
            {
                tb1.Text = except.Message;
                return;
            }
            totalTime = 0;
            totalCaptured = 0;
            cam = false;
            v1 = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cam == false)
            {
                tb1.Text = "";
                try { cap = new Capture(); }
                catch (NullReferenceException except)
                {
                    tb1.Text = except.Message;
                    return;
                }
                totalTime = 0;
                totalCaptured = 0;
                cam = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tb1.Text = "";
            try { cap = new Capture(@Video2); }
            catch (NullReferenceException except)
            {
                tb1.Text = except.Message;
                return;
            }
            totalTime = 0;
            totalCaptured = 0;
            cam = false;
            v1 = false;
        }
    }
}
