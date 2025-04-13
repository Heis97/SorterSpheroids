
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
        
    }

    public class ImageCoordinatsConverter
    {
        public Mat mat_common;
        public int w_pix, h_pix;
        public double x_mm, y_mm, w_mm, h_mm, pixel_mm_ratio_default, mm_pixel_ratio_image;
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
            mat_common = new Mat((int)(mm_pixel_ratio_image * w_mm), (int)(mm_pixel_ratio_image * h_mm), MatType.CV_8UC3);
        }

        public void add_image(Mat mat, GFrame frame)
        {
            var frame_mm = mat.Width * pixel_mm_ratio_default;



            var scale_f = frame_mm / w_mm;
            mat = mat.Resize(new OpenCvSharp.Size( mat.Width*scale_f, mat.Height*scale_f));
            var x = (frame.x - x_mm) * mm_pixel_ratio_image;
            var y = (frame.y - y_mm) * mm_pixel_ratio_image;
           
            var rect_for_ins = new Rect(new Point(x,y),mat.Size());


            mat.CopyTo(new Mat(mat_common, rect_for_ins));
           Cv2.ImShow("tets1", mat_common);
            Cv2.WaitKey();
        }

        Vec3f[] get_centres_objects(Point[] points)
        {
            return null;
        }

        public Vec3f[] get_centres_objects()
        {

            return null;
        }
    }
}
