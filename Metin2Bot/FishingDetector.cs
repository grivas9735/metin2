

namespace Metin2Bot
{
    using OpenCvSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class FishingDetector
    {
        public static (int X, int Y, int Width, int Height) DetectObjectColor(
            Mat image,
            Scalar colorLBound,
            Scalar colorHBound)
        {
            using Mat hsv = new();

            // Python:
            // cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
            Cv2.CvtColor(
                image,
                hsv,
                ColorConversionCodes.BGR2HSV);

            using Mat mask = new();

            // Python:
            // cv2.inRange(into_hsv, color_lbound, color_hbound)
            Cv2.InRange(
                hsv,
                colorLBound,
                colorHBound,
                mask);

            // Python:
            // cv2.findContours(
            //     b_mask,
            //     cv2.RETR_EXTERNAL,
            //     cv2.CHAIN_APPROX_SIMPLE
            // )
            Cv2.FindContours(
                mask,
                out Point[][] contours,
                out HierarchyIndex[] hierarchy,
                RetrievalModes.External,
                ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
                throw new InvalidOperationException("No objects found");

            Point[] largestContour = contours[0];
            double largestArea = Cv2.ContourArea(largestContour);

            for (int i = 1; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);

                if (area > largestArea)
                {
                    largestArea = area;
                    largestContour = contours[i];
                }
            }

            Rect rect = Cv2.BoundingRect(largestContour);

            return (
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height
            );
        }

        public static bool IsFishable(Mat image)
        {
            using Mat hsv = new();

            Cv2.CvtColor(
                image,
                hsv,
                ColorConversionCodes.BGR2HSV);

            using Mat mask = new();

            Cv2.InRange(
                hsv,
                new Scalar(118, 56, 141),
                new Scalar(255, 144, 255),
                mask);

            Scalar sum = Cv2.Sum(mask);

            return sum.Val0 > 40000;
        }
    }
}
