using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class MaskStage : IPreProcessingStage
    {
        public IplImage Mask { get; set; }
        
        public static MaskStage BuildDefaultMask()
        {
            var stage = new MaskStage();
            stage.Mask = new IplImage(1920, 1080, BitDepth.U8, 1);
            stage.Mask.Zero();
            stage.Mask.FillConvexPoly(new[]
                {
                    new CvPoint(0,0), 
                    new CvPoint(600,0), 
                    new CvPoint(960, 500), 
                    new CvPoint(0, 900), 
                }, CvColor.White, LineType.Link4);

            stage.Mask.FillConvexPoly(new[]
                {
                    new CvPoint(960,500), 
                    new CvPoint(1200,0), 
                    new CvPoint(1920, 0), 
                    new CvPoint(1920, 900), 
                }, CvColor.White, LineType.Link4);
            stage.Mask.FillConvexPoly(new[]
                {
                    new CvPoint(0,200), 
                    new CvPoint(1920,200), 
                    new CvPoint(1920, 600), 
                    new CvPoint(0, 600), 
                }, CvColor.White, LineType.Link4);

            return stage;
        }

        public IplImage PreProcess(IplImage image)
        {
            var maskedImage = new IplImage(image.Size, image.Depth, image.NChannels);
            image.Copy(maskedImage, Mask);
            return maskedImage;
        }
    }
}