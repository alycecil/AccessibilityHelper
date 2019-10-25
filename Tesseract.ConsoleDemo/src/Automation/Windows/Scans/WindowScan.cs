using System;
using System.Collections.Generic;
using Tesseract.ConsoleDemo;

namespace runner
{
    public abstract class WindowScan : ITickable
    {
        public List<Thing> things;
        public Program program;

        public WindowScan(List<Thing> things, Program program)
        {
            this.things = things;
            this.program = program;
        }
        
        public abstract void tickCommon(long tick, Program program, IntPtr baseHandle);
    }
}