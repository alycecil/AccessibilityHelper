using System;
using AutoIt;

namespace runner
{
    public enum MouseButton
    {
        LEFT,
        RIGHT,
        MIDDLE
    }
    public class MouseManager
    {
        public static bool MouseMove(IntPtr baseHandle, int x=Int32.MinValue, int y=Int32.MinValue, int speed = 1)
        {
            return AutoItX.MouseMove(x, y,  speed)!=0;
        }

        public static void MouseClick(IntPtr baseHandle, string button="LEFT", int x=-2147483647, int y = -2147483647,  int clicks = 1 , int speed = 1)
        {
            AutoItX.MouseClick(button, x, y, clicks, speed);
        }
    }
}