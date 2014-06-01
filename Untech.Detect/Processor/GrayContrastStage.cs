using System.Diagnostics;
using OpenCvSharp;
using Untech.Detect.Processor.Helpers;

namespace Untech.Detect.Processor
{
    public class GrayContrastStage : IPreProcessingStage
    {
        public GrayContrastStage()
        {
            LowerPercentage = 0.1;
            UpperPercentage = 0.1;
        }

        public double LowerPercentage { get; set; }
        public double UpperPercentage { get; set; }
        private int MaxValue { get; set; }

        public IplImage PreProcess(IplImage image)
        {
            MaxValue = (1 << BitDepthHelper.BitDepthToInt(image.Depth)) - 1;

            double min, max;
            double alpha = .0;
            double beta = 0;
            double diff;

            Cv.MinMaxLoc(image, out min, out max);
            diff = max - min;
            max = max - diff * UpperPercentage;
            min = min + diff * LowerPercentage;

            beta = -min;
            alpha = MaxValue / (max - min);

            Debug.Print("Max value: {0}", MaxValue);
            Debug.Print("Min: {0}",min);
            Debug.Print("Max: {0}", max);
            Debug.Print("Alpha: {0}", alpha);
            Debug.Print("Beta: {0}", beta);

            var scaledImage = new IplImage(image.Size, image.Depth, image.NChannels);
            image.ConvertScale(scaledImage, alpha, beta);
            
            return scaledImage;
        }
    }
}