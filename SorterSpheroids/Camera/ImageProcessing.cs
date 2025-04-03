
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
        /*public static Mat get_focal_surface(Mat mat)
        {
            var lapl = new Mat();
            var im = mat.ToImage<Gray, byte>();
            Cv2.Laplacian(im, lapl, DepthType.Default);
            Cv2.Threshold(lapl, lapl, 20, 255, ThresholdType.Binary);
            var im_th = lapl.ToImage<Gray, byte>();
            Mat kernel7 = Cv2.GetStructuringElement(ElementShape.Rectangle, new Size(7, 7), new Point(3, 3));

            Mat kernel5 = Cv2.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(2, 2));
            Mat kernel3 = Cv2.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(1, 1));

            Mat ellips7 = Cv2.GetStructuringElement(ElementShape.Ellipse, new Size(7, 7), new Point(1, 1));
            int num = 4;
            Image<Gray, Byte> im_med = im_th;
            for (int i = 0; i < num; i++)
            {
                im_med = im_med.MorphologyEx(MorphOp.Dilate, ellips7, new Point(-1, -1), 3, BorderType.Default, new MCvScalar());
                im_med = im_med.MorphologyEx(MorphOp.Close, ellips7, new Point(-1, -1), 10, BorderType.Default, new MCvScalar());
            }
            //var mat_r = new Image<>
            var ret = new Mat();
            var spl = mat.Split();
            spl[1] += im_med.Mat * 0.3;
            Cv2.Merge(new VectorOfMat(spl), ret);
            return ret;
        }

        public static (Mat,double) get_focal_surface_for_conf(Mat mat)
        {
            var lapl = new Mat();
            var im = mat.ToImage<Gray, byte>();
            Cv2.Laplacian(im, lapl, DepthType.Default);
            var r = im.GetAverage();
            Cv2.Threshold(lapl, lapl, 20, 255, ThresholdType.Binary);
            var conf = new Mat();
            Cv2.BitwiseAnd(mat, mat, conf, lapl);

            return (conf,r.Intensity);
        }*/

    }
}
