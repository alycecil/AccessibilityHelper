using System.Drawing;

namespace runner
{
    static internal class PixelManager
    {
        public static bool isBlack(Color captureTime)
        {
            return captureTime.R == 0
                   && captureTime.G == 0
                   && captureTime.B == 0;
        }
    }
}