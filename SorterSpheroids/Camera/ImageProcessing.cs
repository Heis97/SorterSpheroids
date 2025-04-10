
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        /*
        static public Mat sobel_mat(Mat mat, bool simple = false)
        {
            simple = false;
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
            var gray_x = mat.Clone();
            var gray_y = mat.Clone();
            var gray_xn = mat.Clone();
            var gray_yn = mat.Clone();

            var gray_xy_r = mat.Clone();
            var gray_xy_rn = mat.Clone();
            var gray_xy_l = mat.Clone();
            var gray_xy_ln = mat.Clone();

            var med = 10;
            var min = 3;

            var med_xy = 14;
            var min_xy = 5;

            Point anchor = new Point(-1, -1);
            var data = new float[,] {
                {  -med_xy,-min_xy, 0 },
                { -min_xy, 0f, min_xy },
                { 0, min_xy,  med_xy } };
            var kernel_xy_ln = new Mat(3, 3, MatType.CV_32FC1, data);


            Matrix<float> kernel_x = new Matrix<float>(new float[3, 3] {
                { -min, 0f, min },
                { - med, 0f, med },
                { -min, 0f, min } });
            Matrix<float> kernel_y = new Matrix<float>(new float[3, 3] {
                { min, med, min },
                { 0f, 0f, 0f },
                { -min, -med, -min} });

            Matrix<float> kernel_xn = new Matrix<float>(new float[3, 3] {
                { min, 0f, -min },
                { med, 0f, -med },
                { min, 0f, -min } });

            Matrix<float> kernel_yn = new Matrix<float>(new float[3, 3] {
                { -min, -med, -min },
                { 0f, 0f, 0f },
                { min, med, min} });

            Matrix<float> kernel_xy_r = new Matrix<float>(new float[3, 3] {
                { 0, min_xy, med_xy },
                { -min_xy, 0f, min_xy },
                { -med_xy, -min_xy, 0} });

            Matrix<float> kernel_xy_rn = new Matrix<float>(new float[3, 3] {
                { 0, -min_xy,-med_xy },
                { min_xy, 0f, -min_xy },
                { med_xy, min_xy, 0} });

            Matrix<float> kernel_xy_l = new Matrix<float>(new float[3, 3] {
                {  med_xy, min_xy, 0 },
                { min_xy, 0f, -min_xy },
                { 0, -min_xy,  -med_xy } });
           
            Cv2.Filter2D(mat, gray_yn, MatType.CV_32FC1, kernel_yn, anchor);
            Cv2.Filter2D(mat, gray_xn, MatType.CV_32FC1, kernel_xn, anchor);
            Cv2.Filter2D(mat, gray_x, MatType.CV_32FC1, kernel_x, anchor);
            Cv2.Filter2D(mat, gray_y, MatType.CV_32FC1, kernel_y, anchor);

            Cv2.Filter2D(mat, gray_xy_r, MatType.CV_32FC1, kernel_xy_r, anchor);
            Cv2.Filter2D(mat, gray_xy_rn, MatType.CV_32FC1, kernel_xy_rn, anchor);
            Cv2.Filter2D(mat, gray_xy_l, MatType.CV_32FC1, kernel_xy_l, anchor);
            Cv2.Filter2D(mat, gray_xy_ln, MatType.CV_32FC1, kernel_xy_ln, anchor);

            //  CvInvoke.ConvertScaleAbs(gray_x, gray_x, 1, 0);
            //CvInvoke.ConvertScaleAbs(gray_y, gray_y, 1, 0);
            return 0.2 * gray_x + 0.2 * gray_y + 0.2 * gray_xn + 0.2 * gray_yn
                + 0.2 * gray_xy_r + 0.2 * gray_xy_rn + 0.2 * gray_xy_l + 0.2 * gray_xy_ln;

        }
        */
    }
}
