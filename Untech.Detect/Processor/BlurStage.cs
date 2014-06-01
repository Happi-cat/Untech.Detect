using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class BlurStage:IPreProcessingStage
    {
        public SmoothType SmoothType { get; set; }
        public int Param1 { get; set; }

        public static BlurStage BuildBlur3()
        {
            var stage = new BlurStage {SmoothType = SmoothType.Blur, Param1 = 3};
            return stage;
        }

        public IplImage PreProcess(IplImage image)
        {
            image.Smooth(image, SmoothType, Param1);
            return image;
        }
    }
}
