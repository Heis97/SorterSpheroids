
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
using Connection;

namespace SorterSpheroids
{
    //600 mkm == 1136 pix
    public partial class CameraForm : Form
    {
        VideoCapture capture;
        OpenCvSharp.Size  cameraSize = new OpenCvSharp.Size(1920,1080); //new OpenCvSharp.Size(1280,720);
        public List<Mat> video_mats = new List<Mat>();
        public List<GFrame> video_coords = new List<GFrame>();
        Mat frameMat = new Mat();
        Mat frameMat_buf = new Mat();
        int videoframe_counts = -1;
        int videoframe_counts_stop = 10000;
        int fps = 40;
        bool saved_video = true;
        enum paint_mode { draw_centr,none};
        double cur_contr = 0;
        bool recording = false;
        MainForm mainForm;
        OpenCvSharp.Point point_of_center_1;

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
                frameMat = new Mat();
                if (capture.Read(frameMat))

                // using (var frameMat = capture1.RetrieveMat())
                {
                    if(frameMat==null) return;  
                    if(frameMat.Empty()) return;
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
            frameMat = (Mat)e.UserState;
           
            Cv2.Flip(frameMat, frameMat, FlipMode.Y);
            frameMat_buf = frameMat.Clone();
            //Console.WriteLine(frameMat.Width+ " " + frameMat.Height + " " + fps);
            frameMat = imProcess(frameMat);
            if (recording) videoframe_counts++;
            pictureBox1.Image?.Dispose();
            try
            {
                Cv2.Resize(frameMat, frameMat, new OpenCvSharp.Size(1280, 720));
                var bitmap = BitmapConverter.ToBitmap(frameMat);
                //Console.WriteLine(bitmap.Width + " " + bitmap.Height + " " + fps);
                pictureBox1.Image = bitmap;
            }
            catch { }
            
        }

