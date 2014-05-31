using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenCvSharp;

namespace Untech.Detect
{
    class Program
    {
        public delegate void DetectObjectsDelegate(
            IplImage frame, IplImage mask, CvWindow windowGray, CvWindow windowCanny, CvWindow windowOriginal);



        public static void Test()
        {
            var line1 = new CvLineSegmentPoint(new CvPoint(10, 10), new CvPoint(45, 10));
            var line2 = new CvLineSegmentPoint(new CvPoint(6, 14), new CvPoint(10, 45));

            Console.WriteLine("Test #1 {0}", RectangleFinder.ValidIntersect(line1, line2));

            line2 = new CvLineSegmentPoint(new CvPoint(6, 24), new CvPoint(10, 45));

            Console.WriteLine("Test #2 {0}", RectangleFinder.ValidIntersect(line1, line2));

            line2 = new CvLineSegmentPoint(new CvPoint(25, 2), new CvPoint(10, 45));

            Console.WriteLine("Test #3 {0}", RectangleFinder.ValidIntersect(line1, line2));

            line2 = new CvLineSegmentPoint(new CvPoint(25, 5), new CvPoint(10, 45));

            Console.WriteLine("Test #4 {0}", RectangleFinder.ValidIntersect(line1, line2));
        }

        private static void Main(string[] args)
        {
            
            IplImage mask = new IplImage(1920, 1080, BitDepth.U8, 1);
            mask.Zero();
            mask.FillConvexPoly(new[]
                {
                    new CvPoint(0,0), 
                    new CvPoint(600,0), 
                    new CvPoint(960, 500), 
                    new CvPoint(0, 900), 
                }, CvColor.White, LineType.Link4);

            mask.FillConvexPoly(new[]
                {
                    new CvPoint(960,500), 
                    new CvPoint(1200,0), 
                    new CvPoint(1920, 0), 
                    new CvPoint(1920, 900), 
                }, CvColor.White, LineType.Link4);
            mask.FillConvexPoly(new[]
                {
                    new CvPoint(0,200), 
                    new CvPoint(1920,200), 
                    new CvPoint(1920, 600), 
                    new CvPoint(0, 600), 
                }, CvColor.White, LineType.Link4);



            int counter = 0;
            var files = Directory.GetFiles(args[0], "*.mp4");
            foreach (var file in files)
            {
                using (var capture = CvCapture.FromFile(file))
                using (var windowOriginal = new CvWindow("Original", WindowMode.AutoSize))
                using (var windowGray = new CvWindow("Gray", WindowMode.AutoSize))
                using (var windowCanny = new CvWindow("Canny", WindowMode.AutoSize))
                {
                    while (true)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        counter++;
                        var frame = capture.QueryFrame();
                        if (frame == null)
                            break;

                        if (counter > 2)
                        {
                            var method = new DetectObjectsDelegate(DetectObjects);
                            method.BeginInvoke(frame.Clone(), mask, windowGray, windowCanny, windowOriginal, null, null);
                            counter = 0;
                        }

                        var key = CvWindow.WaitKey(33);
                        if (key == 27)
                            break;

                        stopwatch.Stop();
                        Console.WriteLine(stopwatch.Elapsed);
                    }
                }
            }
        }

        public static void DetectObjects(IplImage frame, IplImage mask, CvWindow windowGray, CvWindow windowCanny, CvWindow windowOriginal)
        {
            using (var imgFrame = new IplImage(frame.Size, frame.Depth, frame.NChannels))
            using (var imgSat = new IplImage(frame.Size, BitDepth.U8, 1))
            using (var imgValue = new IplImage(frame.Size, BitDepth.U8, 1))
            using (var imgCanny = new IplImage(frame.Size, BitDepth.U8, 1))
            using (var imgCanny2 = new IplImage(frame.Size, BitDepth.U8, 1))
            using (var imgHough = frame)
            {
                Cv.CvtColor(frame, imgFrame, ColorConversion.BgrToHsv_Full);
                Cv.Split(imgFrame, null, imgValue, null,  null); // get saturation
                
                //Cv.CvtColor(frame, imgValue, ColorConversion.BgrToGray);
                
                Cv.Smooth(imgValue, imgValue, SmoothType.Blur, 5);
                
                windowGray.ShowImage(imgValue);

                Cv.Canny(imgValue, imgCanny, 90, 130, ApertureSize.Size3);
                imgCanny.Copy(imgCanny2, mask);

                windowCanny.ShowImage(imgCanny2);

                using (CvMemStorage storage = new CvMemStorage())
                {
                    CvSeq seq = imgCanny2.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, 30, 40, 15);
                    var lines = new List<CvLineSegmentPoint>();
                    for (int i = 0; i < seq.Total; i++)
                    {
                        var cvLineSegmentPoint = seq.GetSeqElem<CvLineSegmentPoint>(i);
                        if (cvLineSegmentPoint != null)
                            lines.Add(cvLineSegmentPoint.Value);
                    }

                    var groupedLines = RectangleFinder.GroupSegments(lines);

                    var rects = RectangleFinder.Convert(groupedLines);
                    RectangleFinder.Filter(rects);

                    foreach (var cvRect in rects)
                    {
                        imgHough.Rectangle(cvRect, CvColor.Red, 3, LineType.AntiAlias);
                    }

                    //for (int i = 0; i < groupedLines.Count; i++)
                    //{
                    //    var color = new CvColor(i*255/max,i*255/max,i*255/max);
                    //    var group = groupedLines[i];
                    //    for (int j = 0; j < group.Lines.Count; j++)
                    //    {
                    //        CvLineSegmentPoint elem = group.Lines[j];
                    //        imgHough.Line(elem.P1, elem.P2, color, 3, LineType.AntiAlias, 0);
                    //    }
                        
                    //}


                    //Console.WriteLine(groupedLines.Count);

                    CvSeq<CvCircleSegment> seq1 = imgValue.HoughCircles(storage,
                                                                      HoughCirclesMethod.Gradient, 1,
                        //imgGray.Size.Height / 8, 150, 55, 0, 50);
                                                                      15, 100, 30, 9, 51);
                    
                    foreach (CvCircleSegment item in seq1)
                    {
                        imgHough.Circle(item.Center, (int)item.Radius, CvColor.Red, 3);
                    }
                }

                windowOriginal.ShowImage(imgHough);
            }

            frame.Dispose();
        }

       
    }
}
