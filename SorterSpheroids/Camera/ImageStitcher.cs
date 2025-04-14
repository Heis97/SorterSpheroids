using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorterSpheroids
{
    public class ImageStitcher
    {
        public Mat StitchImages(ImageInfo[] images, double blendingRatio = 0.5)
        {
            if (images == null || images.Length == 0)
                throw new ArgumentException("No images provided");

            // Start with the first image as the base
            Mat result = images[0].Image.Clone();
            Rect resultRoi = new Rect(0, 0, result.Width, result.Height);

            for (int i = 1; i < images.Length; i++)
            {
                Mat currentImage = images[i].Image;
                Point estimatedOffset = images[i].EstimatedPosition;
                Point estimatedError = images[i].EstimatedError;

                // Find the best overlap between the current result and the new image
                Point optimalOffset = FindOptimalOverlap_self(result, currentImage, estimatedOffset, estimatedError);

                // Create a new canvas that can contain both images
                Rect newCanvas = CalculateNewCanvas(resultRoi, new Rect(optimalOffset.X, optimalOffset.Y, currentImage.Width, currentImage.Height));

                // If the new image extends beyond the current canvas, resize the canvas
                if (newCanvas != resultRoi)
                {
                    Mat newResult = new Mat(newCanvas.Height, newCanvas.Width, result.Type(), Scalar.Black);
                    result.CopyTo(newResult[resultRoi]);
                    result = newResult;
                    resultRoi = new Rect(resultRoi.X, resultRoi.Y, resultRoi.Width, resultRoi.Height);
                }

                // Blend the images in the overlapping region
                result = BlendImages(result, currentImage, optimalOffset, blendingRatio);
                Console.WriteLine("stitching: " + i + "/" + images.Length+" "+ optimalOffset+" "+ estimatedOffset);
            }

            return result;
        }

        private Point FindOptimalOverlap(Mat baseImage, Mat newImage, Point estimatedOffset, Point estimatedError)
        {
            // Define search window around the estimated offset
            int searchWidth = estimatedError.X * 2;
            int searchHeight = estimatedError.Y * 2;

            // Crop regions of interest for matching
            Rect baseRoi = new Rect(
                Math.Max(0, estimatedOffset.X - estimatedError.X),
                Math.Max(0, estimatedOffset.Y - estimatedError.Y),
                Math.Min(searchWidth, baseImage.Width - (estimatedOffset.X - estimatedError.X)),
                Math.Min(searchHeight, baseImage.Height - (estimatedOffset.Y - estimatedError.Y)));

            Rect newRoi = new Rect(
                Math.Max(0, -estimatedOffset.X + estimatedError.X),
                Math.Max(0, -estimatedOffset.Y + estimatedError.Y),
                Math.Min(searchWidth, newImage.Width - (-estimatedOffset.X + estimatedError.X)),
                Math.Min(searchHeight, newImage.Height - (-estimatedOffset.Y + estimatedError.Y)));

            if (baseRoi.Width <= 0 || baseRoi.Height <= 0 || newRoi.Width <= 0 || newRoi.Height <= 0)
            {
                return estimatedOffset; // Fallback if ROIs are invalid
            }

            Mat basePatch = baseImage[baseRoi];
            Mat newPatch = newImage[newRoi];

            // Convert to grayscale for template matching
            Mat baseGray = new Mat();
            Mat newGray = new Mat();
            Cv2.CvtColor(basePatch, baseGray, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(newPatch, newGray, ColorConversionCodes.BGR2GRAY);

            // Perform template matching
            Mat result = new Mat();
            Cv2.MatchTemplate(baseGray, newGray, result, TemplateMatchModes.CCoeffNormed);

            // Find the best match location
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out Point maxLoc);

            // If the match quality is too low, fall back to estimated offset
            if (maxVal < 0.5)
            {
                return estimatedOffset;
            }

            // Calculate the optimal offset based on the template matching result
            Point optimalOffset = new Point(
                estimatedOffset.X + (maxLoc.X - estimatedError.X),
                estimatedOffset.Y + (maxLoc.Y - estimatedError.Y));

            return optimalOffset;
        }

        private Point FindOptimalOverlap_self(Mat baseImage, Mat newImage, Point estimatedOffset, Point estimatedError)
        {
            //Define search window around the estimated offset
            int searchWidth = estimatedError.X * 2;
            int searchHeight = estimatedError.Y * 2;

            //Crop regions of interest for matching
            Rect baseRoi = new Rect(
                Math.Max(0, estimatedOffset.X - estimatedError.X),
                Math.Max(0, estimatedOffset.Y - estimatedError.Y),
                Math.Min(searchWidth, baseImage.Width - (estimatedOffset.X - estimatedError.X)),
                Math.Min(searchHeight, baseImage.Height - (estimatedOffset.Y - estimatedError.Y)));

            Rect newRoi = new Rect(
                Math.Max(0, -estimatedOffset.X + estimatedError.X),
                Math.Max(0, -estimatedOffset.Y + estimatedError.Y),
                Math.Min(searchWidth, newImage.Width - (-estimatedOffset.X + estimatedError.X)),
                Math.Min(searchHeight, newImage.Height - (-estimatedOffset.Y + estimatedError.Y)));

            if (baseRoi.Width <= 0 || baseRoi.Height <= 0 || newRoi.Width <= 0 || newRoi.Height <= 0)
            {
                return estimatedOffset; // Fallback if ROIs are invalid
            }

            Mat basePatch = baseImage[baseRoi];
            Mat newPatch = newImage[newRoi];

            // Convert to grayscale for template matching
            Mat baseGray = new Mat();
            Mat newGray = new Mat();
            Cv2.CvtColor(basePatch, baseGray, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(newPatch, newGray, ColorConversionCodes.BGR2GRAY);

            // Perform template matching
            Mat result = new Mat();
            Cv2.MatchTemplate(baseGray, newGray, result, TemplateMatchModes.CCoeffNormed);

            // Find the best match location
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out Point maxLoc);

            // If the match quality is too low, fall back to estimated offset
            if (maxVal < 0.5)
            {
                return estimatedOffset;
            }

            // Calculate the optimal offset based on the template matching result
            Point optimalOffset = new Point(
                estimatedOffset.X + (maxLoc.X - estimatedError.X),
                estimatedOffset.Y + (maxLoc.Y - estimatedError.Y));

            return optimalOffset;
        }

        private Rect CalculateNewCanvas(Rect currentCanvas, Rect newImageRect)
        {
            int x1 = Math.Min(currentCanvas.X, newImageRect.X);
            int y1 = Math.Min(currentCanvas.Y, newImageRect.Y);
            int x2 = Math.Max(currentCanvas.Right, newImageRect.Right);
            int y2 = Math.Max(currentCanvas.Bottom, newImageRect.Bottom);

            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        private Mat BlendImages(Mat baseImage, Mat newImage, Point offset, double ratio)
        {
            // Determine the overlapping region
            int overlapX1 = Math.Max(0, offset.X);
            int overlapY1 = Math.Max(0, offset.Y);
            int overlapX2 = Math.Min(baseImage.Width, offset.X + newImage.Width);
            int overlapY2 = Math.Min(baseImage.Height, offset.Y + newImage.Height);

            if (overlapX1 >= overlapX2 || overlapY1 >= overlapY2)
            {
                // No overlap, just copy the new image
                newImage.CopyTo(baseImage[new Rect(offset.X, offset.Y, newImage.Width, newImage.Height)]);
                return baseImage;
            }

            // Blend the overlapping region
            for (int y = overlapY1; y < overlapY2; y++)
            {
                for (int x = overlapX1; x < overlapX2; x++)
                {
                    int newX = x - offset.X;
                    int newY = y - offset.Y;

                    if (newX >= 0 && newX < newImage.Width && newY >= 0 && newY < newImage.Height)
                    {
                        Vec3b basePixel = baseImage.At<Vec3b>(y, x);
                        Vec3b newPixel = newImage.At<Vec3b>(newY, newX);

                        // Simple alpha blending
                        basePixel.Item0 = (byte)(basePixel.Item0 * (1 - ratio) + newPixel.Item0 * ratio);
                        basePixel.Item1 = (byte)(basePixel.Item1 * (1 - ratio) + newPixel.Item1 * ratio);
                        basePixel.Item2 = (byte)(basePixel.Item2 * (1 - ratio) + newPixel.Item2 * ratio);

                        baseImage.Set(y, x, basePixel);
                    }
                }
            }

            // Copy non-overlapping parts of the new image
            if (offset.X + newImage.Width > baseImage.Width)
            {
                int width = offset.X + newImage.Width - baseImage.Width;
                newImage[new Rect(baseImage.Width - offset.X, 0, width, newImage.Height)]
                    .CopyTo(baseImage[new Rect(baseImage.Width, offset.Y, width, newImage.Height)]);
            }

            if (offset.Y + newImage.Height > baseImage.Height)
            {
                int height = offset.Y + newImage.Height - baseImage.Height;
                newImage[new Rect(0, baseImage.Height - offset.Y, newImage.Width, height)]
                    .CopyTo(baseImage[new Rect(offset.X, baseImage.Height, newImage.Width, height)]);
            }
            return baseImage;
        }

    }

    public class ImageInfo
    {
        public Mat Image { get; set; }
        public Point EstimatedPosition { get; set; } // Position relative to the first image
        public Point EstimatedError { get; set; }    // Estimated error in pixels (x,y)
    }

/*
    static void Main(string[] args)
        {
            // Load images
            Mat image1 = Cv2.ImRead("image1.jpg", ImreadModes.Color);
            Mat image2 = Cv2.ImRead("image2.jpg", ImreadModes.Color);
            Mat image3 = Cv2.ImRead("image3.jpg", ImreadModes.Color);

            // Create image list with estimated positions and errors
            var images = new List<ImageInfo>
        {
            new ImageInfo { Image = image1, EstimatedPosition = new Point(0, 0), EstimatedError = new Point(0, 0) },
            new ImageInfo { Image = image2, EstimatedPosition = new Point(image1.Width - 200, 50), EstimatedError = new Point(20, 20) },
            new ImageInfo { Image = image3, EstimatedPosition = new Point(image1.Width + image2.Width - 400, -30), EstimatedError = new Point(30, 30) }
        };

            // Stitch images
            var stitcher = new ImageStitcher();
            Mat result = stitcher.StitchImages(images, 0.5);

            // Save result
            Cv2.ImWrite("stitched_result.jpg", result);

            // Clean up
            image1.Dispose();
            image2.Dispose();
            image3.Dispose();
            result.Dispose();
        }*/
    }
