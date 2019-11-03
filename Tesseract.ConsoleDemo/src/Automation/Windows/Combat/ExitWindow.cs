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
            MouseManager.MouseClick(hCntr,startBtnX, startBtnY);
            return true;
        }
    }
}