
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Cv2.Laplacian(mat, lapl, MatType.CV_8UC1);
            var mean = lapl.Mean();
            Cv2.Threshold(lapl, lapl, bin, 255, ThresholdTypes.Binary);
            var conf = mat.Clone();
            //mat.ConvertTo(mat, MatType.CV_8U);
            //BitwiseAnd(mat, mat, conf, lapl);




            return (lapl, mean.Val0);
        }
        /* public static (Mat, double) get_focal_surface_from_mats(Mat[] mat)
         {


             return (lapl, mean.Val0);
         }*/

    }
}
