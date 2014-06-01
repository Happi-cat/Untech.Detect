﻿using System;
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

            using (var windowOriginal = new CvWindow("Original", WindowMode.AutoSize))
            using (var windowPreImage = new CvWindow("Pre-Image", WindowMode.AutoSize))
            using (var windowPostImage = new CvWindow("Post-Image", WindowMode.AutoSize))
            {
                processor.OriginalWindow = windowOriginal;
                processor.PreProcessedWindow = windowPreImage;
                processor.PostProcessedWindow = windowPostImage;
                foreach (var file in files)
                    using (var capture = CvCapture.FromFile(file))
                        processor.Process(capture);
            }
        }      

        private static VideoStreamProcessor BuildProcessor()
        {
            var processor = new VideoStreamProcessor {FrameSkipValue = 2};

            processor.PreProcessingStages.Add(new CloneStage());

            processor.PreProcessingStages.Add(new BGRContrastStage{ LowerPercentage = 0.15, UpperPercentage = 0.25});
            processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
            
            processor.PreProcessingStages.Add(PosterizationStage.BuildColoredLUT(10));
            processor.PreProcessingStages.Add(BlurStage.BuildBlur3());

            //processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
            
            //processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
            //processor.PreProcessingStages.Add(new HistogramContrastStage());
            
            //processor.PreProcessingStages.Add(new GrayContrastStage
            //    {
            //        MaxValue = 255
            //    });
            
            processor.PreProcessingStages.Add(new CannyStage());
            processor.PreProcessingStages.Add(MaskStage.BuildDefaultMask());

            processor.PostProcessingStages.Add(new CloneStage());
            processor.PostProcessingStages.Add(new DetectionStage());

            return processor;
        }
    }
}
