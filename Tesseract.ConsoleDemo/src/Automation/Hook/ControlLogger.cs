using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace runner
{
    abstract class ControlLogger
    {
        public abstract string LogRoom();
        
        public static ControlLogger build(IntPtr intPtr)
        {
            var ae = AutomationElement.FromHandle(intPtr);
            Condition condition = new PropertyCondition(AutomationElement.ClassNameProperty, "RichEdit20A");
            AutomationElementCollection all = ae.FindAll(TreeScope.Subtree, condition);
            List<ControlLogger> loggers = new List<ControlLogger>();
            foreach (AutomationElement richText in all)
            {
                loggers.Add(new SingleControlLogger(intPtr, (IntPtr) richText.Current.NativeWindowHandle));
            }
            
            return new BundledControlLogger(loggers);
        }
    }
}