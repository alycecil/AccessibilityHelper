using System;
using System.Drawing;
using AutoIt;
using static runner.MouseButton;

namespace runner
{
    internal static class MouseManager
    {
        public static bool MouseMoveUnScaled(IntPtr baseHandle, int x, int y, int speed = 1)
        {
            MouseManagerHelper.Scale(baseHandle, x, y, out var scaledX, out var scaledY);
            MouseManagerHelper.Offset(baseHandle, scaledX, scaledY,out int offsetX, out int offsetY);
            
            return MouseMoveAbsolute(baseHandle, offsetX, offsetY, speed);
        }

        public static bool MouseMoveAbsolute(IntPtr baseHandle, int x = -2147483647, int y = -2147483647, int speed = 1)
        {
            return AutoItX.MouseMove(x, y, speed) != 0;
        }

        public static void MouseClick(IntPtr baseHandle,
            int x, int y,
            MouseButton button = LEFT, int clicks = 1, int speed = 1)
        {
            MouseManagerHelper.Scale(baseHandle, x, y, out var scaledX, out var scaledY);
            MouseManagerHelper.Offset(baseHandle, scaledX, scaledY,out int offsetX, out int offsetY);


            MouseClickAbsolute(baseHandle, button, offsetX, offsetY, clicks, speed);
        }

        public static void MouseClickAbsolute(IntPtr baseHandle, MouseButton button = LEFT, int x = -2147483647,
            int y = -2147483647, int clicks = 1, int speed = 1)
        {
            if (x >= 0 && y >= 0)
            {
                MouseMoveAbsolute(baseHandle, x, y);
            }

            var buttonTxt = MouseManagerHelper.GetButtonTxt(button);

            AutoItX.MouseClick(buttonTxt, x, y, clicks, speed);
        }
    }
}