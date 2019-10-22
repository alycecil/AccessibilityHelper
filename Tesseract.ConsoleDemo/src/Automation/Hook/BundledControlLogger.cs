using System;
using System.Collections.Generic;

namespace runner
{
    class BundledControlLogger : ControlLogger
    {
        public BundledControlLogger(List<ControlLogger> controls)
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