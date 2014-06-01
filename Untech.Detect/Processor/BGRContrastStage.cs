using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenCvSharp;
using Untech.Detect.Processor.Helpers;

namespace Untech.Detect.Processor
{
    public class BGRContrastStage : IPreProcessingStage
    {
        public BGRContrastStage()
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
            
            var grayImage = new IplImage(image.Size, BitDepth.U8, 1);
            Cv.CvtColor(image, grayImage, ColorConversion.BgrToGray);

            double min, max;
            double alpha = .0;
            double beta = 0;
            double diff;

            Cv.MinMaxLoc(grayImage, out min, out max);
            grayImage.Dispose();

            diff = max - min;
            max = max - diff*UpperPercentage;
            min = min + diff*LowerPercentage;

            beta = -min;
            alpha = MaxValue / (max - min);

            Debug.Print("[BGR] Max value: {0}", MaxValue);
            Debug.Print("[BGR] Min: {0}", min);
            Debug.Print("[BGR] Max: {0}", max);
            Debug.Print("[BGR] Alpha: {0}", alpha);
            Debug.Print("[BGR] Beta: {0}", beta);

            var scaledImage = new IplImage(image.Size, image.Depth, image.NChannels);
            image.ConvertScale(scaledImage, alpha, beta);

            
            return scaledImage;
        }
    }
}
