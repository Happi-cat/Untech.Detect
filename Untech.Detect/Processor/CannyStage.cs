using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class CannyStage : IPreProcessingStage
    {
        public IplImage PreProcess(IplImage image)
        {
            var cannyImage = new IplImage(image.Size, BitDepth.U8, 1);
            Cv.Canny(image, cannyImage, 90, 130, ApertureSize.Size3);
            return cannyImage;
        }
    }
}