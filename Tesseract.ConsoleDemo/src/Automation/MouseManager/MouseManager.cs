using System;
using System.Threading;
using AutoIt;

namespace runner
{
    public class MouseManager
    {
        public static bool MouseMoveUnScaled(IntPtr baseHandle, int x, int y, int speed = 1)
        {
            ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);
            //todo rect from bounds adds
            return MouseMoveAbsolute(baseHandle, (int) (x*sX), (int) (y*sY), speed);
        }
        public static bool MouseMoveAbsolute(IntPtr baseHandle, int x=-2147483647, int y = -2147483647, int speed = 1)
        {
            return AutoItX.MouseMove(x, y,  speed)!=0;
        }

        public static void MouseClick(IntPtr baseHandle, string button="LEFT", int x=-2147483647, int y = -2147483647,  int clicks = 1 , int speed = 1)
        {
            if (x >= 0 && y >= 0)
            {
                MouseMoveAbsolute(baseHandle, x, y);
//                Thread.Sleep(TimeSpan.FromSeconds(10));
//                Console.WriteLine("Clicking!");
            }

            AutoItX.MouseClick(button, x, y, clicks, speed);
        }
    }
}