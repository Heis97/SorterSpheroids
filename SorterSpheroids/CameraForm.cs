
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Text.RegularExpressions;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace SorterSpheroids
{
    public partial class CameraForm : Form
    {
        VideoCapture capture;
        OpenCvSharp.Size  cameraSize = new OpenCvSharp.Size(1920,1080);
        List<Mat> video_mats = new List<Mat>();
        int videoframe_counts = -1;
        int videoframe_counts_stop = 10000;
        int fps = 30;
        enum paint_mode { draw_centr,none};
        double cur_contr = 0;
        bool recording = false;
        MainForm mainForm;


        public CameraForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void but_con_cam_Click(object sender, EventArgs e)
        {
            var num = int.Parse(textBox_camera_number.Text);
            videoStart(num);
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker)sender;

            while (!bgWorker.CancellationPending)
            {
                var frameMat = new Mat();
                if (capture.Read(frameMat))
                // using (var frameMat = capture1.RetrieveMat())
                {
                    if (frameMat.Width != 0)
                    {
                        bgWorker.ReportProgress(0, frameMat);
                        Thread.Sleep(10);
                    }

                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            var frameMat = (Mat)e.UserState;
            frameMat = imProcess(frameMat);
            if (recording) videoframe_counts++;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = BitmapConverter.ToBitmap(frameMat);
        }

        private void videoStart(int number)
        {
            capture = new VideoCapture(); // Первая камера
                                           //  capture2 = new VideoCapture(); // Вторая камера
            capture.Open(number, VideoCaptureAPIs.DSHOW);
            // capture2.Open(0, VideoCaptureAPIs.ANY);

            if (!capture.IsOpened())
            {
                MessageBox.Show("Не удалось открыть камеру 1 (индекс 1). Проверьте подключение или индекс.");
                return;
            }
            capture.Set(VideoCaptureProperties.FrameWidth, 1920);
            capture.Set(VideoCaptureProperties.FrameHeight, 1080);

            capture.Set(VideoCaptureProperties.Fps, 40);
            capture.Set(VideoCaptureProperties.FourCC, OpenCvSharp.VideoWriter.FourCC("MJPG"));

            var fps = capture.Get(VideoCaptureProperties.Fps);
            var w = capture.Get(VideoCaptureProperties.FrameWidth);
            var h = capture.Get(VideoCaptureProperties.FrameHeight);

            ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);
            backgroundWorker1.RunWorkerAsync();
            Console.WriteLine(w + " " + h + " " + fps);

        }
        Mat imProcess(Mat mat)
        {
            
            if (mat == null) return null;
            if (video_mats != null)
            {
                
                if (videoframe_counts > 0 && videoframe_counts < videoframe_counts_stop)
                {

                    video_mats.Add(mat.Clone());
                }
                else if(videoframe_counts > videoframe_counts_stop)
                {
                    save_video(cameraSize.Width, cameraSize.Height);
                }

                if (focal_area)
                {
                   // (mat, cur_contr) = ImageProcessing.get_focal_surface_for_conf(mat);
                 //   Console.WriteLine(cur_contr.ToString());
                }
                if (centr_object)
                {
                   // Cv2.DrawMarker(mat)
                    //(mat, cur_contr) = ImageProcessing.get_focal_surface_for_conf(mat);
                    //Console.WriteLine(cur_contr.ToString());
                }
            }
            return mat;

        }

        void save_video(int w, int h)
        {
            recording = false;
            int fcc = VideoWriter.FourCC('h', '2', '6', '4'); //'M', 'J', 'P', 'G';'m', 'p', '4', 'v';'M', 'P', '4', 'V';'H', '2', '6', '4';'h', '2', '6', '4'
            var dir = "recordings";
            Directory.CreateDirectory(dir);
            var video_scan_name = "viseo_"+ DateTime.Now.ToString("hh_mm_ss"); ;
            string name = dir + "\\" + video_scan_name + ".mp4";
            Console.WriteLine("wr" + " " + w + " " + h + " " + fps);
            var video_writer = new VideoWriter(name, -1, fps, new OpenCvSharp.Size(w, h), true);
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
            video_mats = new List<Mat>();
            videoframe_counts = 0;
            videoframe_counts_stop = 10000;
            recording = true ;
            
        }

        private void but_stop_recording_Click(object sender, EventArgs e)
        {
            videoframe_counts_stop = 0;
        }

        private void but_start_video_Click(object sender, EventArgs e)
        {

        }

        
        private void but_set_exposit_Click(object sender, EventArgs e)
        {
            var exp = MainForm.to_double_textbox(textBox_set_exposit, -2, 20);
            if (exp < 0)
            {

                capture.Set(VideoCaptureProperties.AutoExposure, 1);
            }
            else
            {
                capture.Set(VideoCaptureProperties.AutoExposure, 0);
                capture.Set(VideoCaptureProperties.Exposure, -exp);
            }
            
        }

        private void button_set_nozzle_centr_Click(object sender, EventArgs e)
        {

        }


        bool focal_area = false;
        bool boarder_object = false;
        bool centr_object = false;
        private void checkBox_focal_area_CheckedChanged(object sender, EventArgs e)
        {
            focal_area = ((CheckBox)sender).Checked;
        }

        private void checkBox_boarder_object_CheckedChanged(object sender, EventArgs e)
        {
            boarder_object = ((CheckBox)sender).Checked;
        }

        private void checkBox_centr_object_CheckedChanged(object sender, EventArgs e)
        {
            centr_object = ((CheckBox)sender).Checked;
        }
    }
}
