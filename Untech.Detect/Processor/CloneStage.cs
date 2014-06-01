using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class CloneStage : IPreProcessingStage
    {
        public IplImage PreProcess(IplImage image)
        {
            return image.Clone();
        }
    }
}