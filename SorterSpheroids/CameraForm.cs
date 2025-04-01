using Emgu.CV.CvEnum;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Features2D;
using Emgu.CV.Linemod;
using Emgu.CV.Structure;
using System.IO;
using System.Threading;

namespace SorterSpheroids
{
    public partial class CameraForm : Form
    {
        VideoCapture capture;
        Size  cameraSize = new Size(1920,1080);
        List<Mat> video_mats = new List<Mat>();
        int videoframe_counts = 0;
        int videoframe_counts_stop = 100000;
        int fps = 30;
        public CameraForm()
        {
            InitializeComponent();
        }

        private void but_con_cam_Click(object sender, EventArgs e)
        {
            var num = int.Parse(textBox_camera_number.Text);
            videoStart(num);
        }

        private void videoStart(int number)
        {
            capture = new VideoCapture(number);
            capture.Set(CapProp.FrameWidth, cameraSize.Width);

            cameraSize.Width = (int)capture.Get(CapProp.FrameWidth);
            cameraSize.Height = (int)capture.Get(CapProp.FrameHeight);
            fps = (int)capture.Get(CapProp.Fps);
            Console.WriteLine(cameraSize.Width.ToString() + " " + cameraSize.Height.ToString() + " " + fps);
            capture.ImageGrabbed += capturingVideo;
            capture.Start();
        }
        void drawCameras(VideoCapture cap)
        {
            //Console.WriteLine("dr_cam");
            var mat = new Mat();
            cap.Retrieve(mat);
            imageBox_main.Image = mat;
            imProcess(mat);
            videoframe_counts++;
        }
        void capturingVideo(object sender, EventArgs e)
        {
            drawCameras((VideoCapture)sender);
        }
        void imProcess(Mat mat)
        {

            if (videoframe_counts > 0 && videoframe_counts < videoframe_counts_stop)
            {

            }
            else
            {
                if (video_mats != null)
                {
                    try
                    {
                        save_video(cameraSize.Width, cameraSize.Height); 
                    }
                    catch
                    {
                    }
                }
            }
                
        }
        void save_video(int w, int h)
        {

            int fcc = VideoWriter.Fourcc('h', '2', '6', '4'); //'M', 'J', 'P', 'G';'m', 'p', '4', 'v';'M', 'P', '4', 'V';'H', '2', '6', '4';'h', '2', '6', '4'
            int fps = 30;
            var dir = "recordings";
            Directory.CreateDirectory(dir);
            var video_scan_name = "viseo_"+(new DateTime()).ToLongDateString();
            string name = dir + "\\" + video_scan_name + ".mp4";
            Console.WriteLine("wr" + " " + w + " " + h + " " + fps);
            var video_writer = new VideoWriter(name, -1, fps, new Size(w, h), true);
            //var reswr = video_writer[ind ].Set(VideoWriter.WriterProperty.Quality, 100);
            //Console.WriteLine(reswr);
            for (int i = 0; i < video_mats.Count ; i++)
            {
                video_writer.Write(video_mats[i]);
                //var p = Detection.detectLineSensor(video_mats[ind - 1][i])[0];
                //Console.WriteLine(ind + " "  + p);
            }
            video_mats = null;
            video_writer.Dispose();



        }

        private void but_start_recording_Click(object sender, EventArgs e)
        {
            videoframe_counts = 0;
            videoframe_counts_stop = 10000;
        }

        private void but_stop_recording_Click(object sender, EventArgs e)
        {
            videoframe_counts_stop = 0;
        }
    }
}
