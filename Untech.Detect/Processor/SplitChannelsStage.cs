﻿using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class SplitChannelsStage : IPreProcessingStage
    {
        public ColorConversion ColorConversion { get; set; }
        public int Channel { get; set; }

        public IplImage PreProcess(IplImage image)
        {
            var splittedImage = new IplImage(image.Size, BitDepth.U8, 1);
            using (var coloredImage = new IplImage(image.Size, image.Depth, image.NChannels))
            {

                Cv.CvtColor(image, coloredImage, ColorConversion);
                switch (Channel)
                {
                    case 0:
                        Cv.Split(coloredImage, splittedImage, null, null, null);
                        break;
                    case 1:
                        Cv.Split(coloredImage, null, splittedImage, null, null);
                        break;
                    case 2:
                        Cv.Split(coloredImage, null, null, splittedImage, null);
                        break;
                    default:
                        Cv.Split(coloredImage, splittedImage, null, null, null);
                        break;
                }
            }
            return splittedImage;
        }
    }
}