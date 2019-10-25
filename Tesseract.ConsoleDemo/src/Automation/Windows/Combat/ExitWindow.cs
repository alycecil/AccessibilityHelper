using System;
using System.Drawing;
using System.Drawing.Imaging;
using AutoIt;

namespace runner
{
    public class ExitWindow
    {
        const int startBtnY = 30;
        const int endBtnY = 60;
        const int startBtnX = 35;
        const int endBtnX = 64;

        public static bool Click(IntPtr hWdw, IntPtr hCntr)
        {
            if(hCntr==IntPtr.Zero) return false;
            ScreenCapturer.GetScale(hCntr, out var sX, out var sY);
            ScreenCapturer.GetBounds(hCntr, out Rectangle rectangle);

            rectangle.Y += (int)(startBtnY * sY);
            rectangle.X += (int)(startBtnX * sX);
            rectangle.Width = (int) ((endBtnX - startBtnX) * sX);
            rectangle.Height= (int) ((endBtnY - startBtnY) * sY);
            
            //ScreenCapturer.ImageSave("RExit", ImageFormat.Tiff, ScreenCapturer.Capture(rectangle));
            //AutoItX.ControlClick(hWdw, hCntr, "left", 1, startBtnX, startBtnY);

            MouseManager.MouseClick(hCntr,"LEFT", rectangle.X, rectangle.Y);
            
            

            return true;
        }
    }
}