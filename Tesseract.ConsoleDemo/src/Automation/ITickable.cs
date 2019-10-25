using System;
using Tesseract.ConsoleDemo;

namespace runner
{
    public interface ITickable
    {
        void tickCommon(long tick, Program program, IntPtr baseHandle);
    }
}