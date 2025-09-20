
using Connection;
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Point = OpenCvSharp.Point;

namespace SorterSpheroids
{
    public static class ImageProcessing
    {
        public static Mat get_focal_surface(Mat mat, int bin)
        {
            var lapl = new Mat();            
           var im = new Mat();
            Cv2.CvtColor(mat, im, ColorConversionCodes.RGB2GRAY);
            Cv2.GaussianBlur(im, im, new OpenCvSharp.Size(61, 61), -1);
            Cv2.Laplacian(im, lapl,MatType.CV_8UC1);
            var im_th = new Mat();
            Cv2.Threshold(lapl, im_th, bin, 255, ThresholdTypes.Binary);
            
            Mat kernel7 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 7), new OpenCvSharp.Point(3, 3));
            Mat kernel5 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5), new OpenCvSharp.Point(2, 2));
            Mat kernel3 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3), new OpenCvSharp.Point(1, 1));
            Mat ellips7 = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(7, 7), new OpenCvSharp.Point(1, 1));
            int num = 4;
            Mat im_med = im_th;
            for (int i = 0; i < num; i++)
            {

                im_med = im_med.MorphologyEx(MorphTypes.Dilate, ellips7, new OpenCvSharp.Point(-1, -1), 3, BorderTypes.Default, new Scalar());
                im_med = im_med.MorphologyEx(MorphTypes.Close, ellips7, new OpenCvSharp.Point(-1, -1), 10, BorderTypes.Default, new Scalar());
            }
            //var mat_r = new Image<>
            var ret = new Mat();
            var spl = mat.Split();
            spl[1] += im_med * 0.3;
            Cv2.Merge(spl, ret);
            ret = im * bin;
            return ret;
        }

        public static (Mat,double) get_focal_surface_for_conf_color(Mat mat)
        {
            var lapl = new Mat();

            Cv2.Laplacian(mat, lapl, MatType.CV_8UC3);
            var mean = lapl.Mean();
            Cv2.Threshold(lapl, lapl, 15, 255,ThresholdTypes.Binary);
            var conf = mat.Clone();
            //mat.ConvertTo(mat, MatType.CV_8U);
            //BitwiseAnd(mat, mat, conf, lapl);

            return (lapl, mean.Val0);
        }
        public static (Mat, double) get_focal_surface_for_conf(Mat mat,int bin = 10)
        {
            var lapl = new Mat();
            Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
            Cv2.Laplacian(mat, lapl, MatType.CV_8UC1,3);
            var mean = lapl.Mean();
            Cv2.Threshold(lapl, lapl, bin, 255, ThresholdTypes.Binary);
            var conf = mat.Clone();
            //mat.ConvertTo(mat, MatType.CV_8U);
            //BitwiseAnd(mat, mat, conf, lapl);

            Cv2.PutText(lapl, Math.Round(mean.Val0, 3).ToString(), new OpenCvSharp.Point(10, 200), HersheyFonts.HersheyPlain, 10, new Scalar(255), 3);


            return (lapl, mean.Val0);
        }

        public static Mat get_board_obj(Mat mat, int bin = 10)
        {
            var thr = new Mat();
            Cv2.CvtColor(mat, thr, ColorConversionCodes.RGB2GRAY);
            Cv2.GaussianBlur(thr, thr, new OpenCvSharp.Size(7, 7), -1);
            Cv2.Threshold(thr, thr, bin, 255, ThresholdTypes.Binary);
            var contours = new OpenCvSharp.Point[0][];
            var  hier = new HierarchyIndex[0];
            Cv2.FindContours(thr, out contours,out hier, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            Cv2.DrawContours(mat, contours, -1, new Scalar(255, 0, 0), 1, LineTypes.Link8);
            return mat;
        }
        /* public static (Mat, double) get_focal_surface_from_mats(Mat[] mat)
         {


             return (lapl, mean.Val0);
         }*/
        public static Mat ConvertFloatArrayToMat(float[,] array)
        {
            // Get dimensions of the 2D array
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            // Create a new Mat with the same dimensions, single channel, 32-bit float type
            Mat mat = new Mat(rows, cols, MatType.CV_32F);

            // Copy data from float array to Mat
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    mat.Set(i, j, array[i, j]);
                    Console.WriteLine(array[i, j]);
                }
                Console.WriteLine("______");
            }

            return mat;
        }
        static public Mat sobel_mat(Mat mat, int gauss_size = 7,double k = 0.1, bool simple = false)
        {
            simple = false;
            if(mat.Channels()==3)
            {
                Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
            }
           
            Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(gauss_size, gauss_size), -1);
            if (simple)
            {
                var gray_x0 = new Mat();
                var gray_y0 = new Mat();
                Cv2.Sobel(mat, gray_x0, MatType.CV_32FC1, 1, 0, 3);
                Cv2.Sobel(mat, gray_y0, MatType.CV_32FC1, 0, 1, 3);
                Cv2.ConvertScaleAbs(gray_x0, gray_x0, 1, 0);
                Cv2.ConvertScaleAbs(gray_y0, gray_y0, 1, 0);
                return gray_x0 + gray_y0;
            }
            var gray_x = new Mat();
            var gray_y = new Mat();
            var gray_xn = new Mat();
            var gray_yn = new Mat();

            var gray_xy_r = new Mat();
            var gray_xy_rn = new Mat();
            var gray_xy_l = new Mat();
            var gray_xy_ln = new Mat();

            var med = 10;
            var min = 3;

            var med_xy = 14;
            var min_xy = 5;

            Point anchor = new Point(-1, -1);
            
           


            var data_x = new float[3, 3] {
                { -min, 0f, min },
                { - med, 0f, med },
                { -min, 0f, min } };
            var data_y = new float[3, 3] {
                { min, med, min },
                { 0f, 0f, 0f },
                { -min, -med, -min} };

            var data_xn = new float[3, 3] {
                { min, 0f, -min },
                { med, 0f, -med },
                { min, 0f, -min } };

            var data_yn = new float[3, 3] {
                { -min, -med, -min },
                { 0f, 0f, 0f },
                { min, med, min} };

            var data_xy_r = new float[3, 3] {
                { 0, min_xy, med_xy },
                { -min_xy, 0f, min_xy },
                { -med_xy, -min_xy, 0} };

            var data_xy_rn = new float[3, 3] {
                { 0, -min_xy,-med_xy },
                { min_xy, 0f, -min_xy },
                { med_xy, min_xy, 0} };

            var data_xy_l = new float[3, 3] {
                {  med_xy, min_xy, 0 },
                { min_xy, 0f, -min_xy },
                { 0, -min_xy,  -med_xy } };
            var data_xy_ln = new float[,] {
                {  -med_xy,-min_xy, 0 },
                { -min_xy, 0f, min_xy },
                { 0, min_xy,  med_xy } };

            var kernel_x = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_x);
            var kernel_y = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_y);
            var kernel_yn = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_yn);
            var kernel_xn = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_xn);

            var kernel_xy_r = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_xy_r);
            var kernel_xy_rn = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_xy_rn);
            var kernel_xy_l = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_xy_l);
            var kernel_xy_ln = Mat.FromPixelData(3, 3, MatType.CV_32FC1, data_xy_ln);
            

            Cv2.Filter2D(mat, gray_yn, MatType.CV_8UC1, kernel_yn);
            Cv2.Filter2D(mat, gray_xn, MatType.CV_8UC1, kernel_xn);
            Cv2.Filter2D(mat, gray_x, MatType.CV_8UC1, kernel_x);
            Cv2.Filter2D(mat, gray_y, MatType.CV_8UC1, kernel_y);

            Cv2.Filter2D(mat, gray_xy_r, MatType.CV_8UC1, kernel_xy_r);
            Cv2.Filter2D(mat, gray_xy_rn, MatType.CV_8UC1, kernel_xy_rn);
            Cv2.Filter2D(mat, gray_xy_l, MatType.CV_8UC1, kernel_xy_l);
            Cv2.Filter2D(mat, gray_xy_ln, MatType.CV_8UC1, kernel_xy_ln);

            //  CvInvoke.ConvertScaleAbs(gray_x, gray_x, 1, 0);
            //CvInvoke.ConvertScaleAbs(gray_y, gray_y, 1, 0);

            return k * gray_x + k * gray_y + k * gray_xn + k * gray_yn
                + k * gray_xy_r + k * gray_xy_rn + k * gray_xy_l + k * gray_xy_ln;

        }

        public static Mat get_mean(Mat mat,int bin)
        {
           // var mean = mat.Mean();
            Cv2.Threshold(mat, mat, bin, 255, ThresholdTypes.Binary);
            // var conf = mat.Clone();
            //mat.ConvertTo(mat, MatType.CV_8U);
            //BitwiseAnd(mat, mat, conf, lapl);
           var  mean = mat.Mean();
            Cv2.PutText(mat, Math.Round(mean.Val0, 3).ToString(), new OpenCvSharp.Point(100, 200), HersheyFonts.HersheyPlain, 5, new Scalar(255), 3);
            return mat;
        }

        public static void print_double(double[,] mat)
        {
            Console.WriteLine(mat.GetLength(0) + " " + mat.GetLength(1));
            for (var rowIndex = 0; rowIndex < mat.GetLength(0); rowIndex++)
            {
                for (var colIndex = 0; colIndex < mat.GetLength(1); colIndex++)
                {
                    Console.Write(Math.Round(mat[rowIndex, colIndex], 4) + " ");
                }
                Console.WriteLine("");
            }
        }

        public static void print_double(double[] mat)
        {
            Console.WriteLine(mat.GetLength(0));
            for (var rowIndex = 0; rowIndex < mat.GetLength(0); rowIndex++)
            {
                Console.Write(Math.Round(mat[rowIndex], 4) + " ");
            }
            Console.WriteLine("");
        }
        public static void print_float(float[,] mat)
        {
            Console.WriteLine(mat.GetLength(0) + " " + mat.GetLength(1));
            for (var rowIndex = 0; rowIndex < mat.GetLength(0); rowIndex++)
            {
                for (var colIndex = 0; colIndex < mat.GetLength(1); colIndex++)
                {
                    Console.Write(Math.Round(mat[rowIndex, colIndex], 4) + " ");
                }
                Console.WriteLine("");
            }
        }
        public static object to_double(Mat mat)
        {

            if (mat.Rows == 1)
            {
                Console.WriteLine(mat.Rows + " " + mat.Cols);
                var data = new double[mat.Cols];
                for (var colIndex = 0; colIndex < mat.Cols; colIndex++)
                {
                    data[colIndex] = mat.At<double>(0, colIndex);
                }
                return (object)data;
            }
            else
            {
                Console.WriteLine(mat.Rows + " " + mat.Cols);
                var data = new double[mat.Rows, mat.Cols];

                for (var rowIndex = 0; rowIndex < mat.Rows; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < mat.Cols; colIndex++)
                    {
                        data[rowIndex, colIndex] = mat.At<double>(0, colIndex);
                    }
                }
                return (object)data;
            }


        }
        public static object to_float(Mat mat)
        {

            if (mat.Rows == 1)
            {
                Console.WriteLine(mat.Rows + " " + mat.Cols);
                var data = new float[mat.Cols];
                for (var colIndex = 0; colIndex < mat.Cols; colIndex++)
                {
                    data[colIndex] = mat.At<float>(0, colIndex);
                }
                return (object)data;
            }
            else
            {
                Console.WriteLine(mat.Rows + " " + mat.Cols);
                var data = new float[mat.Rows, mat.Cols];

                for (var rowIndex = 0; rowIndex < mat.Rows; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < mat.Cols; colIndex++)
                    {
                        data[rowIndex, colIndex] = mat.At<float>(rowIndex, colIndex);
                    }
                }
                return (object)data;
            }


        }

        static public Dictionary<GFrame,Mat> load_images(string path)
        {
            var names = Directory.GetFiles(path);
            var mats = new Dictionary<GFrame,Mat>();
            foreach (var name in names)
            {
                mats.Add(new GFrame(Path.GetFileNameWithoutExtension(name)), new Mat(name));
            }
            return mats;
        }

        static public Dictionary<GFrame, Mat> load_images_video(GFrame[] frms, Mat[] images)
        {
            var mats = new Dictionary<GFrame, Mat>();
            for (int i =0; i<frms.Length;i++)
            {
                //if()
                mats.Add(frms[i], images[i]);
            }
            return mats;
        }
        public static OpenCvSharp.Point2f findCentrCont(Point[] contour)
        {
            var M = Cv2.Moments(contour);
            var cX = M.M10 / M.M00;
            var cY = M.M01 / M.M00;
            var p =new OpenCvSharp.Point2f((float)cX, (float)cY);
            return p;
        }

        static public ImageInfo[] load_images_info(string path,double k_decrease = 0.1, double pixel_mm_ratio = 0.000528169)
        {
            var names = Directory.GetFiles(path);
            var mats = new List<ImageInfo>();

            var mm_pixel_ratio = (1/pixel_mm_ratio)* k_decrease;
            foreach (var name in names)
            {
                var coord = new GFrame(Path.GetFileNameWithoutExtension(name));
                var coord_pix = coord * mm_pixel_ratio;
                var mat_orig = new Mat(name);
                Cv2.Resize(mat_orig,mat_orig,new OpenCvSharp.Size(mat_orig.Width* k_decrease, mat_orig.Height * k_decrease));
                var im_info = new ImageInfo()
                {
                    EstimatedError = new Point(30,30),
                    EstimatedPosition = new Point(coord_pix.x, coord_pix.y),
                    Image = mat_orig
                };
                mats.Add(im_info);
            }
            return mats.ToArray();
        }
    }

    public class ImageCoordinatsConverter
    {
        public Mat mat_common;
        public Mat mask;
        public Mat mask_inv;
        public int w_pix, h_pix,bin;
        public double x_mm, y_mm, w_mm, h_mm, pixel_mm_ratio_default, pixel_mm_ratio_common, mm_pixel_ratio_image,k_decr,obj_mm,obj_form;
        public List<GFrame> points_mats = new List<GFrame>();

        public List<Point> points1 = new List<Point>();
        public ImageCoordinatsConverter(double x_mm, double y_mm, double  w_mm, double h_mm, int  w_pix,int h_pix,double pixel_mm_ratio = 0.000528169) 
        {            
            this.w_pix = w_pix;
            this.h_pix = h_pix;
            this.x_mm = x_mm;
            this.y_mm = y_mm;
            this.w_mm = w_mm;
            this.h_mm = h_mm;
            this.pixel_mm_ratio_default = pixel_mm_ratio;


            if(w_pix / w_mm > h_pix / h_mm)
            {
                this.mm_pixel_ratio_image = w_pix / w_mm;
            }
            else
            {
                this.mm_pixel_ratio_image = h_pix / h_mm;
            }
            k_decr = 0.9;
            bin = 50;
            pixel_mm_ratio_common =  w_mm/w_pix;
            var size_common = new OpenCvSharp.Size((int)(mm_pixel_ratio_image * w_mm), (int)(mm_pixel_ratio_image * h_mm));
            mat_common = new Mat(size_common, MatType.CV_8UC3);

            mask = new Mat(size_common, MatType.CV_8UC3);
            mask_inv = new Mat(size_common, MatType.CV_8UC3);
            mask.SetTo(new Scalar(0, 0, 0));
            mask_inv.SetTo(new Scalar(255,255,255));
        }
        // 0.000528169
        public ImageCoordinatsConverter(GFrame[] gFrames, int w_pix, int h_pix, double pixel_mm_ratio = 0.000528169)
        {

            var max_fr = GFrame.Max(gFrames);
            var min_fr = GFrame.Min(gFrames);

            var board_mm = 2;
            this.x_mm = min_fr.x - board_mm/2;
            this.y_mm = min_fr.y - board_mm / 2;

            this.w_mm = max_fr.x - min_fr.x + board_mm;
            this.h_mm = max_fr.y - min_fr.y + board_mm;

            this.w_pix = w_pix;
            this.h_pix = h_pix;
            this.pixel_mm_ratio_default = pixel_mm_ratio;

            if (w_pix / w_mm > h_pix / h_mm)
            {
                this.mm_pixel_ratio_image = w_pix / w_mm;
            }
            else
            {
                this.mm_pixel_ratio_image = h_pix / h_mm;
            }
            k_decr = 0.9;
            bin = 50;
            pixel_mm_ratio_common = w_mm / w_pix;
            var size_common = new OpenCvSharp.Size((int)(mm_pixel_ratio_image * w_mm), (int)(mm_pixel_ratio_image * h_mm));
            mat_common = new Mat(size_common, MatType.CV_8UC3);

            mask = new Mat(size_common, MatType.CV_8UC3);
            mask_inv = new Mat(size_common, MatType.CV_8UC3);
            mask.SetTo(new Scalar(0, 0, 0));
            mask_inv.SetTo(new Scalar(255, 255, 255));
        }
        public void add_image_simple(Mat mat, GFrame frame)
        {
            var frame_mm_w = mat.Width * pixel_mm_ratio_default;
            var frame_mm_h = mat.Height * pixel_mm_ratio_default;

            mat = mat.Resize(new OpenCvSharp.Size(frame_mm_w * mm_pixel_ratio_image , frame_mm_h * mm_pixel_ratio_image ));
            //Console.WriteLine(mat.Width + " " + mat.Height);
            var x = (frame.x - x_mm) * mm_pixel_ratio_image;
            var y = (frame.y - y_mm) * mm_pixel_ratio_image;
           
            var rect_for_ins = new Rect(new Point(x,y),mat.Size());
            //Cv2.Rectangle(mat,new Rect(new Point(0,0),new OpenCvSharp.Size(mat.Width,mat.Height)),new Scalar(0,55,0),1);

            add_image_blend(mat_common, mat, rect_for_ins);
            //Cv2.ImShow("tets1", mat_common);
            //Cv2.WaitKey();
        }

        public void add_image(Mat mat, GFrame frame)
        {
            var frame_mm_w = mat.Width * pixel_mm_ratio_default;
            var frame_mm_h = mat.Height * pixel_mm_ratio_default;

            mat = mat.Resize(new OpenCvSharp.Size(frame_mm_w * mm_pixel_ratio_image, frame_mm_h * mm_pixel_ratio_image));
            //Console.WriteLine(mat.Width + " " + mat.Height);
            var x = (frame.x - x_mm) * mm_pixel_ratio_image;
            var y = (frame.y - y_mm) * mm_pixel_ratio_image;

            var rect_for_ins = new Rect(new Point(x, y), mat.Size());
            //Cv2.Rectangle(mat,new Rect(new Point(0,0),new OpenCvSharp.Size(mat.Width,mat.Height)),new Scalar(0,55,0),1);

            var white_mat = new Mat(mat.Size(), MatType.CV_8UC3);
            var black_mat  = new Mat(mat.Size(), MatType.CV_8UC3);
            black_mat.SetTo(new Scalar(0, 0, 0));
            white_mat.SetTo(new Scalar(255, 255, 255));

            

            var black_or = new Mat(mat_common.Size(), MatType.CV_8UC3);
            black_or.SetTo(new Scalar(0, 0, 0));
            var white_or = new Mat(mat_common.Size(), MatType.CV_8UC3);
            white_or.SetTo(new Scalar(255, 255, 255));

            white_mat.CopyTo(new Mat(black_or, rect_for_ins));
            black_mat.CopyTo(new Mat(white_or, rect_for_ins));

            var black_mat_orig = new Mat(mat_common.Size(), MatType.CV_8UC3);
            black_mat_orig.SetTo(new Scalar(0, 0, 0));
            mat.CopyTo(new Mat(black_mat_orig, rect_for_ins));

            var s1 = (black_mat_orig - mask).ToMat();
            var s2_mat = (black_mat_orig - mask_inv).ToMat();
            var s2_or = (mat_common - white_or).ToMat();

            Cv2.ImShow("mat_common", mat_common);
            var s2 = 0.5 * s2_mat.Clone() + 0.5 * s2_or.Clone();
            mat_common -= black_or;
            mat_common += (s1 + s2);

            /*  Cv2.ImShow("black_or", black_or);
              Cv2.ImShow(" white_or", white_or);
              Cv2.ImShow(" black_mat_orig", black_mat_orig);

              Cv2.ImShow("  mask", mask);
              Cv2.ImShow(" mask_inv", mask_inv);
            Cv2.ImShow("s1", s1);
            Cv2.ImShow("s2_mat", s2_mat);
            Cv2.ImShow("s2_or", s2_or);
            */
            /* Cv2.ImShow("  mask", mask);
             Cv2.ImShow(" mask_inv", mask_inv);
             Cv2.ImShow("tets1", mat_common);*/
            // Cv2.ImShow("tets1", mat_common);
            //   Cv2.WaitKey();
            white_mat.CopyTo(new Mat(mask, rect_for_ins));
            black_mat.CopyTo(new Mat(mask_inv, rect_for_ins));
        }
        public void add_image_allign(Mat mat, GFrame frame, Point err, bool debug = false)
        {
            var frame_mm_w = mat.Width * pixel_mm_ratio_default;
            var frame_mm_h = mat.Height * pixel_mm_ratio_default;

            mat = mat.Resize(new OpenCvSharp.Size(frame_mm_w * mm_pixel_ratio_image, frame_mm_h * mm_pixel_ratio_image));
            //Console.WriteLine(mat.Width + " " + mat.Height);s
            var x = (frame.x - x_mm) * mm_pixel_ratio_image;
            var y = (frame.y - y_mm) * mm_pixel_ratio_image;

            var rect_for_ins = new Rect(new Point(x, y), mat.Size());
            //Cv2.Rectangle(mat,new Rect(new Point(0,0),new OpenCvSharp.Size(mat.Width,mat.Height)),new Scalar(0,55,0),1);
            var mat_black = new Mat(mat_common.Size(), MatType.CV_8UC3);
            var roi_for_allign = new Rect(new Point(x - err.X, y - err.Y), new OpenCvSharp.Size(mat.Width + 2 * err.X, mat.Height + 2 * err.Y));
            var area_for_allign = new Mat(mat_common, roi_for_allign);

            var p_estim = estimated_allign(area_for_allign, mat, debug);

            var rect_for_ins_allign = new Rect(new Point(x - err.X + p_estim.X, y - err.Y + p_estim.Y), mat.Size());
            rect_for_ins = rect_for_ins_allign;
            var white_mat = new Mat(mat.Size(), MatType.CV_8UC3);
            var black_mat = new Mat(mat.Size(), MatType.CV_8UC3);
            black_mat.SetTo(new Scalar(0, 0, 0));
            white_mat.SetTo(new Scalar(255, 255, 255));



            var black_or = new Mat(mat_common.Size(), MatType.CV_8UC3);
            black_or.SetTo(new Scalar(0, 0, 0));
            var white_or = new Mat(mat_common.Size(), MatType.CV_8UC3);
            white_or.SetTo(new Scalar(255, 255, 255));

            white_mat.CopyTo(new Mat(black_or, rect_for_ins));
            black_mat.CopyTo(new Mat(white_or, rect_for_ins));

            var black_mat_orig = new Mat(mat_common.Size(), MatType.CV_8UC3);
            black_mat_orig.SetTo(new Scalar(0, 0, 0));
            mat.CopyTo(new Mat(black_mat_orig, rect_for_ins));

            var s1 = (black_mat_orig - mask).ToMat();
            var s2_mat = (black_mat_orig - mask_inv).ToMat();
            var s2_or = (mat_common - white_or).ToMat();

          //  Cv2.ImShow("mat_common", mat_common);
            var s2 = 0.5 * s2_mat.Clone() + 0.5 * s2_or.Clone();
            mat_common -= black_or;
            mat_common += (s1 + s2);

            /*  Cv2.ImShow("black_or", black_or);
              Cv2.ImShow(" white_or", white_or);
              Cv2.ImShow(" black_mat_orig", black_mat_orig);

              Cv2.ImShow("  mask", mask);
              Cv2.ImShow(" mask_inv", mask_inv);
            Cv2.ImShow("s1", s1);
            Cv2.ImShow("s2_mat", s2_mat);
            Cv2.ImShow("s2_or", s2_or);
            */
            /* Cv2.ImShow("  mask", mask);
             Cv2.ImShow(" mask_inv", mask_inv);
             Cv2.ImShow("tets1", mat_common);*/
           //  Cv2.ImShow("mat_common", mat_common);
          //     Cv2.WaitKey();
            white_mat.CopyTo(new Mat(mask, rect_for_ins));
            black_mat.CopyTo(new Mat(mask_inv, rect_for_ins));
            points_mats.Add(frame);
        }
        public void add_image_allign_simple(Mat mat, GFrame frame, Point err)
        {
            var frame_mm_w = mat.Width * pixel_mm_ratio_default;
            var frame_mm_h = mat.Height * pixel_mm_ratio_default;

            mat = mat.Resize(new OpenCvSharp.Size(frame_mm_w * mm_pixel_ratio_image, frame_mm_h * mm_pixel_ratio_image));
            //Console.WriteLine(mat.Width + " " + mat.Height);
            var x = (frame.x - x_mm) * mm_pixel_ratio_image;
            var y = (frame.y - y_mm) * mm_pixel_ratio_image;

            var rect_for_ins = new Rect(new Point(x, y), mat.Size());
           
            var mat_black = new Mat(mat_common.Size(), MatType.CV_8UC3);
            var roi_for_allign = new Rect(new Point(x - err.X, y - err.Y), new OpenCvSharp.Size(mat.Width + 2 * err.X, mat.Height + 2 * err.Y));
            var area_for_allign = new Mat(mat_common, roi_for_allign);
            
            var p_estim = estimated_allign(area_for_allign, mat);

            var rect_for_ins_allign = new Rect(new Point(x - err.X + p_estim.X, y - err.Y + p_estim.Y), mat.Size());
            Console.WriteLine(p_estim.X + " " + p_estim.Y);
            //Cv2.Rectangle(mat, new Rect(new Point(0, 0), new OpenCvSharp.Size(mat.Width, mat.Height)), new Scalar(0, 55, 0), 1);
            //mat.CopyTo(new Mat(mat_black, rect_for_ins_allign));
            //mat_common += k_decr * mat_black;

            add_image_blend(mat_common, mat, rect_for_ins_allign);
            //mat.CopyTo(new Mat(mat_common, rect_for_ins));
            //Cv2.ImShow("tets1", mat_common);
            //Cv2.WaitKey();
        }
        public static void add_image_blend(Mat mat_base, Mat mat_added, Rect roi_ins)
        {
            var mat_ins_area = (new Mat(mat_base, roi_ins)*0.5+mat_added*0.5).ToMat();
            mat_ins_area.CopyTo(new Mat(mat_base, roi_ins));
        }
        Vec3f[] get_centres_objects(Point[] points)
        {
            
            return null;
        }
        public Point estimated_allign_old_not_work(Mat area_for_allign, Mat mat_allign,bool debug = false)
        {
            var area_for_allign_gray = new Mat();
            var mat_allign_gray = new Mat();

            Cv2.CvtColor(area_for_allign, area_for_allign_gray, ColorConversionCodes.RGB2GRAY);
            Cv2.CvtColor(mat_allign, mat_allign_gray, ColorConversionCodes.RGB2GRAY);
            var coord_allign = new Point(0, 0);
            var wind_x = area_for_allign.Width - mat_allign.Width;
            var wind_y = area_for_allign.Height - mat_allign.Height;
            double val_min = double.MaxValue;
            bool finded = false;

            var val_al = mat_allign.Mean().Val0;
            if(val_al < 0.5) return new Point(wind_x / 2, wind_y / 2);

            var data_diff = new byte[wind_x, wind_y];

            for (int x = 0; x < wind_x; x++)
            {
                for (int y = 0; y < wind_y; y++)
                {
                    var roi_for_allign = new Rect(new Point(x, y), new OpenCvSharp.Size(mat_allign.Width, mat_allign.Height));
                    var orig_place_gray = new Mat(area_for_allign_gray, roi_for_allign);
                    /* if (x == 15 && y == 15)
                     {
                         Cv2.ImShow("orig_place", orig_place);
                         Cv2.ImShow("area_for_allign", mat_allign);
                         Cv2.WaitKey();
                     }*/

                    var diff1 = (orig_place_gray - mat_allign_gray).ToMat();
                    var diff2 = (mat_allign_gray - orig_place_gray).ToMat();
                    var val1 = diff1.Mean().Val0;
                    var val2 = diff2.Mean().Val0;
                    var val = val1;
                    data_diff[y, x] = (byte)(val * 150);
                    if(debug)
                    {
                        Console.WriteLine(x + " " + y + " " + val);
                        Cv2.PutText(diff1, Math.Round(val, 3).ToString(), new OpenCvSharp.Point(10, 20), HersheyFonts.HersheyPlain, 3 ,new Scalar(255), 1);
                        Cv2.ImShow("diff1", diff1);

                       // Cv2.ImShow("diff2", diff2);
                        Cv2.WaitKey();
                    }
                   
                    
                    if (val < val_min)
                    {
                        val_min = val;
                        coord_allign = new Point(x, y);
                        finded = true;
                        //Console.WriteLine(x+" " + y+" "+val_min);
                    }
                }
            }
            Console.WriteLine(coord_allign.X + " " + coord_allign.Y + " " + val_min);
            var roi_for_allign_end = new Rect(coord_allign, new OpenCvSharp.Size(mat_allign.Width, mat_allign.Height));
            var orig_place_end = new Mat(area_for_allign, roi_for_allign_end);
            var kernel = Mat.FromPixelData(wind_x, wind_y, MatType.CV_8UC1, data_diff);
            //Cv2.ConvertScaleAbs(kernel, kernel);
            //Cv2.Normalize(kernel, kernel);
            //if(debug)
            {
                var k_res = 10;
                Cv2.CvtColor(kernel, kernel, ColorConversionCodes.GRAY2RGB);

                // Cv2.DrawMarker(kernel, coord_allign, new Scalar(255), MarkerTypes.TiltedCross, 5, 1);
                Cv2.Resize(kernel, kernel, new OpenCvSharp.Size(kernel.Width*k_res, kernel.Height * k_res));
                Cv2.DrawMarker(kernel, new Point( coord_allign.X*k_res, coord_allign.Y * k_res), new Scalar(0, 0, 255), MarkerTypes.Cross, 3, 1);
                Cv2.ImShow("orig_place", orig_place_end);
                Cv2.ImShow("area_for_allign", mat_allign);
                Cv2.ImShow("map_diff", kernel);
                Cv2.WaitKey();
            }
           
            
            if (!finded) return new Point(wind_x/2, wind_y/2);
            //Console.WriteLine("__________________"+val_al);
            return coord_allign;
        }
        public Point estimated_allign(Mat area_for_allign, Mat mat_allign, bool debug = false)
        {
            var area_for_allign_gray = new Mat();
            var mat_allign_gray = new Mat();

            Cv2.CvtColor(area_for_allign, area_for_allign_gray, ColorConversionCodes.RGB2GRAY);
            Cv2.CvtColor(mat_allign, mat_allign_gray, ColorConversionCodes.RGB2GRAY);
            var coord_allign = new Point(0, 0);
            var wind_x = area_for_allign.Width - mat_allign.Width;
            var wind_y = area_for_allign.Height - mat_allign.Height;
            double val_min = double.MaxValue;
            bool finded = false;

            var val_al = mat_allign.Mean().Val0;
            if (val_al < 0.5) return new Point(wind_x / 2, wind_y / 2);

            var data_diff = new byte[wind_y, wind_x];

            for (int x = 0; x < wind_x-1; x++)
            {
                for (int y = 0; y < wind_y - 1; y++)
                {
                    var roi_for_allign = new Rect(new Point(x, y), new OpenCvSharp.Size(mat_allign.Width, mat_allign.Height));
                    var orig_place_gray = new Mat(area_for_allign_gray, roi_for_allign);

                    var diff1 = (orig_place_gray - mat_allign_gray).ToMat();
                    var diff2 = (mat_allign_gray - orig_place_gray).ToMat();
                    var val1 = diff1.Mean().Val0;
                    var val2 = diff2.Mean().Val0;
                    var val = val1;
                    data_diff[y, x] = (byte)(val * 150);
                    if (debug)
                    {
                        Console.WriteLine(x + " " + y + " " + val);
                        Cv2.PutText(diff1, Math.Round(val, 3).ToString(), new OpenCvSharp.Point(10, 20), HersheyFonts.HersheyPlain, 3, new Scalar(255), 1);
                        Cv2.ImShow("diff1", diff1);

                        // Cv2.ImShow("diff2", diff2);
                        Cv2.WaitKey();
                    }


                    if (val < val_min)
                    {
                        val_min = val;
                        coord_allign = new Point(x, y);
                        finded = true;
                        //Console.WriteLine(x+" " + y+" "+val_min);
                    }
                }
            }
            Console.WriteLine(coord_allign.X + " " + coord_allign.Y + " " + val_min);
            var roi_for_allign_end = new Rect(coord_allign, new OpenCvSharp.Size(mat_allign.Width, mat_allign.Height));
            var orig_place_end = new Mat(area_for_allign, roi_for_allign_end);
            var kernel = Mat.FromPixelData(wind_x, wind_y, MatType.CV_8UC1, data_diff);
            //Cv2.ConvertScaleAbs(kernel, kernel);
            //Cv2.Normalize(kernel, kernel);
            if(debug)
            {
                var k_res = 10;
                Cv2.CvtColor(kernel, kernel, ColorConversionCodes.GRAY2RGB);

                // Cv2.DrawMarker(kernel, coord_allign, new Scalar(255), MarkerTypes.TiltedCross, 5, 1);
                Cv2.Resize(kernel, kernel, new OpenCvSharp.Size(kernel.Width * k_res, kernel.Height * k_res));
                Cv2.DrawMarker(kernel, new Point(coord_allign.X * k_res, coord_allign.Y * k_res), new Scalar(0, 0, 255), MarkerTypes.Cross, 3, 1);
                Cv2.ImShow("orig_place", orig_place_end);
                Cv2.ImShow("area_for_allign", mat_allign);
                Cv2.ImShow("map_diff", kernel);
                Cv2.WaitKey();
            }


            if (!finded) return new Point(wind_x / 2, wind_y / 2);
            //Console.WriteLine("__________________"+val_al);
            return coord_allign;
        }
        public Vec3f[] get_centres_objects(double form, double form_err, double circle_diametr_mm, double diametr_err_mm)
        {
            var mat = mat_common.Clone();
            var thr = new Mat();
            Cv2.CvtColor(mat, thr, ColorConversionCodes.RGB2GRAY);
            Cv2.GaussianBlur(thr, thr, new OpenCvSharp.Size(7, 7), -1);
            Cv2.Threshold(thr, thr, bin, 255, ThresholdTypes.Binary);
            var contours = new OpenCvSharp.Point[0][];
            var hier = new HierarchyIndex[0];
            Cv2.FindContours(thr, out contours, out hier, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
           
            contours = filtr_contours(contours, form, form_err, circle_diametr_mm * mm_pixel_ratio_image, diametr_err_mm * mm_pixel_ratio_image);
            //Cv2.DrawContours(mat, contours, -1, new Scalar(255, 0, 0), 1, LineTypes.Link8);
            mat = draw_contours_with_info(mat.Clone(), contours, pixel_mm_ratio_common);
            mat = draw_contours_centres(mat.Clone(), contours);
            Cv2.ImShow("conts ",mat);
            return null;
        }

        public static Point[][] filtr_contours(Point[][]  contours, double form, double form_err, double circle_diametr_pix, double diametr_err_pix)
         {
            if(contours == null) return null;
            if(contours.Length == 0) return null;
            var contours_filtr = new List<Point[]>();
           
            for (int i = 0; i < contours.Length; i++)
            {
                var s = Cv2.ContourArea(contours[i]);
                var p = Cv2.ArcLength(contours[i],true);
                var d_red = 2*Math.Sqrt(s / Math.PI);
                var form_cont = s/(p*p);

                if(Math.Abs(d_red- circle_diametr_pix) < diametr_err_pix && Math.Abs(form_cont - form) < form*form_err)
                {
                    contours_filtr.Add(contours[i]);
                }
            }

            return contours_filtr.ToArray();
        }

        public static Mat draw_contours_with_info(Mat mat,Point[][] contours, double mm_pixel_ratio_image)
        {
            Cv2.DrawContours(mat, contours, -1, new Scalar(255, 0, 0), 1, LineTypes.Link8);
            for (int i = 0; i < contours.Length; i++)
            {
                var s = Cv2.ContourArea(contours[i]);
                var p = Cv2.ArcLength(contours[i], true);
                var d_red = 2 * Math.Sqrt(s / Math.PI);
                var form_cont = s / (p * p);
                var pc = ImageProcessing.findCentrCont(contours[i]);

                string info = "<- " + Math.Round(1000 * d_red * mm_pixel_ratio_image, 1) + "um" + "; "+ Math.Round(1000 * form_cont, 1) + "/" + Math.Round(1000 * 1 / (4 * Math.PI), 1);
                var dx_offset = 30;
                Cv2.PutText(mat, info, new Point(pc.X+ dx_offset, pc.Y), HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 0, 0), 3);
                Cv2.PutText(mat,info,new Point(pc.X + dx_offset, pc.Y),HersheyFonts.HersheySimplex,0.5,new Scalar(0,255,0),1);
               
            }

            return mat;
        }

        public static Mat draw_contours_centres(Mat mat, Point[][] contours)
        {
            for (int i = 0; i < contours.Length; i++)
            {
                var pc = ImageProcessing.findCentrCont(contours[i]);
                Cv2.DrawMarker(mat, new Point(pc.X, pc.Y), new Scalar(0, 0, 0), MarkerTypes.TiltedCross, 20,3);
                Cv2.DrawMarker(mat, new Point(pc.X, pc.Y), new Scalar(255, 255, 0), MarkerTypes.TiltedCross, 20, 1);
            }

            return mat;
        }
        public static bool ContourTouchesBorder(Point[] contour, int imgWidth, int imgHeight)
        {
            // Проверяем, есть ли хотя бы одна точка контура на границе изображения
            return contour.Any(p =>
                p.X <= 1 || p.X >= imgWidth - 1 ||
                p.Y <= 1 || p.Y >= imgHeight - 1);

            // Используем <= 1 и >= width-1/height-1 для учета возможной погрешности
        }

    }
    public class ContourCV
    {
        public OpenCvSharp.Point[] cont_orig;
        public double perim;
        public double area;
        public double fig;//  fig = area/perim^2
        public OpenCvSharp.Point2f pc;
        public ContourCV(Point[] cont)
        {
            area = Cv2.ContourArea(cont);
            perim = Cv2.ArcLength(cont, true);
            fig = area / (perim* perim);
            pc = ImageProcessing.findCentrCont(cont);
            cont_orig = cont;
        }

        static public ContourCV[] contourCVs(Point[][] conts)
        {
            var conts_cv = new List<ContourCV>();
            for (int i = 0; i < conts.Length; i++)
            {
                conts_cv.Add(new ContourCV(conts[i]));
            }
            return conts_cv.ToArray();
        }

    }
}
