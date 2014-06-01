using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public interface IPreProcessingStage
    {
        IplImage PreProcess(IplImage image);
    }
}