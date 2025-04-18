﻿using Connection;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Application = System.Windows.Forms.Application;
using File = System.IO.File;
using Point = System.Drawing.Point;

namespace SorterSpheroids
{
    public partial class MainForm : Form
    {
        AutoForm auto_form;
        ManualForm manual_form;
        CameraForm camera_form;
        Point[] ps_loc = new Point[2];
        public MainForm()
        {
            InitializeComponent();
            ps_loc = new Point[] { new Point(10,40), new Point(1970, 120), new Point(1970, 120), };//camera, manual, auto
            manual_form = new ManualForm(this);
            auto_form = new AutoForm(this);
            camera_form = new CameraForm(this);

             


           /* var frms = new GFrame[] { new GFrame(1, 2, 3, 5), new GFrame(2, 2, 3, 5), new GFrame(3, 2, 3, 5) };

            save_obj("test1.json", frms);
            var loaded_frms = load_obj<GFrame[]>("test1.json");
            Console.WriteLine("");*/
            /*  var ims = ImageProcessing.load_images_info("test_ph_3");
              var stitcher = new ImageStitcher();
              Mat result = stitcher.StitchImages(ims, 0.5);
              Cv2.ImShow("res",result);*/
            /*  var med = 10;
              var min = 3;

              var med_xy = 14;
              var min_xy = 5;

              OpenCvSharp.Point anchor = new OpenCvSharp.Point(-1, -1);
              var data = new float[,] {
                  {  -med_xy,-min_xy, 0 },
                  { -min_xy, 0f, min_xy },
                  { 0, min_xy,  med_xy } };

              var data_v = new Vec3f[] {
                 new Vec3f(  -med_xy,-min_xy, 0 ),
                  new Vec3f( -min_xy, 0f, min_xy ),
                 new Vec3f( 0, min_xy,  med_xy ) };
             var kernel_xy_ln = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data);
             // var kernel_xy_ln = ImageProcessing.ConvertFloatArrayToMat(data);
              ImageProcessing.print_float((float[,])ImageProcessing.to_float(kernel_xy_ln));
              var test_mat = new Mat("test1.png");
              var gray_xy_ln = new Mat();
              Cv2.CvtColor(test_mat, test_mat,ColorConversionCodes.RGB2GRAY);
              Cv2.Filter2D(test_mat, gray_xy_ln, MatType.CV_8UC1, kernel_xy_ln);
              Cv2.ImShow("test1", 3 * ImageProcessing.sobel_mat(test_mat));
              Cv2.WaitKey();*/
        }
        private void button_maintest_Click(object sender, EventArgs e)
        {
            
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            camera_form.Location = add_p(ps_loc[0], this.Location);
            camera_form.Show(this);

            manual_form.Location = add_p(ps_loc[1],this.Location);
            manual_form.Show(this);

            auto_form.Location = add_p(ps_loc[2], this.Location);
            // auto_form.Show(this);
           // Console.WriteLine("load: " + this.Location.X + " " + this.Location.Y);
            this.Refresh();
            Application.DoEvents();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if(manual_form == null || camera_form == null) return;
            camera_form.Location = add_p(ps_loc[0], this.Location);
            manual_form.Location = add_p(ps_loc[1], this.Location);
            auto_form.Location = add_p(ps_loc[2], this.Location);
           //Console.WriteLine("move: "+this.Location.X + " " + this.Location.Y);
        }

        Point add_p(Point p1,Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        private void but_manual_Click(object sender, EventArgs e)
        {
            manual_form.Visible = false;
            manual_form.Show(this);
            auto_form.Hide();
           
        }

        private void but_auto_Click(object sender, EventArgs e)
        {
            auto_form.Visible = false;
            auto_form.Show(this);
            manual_form.Hide();
           
        }

        public static double to_double_textbox(TextBox textBox, double min, double max)
        {
            var val = to_double(textBox.Text);
            if (val == double.NaN)
            {
                val = min;
            }
            if (val < min)
            {
                val = min;
            }
            if (val > max)
            {
                val = max;
            }
            return val;
        }

        public static double to_double(string val)
        {
            if (val == null) return 0;
            if (val.Length == 0) return 0;
            val = val.Replace(',', '.');
            try
            {
                return Convert.ToDouble(val);
            }
            catch
            {
                return double.NaN;
            }
            //return 
        }
        static public string get_file_name(string init_direct, string extns)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = init_direct;
                //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Filter = extns + " files (" + extns + ")|" + extns;
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            return filePath;
        }
        public void save_photo(string name)
        {
            camera_form.save_photo(name);
        }
        public void scan_thread(GFrame[] frms,double vel_xy, int dt)
        {

            manual_form.scan_thread(frms,  vel_xy,  dt);
        }

        public GFrame get_cur_pos()
        {
            return manual_form.cur_pos;
        }

        static public void save_obj(string path, object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
        }
        static public T load_obj<T>(string path, string text = null)
        {
            string jsontext = "";

            try
            {
                if (text != null)
                {
                    jsontext = text;
                }
                else
                {
                    using (StreamReader file = File.OpenText(path))
                    {
                        jsontext = file.ReadToEnd();
                    }
                    // Console.WriteLine(path + "__________________________");
                    //Console.WriteLine(jsontext);
                }
                return JsonConvert.DeserializeObject<T>(jsontext);
            }
            catch
            {
                return default(T);
            }

        }

       
    }
}
