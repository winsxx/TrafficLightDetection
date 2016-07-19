using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TrafficLightDetection
{
    public partial class MainView : Form
    {
        private Capture cap = null;
        private Timer timer = new Timer();
        private int FPS = 40;

        public MainView()
        {
            InitializeComponent();

            timer.Interval = 1000 / FPS;
            timer.Tick += ProcessVideo;
        }

        // Browse Button
        private void browse_image_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();
            FD.Title = "Choose Image";
            //FD.InitialDirectory = @"C:\Users\Stephen Djohan\Documents\Visual Studio 2015\Projects\TrafficLightRecognition\TrafficLightRecognition\video";
            FD.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            FD.FilterIndex = 2;
            FD.RestoreDirectory = true;
            if (FD.ShowDialog() == DialogResult.OK)
            {
                ProcessImage(new Image<Bgr, byte>(FD.FileName));
            }
        }

        private void browse_video_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();
            FD.Title = "Choose Image";
            //FD.InitialDirectory = @"C:\Users\Stephen Djohan\Documents\Visual Studio 2015\Projects\TrafficLightRecognition\TrafficLightRecognition\video";
            FD.Filter = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                        " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";
            FD.FilterIndex = 2;
            FD.RestoreDirectory = true;
            if (FD.ShowDialog() == DialogResult.OK)
            {
                try { cap = new Capture(FD.FileName); }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.InnerException);
                    return;
                }
                
                timer.Start();
            }
        }

        private void ProcessVideo(object sender, EventArgs e)
        {
            Image<Bgr, byte> imgOri = cap.QueryFrame().ToImage<Bgr, Byte>();

            if (imgOri != null)
            {
                Video_Image_1.Image = imgOri.Bitmap;
            }
            else
            {
                timer.Stop();
            }            
        }

        private void ProcessImage(Image<Bgr, byte> imgOri)
        {
            Video_Image_1.Image = imgOri.Bitmap;
        }
    }
}
