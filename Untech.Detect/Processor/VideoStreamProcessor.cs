using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class VideoStreamProcessor
    {
        public delegate void ProcessIplImage(IplImage image);

        public VideoStreamProcessor()
        {
            PreProcessingStages = new List<IPreProcessingStage>();
            PostProcessingStages = new List<IPostProcessingStage>();
        }
        public List<IPreProcessingStage> PreProcessingStages { get; private set; }
        public List<IPostProcessingStage> PostProcessingStages { get; private set; }
        public CvWindow OriginalWindow { get; set; }
        public CvWindow PreProcessedWindow { get; set; }
        public CvWindow PostProcessedWindow { get; set; }
        public int FrameSkipValue { get; set; }

        private int FrameCounter { get; set; }

        public void Process(IplImage image)
        {
            OriginalWindow.ShowImage(image);

            var preImage = image;
            foreach (var stage in PreProcessingStages)
            {
                var oldPreImage = preImage;
                preImage = stage.PreProcess(preImage);
                if (oldPreImage != preImage && oldPreImage != image)
                {
                    oldPreImage.Dispose();
                }
            }

            PreProcessedWindow.ShowImage(preImage);
            
            var postImage = image;
            foreach (var stage in PostProcessingStages)
            {
                var oldPostImage = postImage;
                postImage = stage.PostProcess(preImage, postImage);
                if (postImage != oldPostImage)
                {
                    oldPostImage.Dispose();
                }
            }

            PostProcessedWindow.ShowImage(postImage);
            preImage.Dispose();
            postImage.Dispose();
        }

        public void Process(CvCapture capture)
        {
            FrameCounter = 0;
            while (true)
            {
                FrameCounter++;
                var frame = capture.QueryFrame();
                if (frame == null)
                    break;

                if (FrameCounter > FrameSkipValue)
                {
                    var method = new ProcessIplImage(Process);
                    method.BeginInvoke(frame, null, null);
                    FrameCounter = 0;
                }

                var key = CvWindow.WaitKey(33);
                if (key == 27)
                    break;
            }
        }
    }
}