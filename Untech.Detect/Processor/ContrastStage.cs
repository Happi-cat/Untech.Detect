using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class ContrastStage : IPreProcessingStage
    {
        public IplImage PreProcess(IplImage image)
        {
            double min, max;
            Cv.MinMaxLoc(image, out min, out max);

            image.GetSize()
        }
    }
}