        private void videoStart(int number)
        {
            capture = new VideoCapture(); // Первая камера
            capture.Open(number, VideoCaptureAPIs.DSHOW);

            if (!capture.IsOpened())
            {
                MessageBox.Show("Не удалось открыть камеру "+number);
                return;
            }
            capture.Set(VideoCaptureProperties.FrameWidth, cameraSize.Width);//1920
            capture.Set(VideoCaptureProperties.FrameHeight, cameraSize.Height);//1080

            capture.Set(VideoCaptureProperties.Fps, this.fps);
            capture.Set(VideoCaptureProperties.FourCC, OpenCvSharp.VideoWriter.FourCC("MJPG"));

            var _fps = capture.Get(VideoCaptureProperties.Fps);
            var w = capture.Get(VideoCaptureProperties.FrameWidth);
            var h = capture.Get(VideoCaptureProperties.FrameHeight);



            ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);
            backgroundWorker1.RunWorkerAsync();
            Console.WriteLine(w + " " + h + " " + _fps);

        }
        private int videoStart_rec(string number)
        {
            capture = new VideoCapture(); // Первая камера
            capture.Open(number, VideoCaptureAPIs.ANY);
            backgroundWorker1.RunWorkerAsync();
            if (!capture.IsOpened())
            {
                MessageBox.Show("Не удалось открыть видео " + number);
                return -1;
            }
            return (int) capture.Get(VideoCaptureProperties.FrameCount);
        }
        Mat imProcess(Mat mat)
        {
            
            if (mat == null) return null;
            if(mat.Empty()) return null;
            if (video_mats != null)
            {
               // Console.WriteLine(videoframe_counts + "/ "+videoframe_counts_stop);


                if (videoframe_counts > 0 && videoframe_counts < videoframe_counts_stop)
                {

                    video_mats.Add(mat.Clone());
                    video_coords.Add(mainForm.get_cur_pos());
                }
                else if(videoframe_counts >= videoframe_counts_stop)
                {
                    save_video(cameraSize.Width, cameraSize.Height);
                }

                if (focal_area)
                {
                   //(mat, cur_contr) = ImageProcessing.get_focal_surface_for_conf(mat,bin);
                    //Console.WriteLine(cur_contr.ToString());

                    mat = ImageProcessing.get_mean( ImageProcessing.sobel_mat(mat),bin);
                  //  Console.WriteLine("sobel_done");
                  //  Cv2.ImShow("test", mat);
                   // Cv2.WaitKey();
                }
                if (centr_object)
                {
                    Cv2.DrawMarker(mat,point_of_center_1,new Scalar(255,255,0),MarkerTypes.TiltedCross,40,4);
                }
                if (boarder_object)
                {
                    mat = ImageProcessing.get_board_obj(mat, bin);
                }
            }
            return mat;

        }
        void analyse()
        {
            var coords = video_coords.ToArray();
            var mats = video_mats.ToArray();
            var common_image = new ImageCoordinatsConverter(coords, 1320, 1080);//1580
            var common_image_or = new ImageCoordinatsConverter(coords, 1320, 1080);
            // var mats = ImageProcessing.load_images_video(camera_form.video_coords.ToArray(), camera_form.video_mats.ToArray());
            /* var mat_f = mats.First().Value;
             var min_mat = (from f in mats
                            orderby f.Value.Mean().Val0 + f.Value.Mean().Val1 + f.Value.Mean().Val2
                            select f).ToArray()[0];
             */
            var ind = 0;
            var max_ind = 180;
            var len = Math.Min(max_ind, mats.Length - 1);
            bool debug = false;
            //debug = true;
            foreach (var mat in mats)
            {

                ind++;
                if (ind > 1 && ind< len)
                {
                    if (coords[ind].y - coords[ind - 1].y > 0)
                    {
                        //common_image_or.add_image(mat, coords[ind]);
                        common_image_or.add_image_simple(mat, coords[ind]);

                        //common_image.add_image_allign(mat, coords[ind],new OpenCvSharp.Point(1,1),debug);
                        // debug = true;
                    }
                   
                    //Cv2.ImShow("mat", 5 * (mat.Value.Clone() - min_mat.Value));
                    //Cv2.WaitKey();
                    Console.WriteLine(ind + "/"+(len));
                }
            }
            Cv2.ImShow("common_", common_image_or.mat_common);
            //Cv2.ImShow("common_allign", common_image.mat_common);
        }
        void save_video(int w, int h)
        {
            recording = false;

            analyse();

            if (!saved_video)
            {
                comboBox_images.Items.AddRange(video_mats.ToArray());
                return;
            }
            
            
            int fcc = VideoWriter.FourCC('h', '2', '6', '4'); //'M', 'J', 'P', 'G';'m', 'p', '4', 'v';'M', 'P', '4', 'V';'H', '2', '6', '4';'h', '2', '6', '4'
            var dir = "recordings";
            Directory.CreateDirectory(dir);
            var video_scan_name = "video_"+ DateTime.Now.ToString("hh_mm_ss_d");
            var name_without_ext = dir + "\\" + video_scan_name;
            string name = name_without_ext + ".mp4";
            Console.WriteLine("wr" + " " + w + " " + h + " " + fps);
            var video_writer = new VideoWriter(name, -1, fps, new OpenCvSharp.Size(w, h), true);
            //var reswr = video_writer[ind].Set(VideoWriter.WriterProperty.Quality, 100);
            //Console.WriteLine(reswr);
            for (int i = 0; i < video_mats.Count ; i++)
            {
                video_writer.Write(video_mats[i]);
                //var p = Detection.detectLineSensor(video_mats[ind - 1][i])[0];
                //Console.WriteLine(ind + " "  + p);
            }

            MainForm.save_obj(name_without_ext+".json", video_coords.ToArray());
            video_coords = new List<GFrame>();
            video_mats = new List<Mat>();
            video_writer.Dispose();
            
            videoframe_counts = -1;
            videoframe_counts_stop = 10000;
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
            videoframe_counts = 10000;
            videoframe_counts_stop = -1;
        }

        private void but_start_video_Click(object sender, EventArgs e)
        {
            video_mats = new List<Mat>();
            recording = true;
            saved_video = false;
            videoframe_counts = 0;
            var k_dec = 1.0;
            videoframe_counts_stop =(int)( (videoStart_rec(textBox_video_name.Text)-1) / k_dec);
            var coords_name = Path.ChangeExtension(textBox_video_name.Text, "json");
            video_coords = MainForm.load_obj<GFrame[]>(coords_name).ToList();
            Console.WriteLine(" videoframe_counts_stop: "+videoframe_counts_stop);
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

        public void save_photo(string name)
        {
            var folder=  Path.GetDirectoryName(name);
            Directory.CreateDirectory(folder);
            frameMat_buf.SaveImage(name+".png");
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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var control = (Control)sender;
            if (e.Button == MouseButtons.Right)
            {
                point_of_center_1 = new OpenCvSharp.Point(e.Location.X , e.Location.Y );
            }
        }

        private void textBox_video_name_DoubleClick(object sender, EventArgs e)
        {
            var video_name = MainForm.get_file_name(Directory.GetCurrentDirectory(), "*.mp4");
            textBox_video_name.Text = video_name;

        }

        private void comboBox_images_SelectedIndexChanged(object sender, EventArgs e)
        {
           var mat = (Mat)comboBox_images.Items[comboBox_images.SelectedIndex];
            try
            {
                var bitmap = BitmapConverter.ToBitmap(imProcess(mat.Clone()));
                pictureBox1.Image = bitmap;
            }
            catch (Exception ex)
            {
            }
            
        }
        int bin = 10;
        private void textBox_focus_binary_KeyDown(object sender, KeyEventArgs e)
        {
            bin = (int)MainForm.to_double_textbox(textBox_focus_binary, 0, 255);
        }
    }
}
