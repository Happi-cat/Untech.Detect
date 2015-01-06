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

            using (var windowOriginal = new CvWindow("Original", WindowMode.StretchImage))
            using (var windowPreImage = new CvWindow("Pre-Image", WindowMode.StretchImage))
            using (var windowPostImage = new CvWindow("Post-Image", WindowMode.StretchImage))
            {
				windowOriginal.Resize(820, 640);
				windowPostImage.Resize(820, 640);
				windowPreImage.Resize(820, 640);

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
            
			//// Case 1
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());


			//// Case 2
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new HistogramContrastStage());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

			//// Case 3
			//processor.PreProcessingStages.Add(new BGRContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PostProcessingStages.Add(new CloneStage());

			//// Case 4
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PostProcessingStages.Add(new CloneStage());

			//// Case 5
			//processor.PreProcessingStages.Add(new BGRContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PostProcessingStages.Add(new CloneStage());

			//// Case 6
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new HistogramContrastStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 7
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildSaturationExtractor());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 8
			//processor.PreProcessingStages.Add(new BGRContrastStage{ LowerPercentage = 0.15, UpperPercentage = 0.25});
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildSaturationExtractor());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 9
			//processor.PreProcessingStages.Add(new BGRContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 10
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 11
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 12
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 13
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(8));
			//processor.PostProcessingStages.Add(new CloneStage());


			////// Case 14
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(6));
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 15
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(8));
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 16
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 17
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 18
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 19
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 20
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 21
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(8));
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 22
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 23
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());


			////// Case 24
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());

			////// Case 25
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

			////// Case 26
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

			////// Case 27
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

			////// Case 28
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			//processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			//processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(8));
			//processor.PreProcessingStages.Add(new CannyStage());
			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

			//// Case 29
			processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(8));
			processor.PreProcessingStages.Add(new CannyStage());
			processor.PostProcessingStages.Add(new CloneStage());
			processor.PostProcessingStages.Add(new DetectionStage());

			////processor.PreProcessingStages.Add(new BGRContrastStage{ LowerPercentage = 0.15, UpperPercentage = 0.25});
			//processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			//processor.PreProcessingStages.Add(new HistogramContrastStage());
			////processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });
			////processor.PreProcessingStages.Add(SplitChannelsStage.BuildSaturationExtractor());
			////processor.PreProcessingStages.Add(new GrayContrastStage { LowerPercentage = 0.15, UpperPercentage = 0.25 });
			////processor.PreProcessingStages.Add(PosterizationStage.BuildDefaultLUT(32));
			////processor.PreProcessingStages.Add(BlurStage.BuildBlur3());
			////processor.PreProcessingStages.Add(new BlurStage { Param1 = 9, SmoothType = SmoothType.Gaussian });

            
            
			////processor.PreProcessingStages.Add(SplitChannelsStage.BuildValueExtractor());
			////processor.PreProcessingStages.Add(new HistogramContrastStage());
            
			////processor.PreProcessingStages.Add(new GrayContrastStage
			////    {
			////        MaxValue = 255
			////    });
            
			//processor.PreProcessingStages.Add(new CannyStage());
			////processor.PreProcessingStages.Add(MaskStage.BuildDefaultMask());

			//processor.PostProcessingStages.Add(new CloneStage());
			//processor.PostProcessingStages.Add(new DetectionStage());

            return processor;
        }
    }
}
