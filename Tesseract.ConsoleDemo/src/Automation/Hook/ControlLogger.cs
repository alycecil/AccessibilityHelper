using System;
using System.Collections.Generic;
using System.Windows.Automation;
using Tesseract.ConsoleDemo;

namespace runner
{
    abstract class ControlLogger
    {
        internal Program program;

        protected ControlLogger(Program program)
        {
            this.program = program;
        }

        public abstract string LogRoom();
        
        public static ControlLogger build(Program program, IntPtr intPtr)
        {
            var ae = AutomationElement.FromHandle(intPtr);
            Condition condition = new PropertyCondition(AutomationElement.ClassNameProperty, "RichEdit20A");
            AutomationElementCollection all = ae.FindAll(TreeScope.Subtree, condition);
            List<ControlLogger> loggers = new List<ControlLogger>();
            foreach (AutomationElement richText in all)
            {
                loggers.Add(new SingleControlLogger(program, intPtr, (IntPtr) richText.Current.NativeWindowHandle));
            }
            
            return new BundledControlLogger(program, loggers);
        }
    }
}