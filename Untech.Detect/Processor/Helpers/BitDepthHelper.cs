using OpenCvSharp;

namespace Untech.Detect.Processor.Helpers
{
    public class BitDepthHelper
    {
        public static int BitDepthToInt(BitDepth depth)
        {
            switch (depth)
            {
                case BitDepth.U8:
                case BitDepth.S8:
                    return 8;

                case BitDepth.U16:
                case BitDepth.S16:
                    return 16;

                case BitDepth.S32:
                case BitDepth.F32:
                    return 32;

                case BitDepth.F64:
                    return 64;

                default:
                    return 8;
            }
        }
    }
}