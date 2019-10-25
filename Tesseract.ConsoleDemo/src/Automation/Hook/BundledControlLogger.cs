using System;
using System.Collections.Generic;
using Tesseract.ConsoleDemo;

namespace runner
{
    class BundledControlLogger : ControlLogger
    {
        public BundledControlLogger(Program program, List<ControlLogger> controls) : base(program)
        {
            this.controls = controls;
        }

        private List<ControlLogger> controls;
        public override string LogRoom()
        {
            foreach (ControlLogger controlLogger in controls)
            {
                controlLogger.LogRoom();
            }

            return String.Empty;
        }
    }
}