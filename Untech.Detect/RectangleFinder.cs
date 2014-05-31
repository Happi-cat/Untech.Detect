using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Untech.Detect
{
    public enum IntersectType
    {
        Type2,
        Type1,
        Type0,
        None,
        TypeMinus1, // almost parallel lines from the same line group
    }

    public class LineGroup
    {
        public List<CvLineSegmentPoint> Lines = new List<CvLineSegmentPoint>();

    }

    public class RectangleFinder
    {
        public static void Filter(List<CvRect> rects)
        {
            for (int i = 0; i < rects.Count;)
            {
                var rect = rects[i];
                if (rect.Width > rect.Height*7 || rect.Height > rect.Width*7)
                {
                    rects.RemoveAt(i);
                }
                else if (rect.Width < 20 || rect.Height < 20)
                {
                    rects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            } 
        }

        public static List<CvRect> Convert(List<LineGroup> groups)
        {
            var rects = new List<CvRect>();
            
            foreach (var lineGroup in groups)
            {
                int xMax = 0, yMax = 0, xMin = int.MaxValue, yMin = int.MaxValue;

                foreach (var line in lineGroup.Lines)
                {
                    xMax = Math.Max(Math.Max(line.P1.X, line.P2.X), xMax);
                    xMin = Math.Min(Math.Min(line.P1.X, line.P2.X), xMin);
                    yMax = Math.Max(Math.Max(line.P1.Y, line.P2.Y), yMax);
                    yMin = Math.Min(Math.Min(line.P1.Y, line.P2.Y), yMin);
                }

                rects.Add(new CvRect(xMin, yMin, xMax-xMin, yMax-yMin));
            }

            return rects;
        }

         
        public static List<LineGroup> GroupSegments(List<CvLineSegmentPoint> lines)
        {
            var groups = new List<LineGroup>();

            while (lines.Count > 0)
            {
                var group = new LineGroup();
                group.Lines.Add(lines[0]);
                lines.RemoveAt(0);

                for (int groupLineIdx = 0; groupLineIdx < group.Lines.Count; groupLineIdx++)
                {
                    for (int lineIdx = 0; lineIdx < lines.Count; lineIdx++)
                    {
                        var intersectType = ValidIntersect(lines[lineIdx], group.Lines[groupLineIdx]);
                        if ( intersectType == IntersectType.Type2 || intersectType == IntersectType.Type1 )
                        {
                            group.Lines.Add(lines[lineIdx]);
                            lines.RemoveAt(lineIdx);
                        }
                        else
                        {
                            lineIdx++;
                        }
                    }
                }

                groups.Add(group);
            }

            return groups;
        }


        public static IntersectType ValidIntersect(CvLineSegmentPoint a, CvLineSegmentPoint b)
        {
            const int dI = 7;

            var intersect = a.LineIntersection(b);

            if (intersect == null) return IntersectType.None;

            bool aFlag = false;
            bool bFlag = false;
            var i = intersect.Value;

            if (((a.P1.X - dI) < i.X && (a.P1.X + dI) > i.X &&
                 (a.P1.Y - dI) < i.Y && (a.P1.Y + dI) > i.Y) ||
                ((a.P2.X - dI) < i.X && (a.P2.X + dI) > i.X &&
                 (a.P2.Y - dI) < i.Y && (a.P2.Y + dI) > i.Y))
            {
                aFlag = true;
            }
            if (((b.P1.X - dI) < i.X && (b.P1.X + dI) > i.X &&
                 (b.P1.Y - dI) < i.Y && (b.P1.Y + dI) > i.Y) ||
                ((b.P2.X - dI) < i.X && (b.P2.X + dI) > i.X &&
                 (b.P2.Y - dI) < i.Y && (b.P2.Y + dI) > i.Y))
            {
                bFlag = true;
            }

            if ( aFlag &&  bFlag) return IntersectType.Type2;
            if (aFlag)
            {
                if (b.P1.X < i.X && b.P2.X > i.X &&
                    b.P1.Y < i.Y && b.P2.Y > i.Y)
                {
                    return IntersectType.Type1;
                }
            } else if (bFlag)
            {
                if (a.P1.X < i.X && a.P2.X > i.X &&
                    a.P1.Y < i.Y && a.P2.Y > i.Y)
                {
                    return IntersectType.Type1;
                }
            }
            //var vec = new CvLineSegmentPoint(CvPoint.Empty, i);
            //if (a.IntersectedSegments(vec)) return IntersectType.Type0;
            return IntersectType.None;
        }
    }
}