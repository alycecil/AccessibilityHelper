using System.Runtime.InteropServices;

namespace runner
{
    public static partial class ScreenCapturer
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}