using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class CloneStage : IPreProcessingStage, IPostProcessingStage
    {
        public IplImage PreProcess(IplImage image)
        {
            return image.Clone();
        }

        public IplImage PostProcess(IplImage preProcessedImage, IplImage postProcessedImage)
        {
            return postProcessedImage.Clone();
        }
    }
}