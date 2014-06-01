using System.Diagnostics;
using OpenCvSharp;
using Untech.Detect.Processor.Helpers;

namespace Untech.Detect.Processor
{
    public class HistogramContrastStage : IPreProcessingStage
    {
        public HistogramContrastStage()
        {
            Percentage = 0;
        }

        public float Percentage { get; set; }
        private int MaxValue { get; set; }

        public IplImage PreProcess(IplImage image)
        {
            MaxValue = (1 << BitDepthHelper.BitDepthToInt(image.Depth)) - 1;

            var histogram = new CvHistogram(new[] {64}, HistogramFormat.Array, new[] {new float[] {0, 256}},
                                            true);

            histogram.Calc(image);

            float min, max;
            float alpha = 0;
            float beta = 0;
            float diff;

            histogram.GetMinMaxValue(out min, out max);

            diff = max - min;
            max = max - diff * Percentage;
            min = min + diff * Percentage;

            beta = -min;
            alpha = MaxValue / (max - min);

            Debug.Print("[HISTO] Max value: {0}", MaxValue);
            Debug.Print("[HISTO] Min: {0}", min);
            Debug.Print("[HISTO] Max: {0}", max);
            Debug.Print("[HISTO] Alpha: {0}", alpha);
            Debug.Print("[HISTO] Beta: {0}", beta);

            var scaledImage = new IplImage(image.Size, image.Depth, image.NChannels);
            image.ConvertScale(scaledImage, alpha, beta);


            return scaledImage;
        }
    }
}