using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Untech.Detect.Processor;
using OpenCvSharp;

namespace Untech.Detect
{
    class Program
    {
        private static void Main(string[] args)
        {
            var processor = BuildProcessor();

            var files = new List<string>();

            var path = args[0];
            if (File.Exists(path))
            {
                files.Add(path);
            }
            else if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path, "*.mp4"));
            }
            
            foreach (var file in files)
            {
                using (var capture = CvCapture.FromFile(file))
                using (var windowOriginal  = new CvWindow("Original", WindowMode.AutoSize))
                using (var windowPreImage  = new CvWindow("Pre-Image", WindowMode.AutoSize))
                using (var windowPostImage = new CvWindow("Post-Image", WindowMode.AutoSize))
                {
                    processor.OriginalWindow = windowOriginal;
                    processor.PreProcessedWindow = windowPreImage;
                    processor.PostProcessedWindow = windowPostImage;
                    processor.Process(capture);
                }
            }
        }      

        private static VideoStreamProcessor BuildProcessor()
        {
            var processor = new VideoStreamProcessor {FrameSkipValue = 2};

            processor.PreProcessingStages.Add(new CloneStage());
            processor.PreProcessingStages.Add(new SplitChannelsStage
                {
                    Channel = 1,
                    ColorConversion = ColorConversion.BgrToHsv_Full
                });
            processor.PreProcessingStages.Add(MaskStage.BuildDefaultMask());
            processor.PreProcessingStages.Add(new ContrastStage());
            processor.PreProcessingStages.Add(new PosterizationStage());
            processor.PreProcessingStages.Add(new CannyStage());

            processor.PostProcessingStages.Add(new DetectionStage());

            return processor;
        }
    }
}
