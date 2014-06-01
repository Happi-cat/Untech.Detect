using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Untech.Detect.Processor
{
    public class DetectionStage : IPostProcessingStage
    {
        public IplImage PostProcess(IplImage preProcessedImage, IplImage postProcessedImage)
        {
            using (CvMemStorage storage = new CvMemStorage())
            {
                CvSeq seq = preProcessedImage.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, 30, 40, 15);
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
                    postProcessedImage.Rectangle(cvRect, CvColor.Red, 3, LineType.AntiAlias);
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

                CvSeq<CvCircleSegment> seq1 = preProcessedImage.HoughCircles(storage,
                                                                  HoughCirclesMethod.Gradient, 1,
                    //imgGray.Size.Height / 8, 150, 55, 0, 50);
                                                                  15, 100, 30, 9, 51);

                foreach (CvCircleSegment item in seq1)
                {
                    postProcessedImage.Circle(item.Center, (int)item.Radius, CvColor.Red, 3);
                }
            }
            return postProcessedImage;
        }
    }
}