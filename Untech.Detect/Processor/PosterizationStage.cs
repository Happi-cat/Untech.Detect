using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class PosterizationStage : IPreProcessingStage
    {
        public CvMat LUT { get; set; }

        public static PosterizationStage BuildDefaultLUT(int levels)
        {
            var stage = new PosterizationStage();

            double factor = (double) 256/levels;

            stage.LUT = new CvMat(1, 256, MatrixType.U8C1);
            for (int i = 0; i < 128; i++)
            {
                stage.LUT[0, i] = factor * (i / factor);
            }
            for (int i = 128; i < 256; i++)
            {
                stage.LUT[0, i] = factor * (1 + (i / factor)) - 1;
            }

            return stage;
        }

        public static PosterizationStage BuildNegativeLUT()
        {
            var stage = new PosterizationStage();

            stage.LUT = new CvMat(1, 256, MatrixType.U8C1);
            for (int i = 0; i < 256; i++)
            {
                stage.LUT[0, i] = 255 - i;
            }

            return stage;
        }

        public static PosterizationStage BuildColoredLUT(int colors)
        {
            var stage = new PosterizationStage();

            int colorStep = 256/colors;
            stage.LUT = new CvMat(1, 256, MatrixType.U8C1);
            for (int i = 0, color = 0; i < 256; i++)
            {
                stage.LUT[0, i] = color;
                if (i % colorStep == colorStep - 1)
                    color+=colorStep;

            }

            return stage;
        }

        public IplImage PreProcess(IplImage image)
        {
            Cv.LUT(image, image, LUT);
            return image;
        }
    }
}