using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public interface IPostProcessingStage
    {
        IplImage PostProcess(IplImage preProcessedImage, IplImage postProcessedImage);
    }
}