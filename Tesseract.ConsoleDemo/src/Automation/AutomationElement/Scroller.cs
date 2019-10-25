using System;
using System.Windows.Automation;


//This doesnt work yet we will keep trying
namespace runner
{
    public static class Scroller
    {
        public static ScrollPattern GetScrollPattern(
            System.Windows.Automation.AutomationElement targetControl)
        {
            ScrollPattern scrollPattern = null;

            try
            {
                scrollPattern =
                    targetControl.GetCurrentPattern(
                            ScrollPattern.Pattern)
                        as ScrollPattern;
            }
            // Object doesn't support the ScrollPattern control pattern
            catch (InvalidOperationException)
            {
                return null;
            }

            return scrollPattern;
        }
        public static bool ScrollElement(
            System.Windows.Automation.AutomationElement targetControl,
            ScrollAmount hScrollAmount,
            ScrollAmount vScrollAmount)
        {
           
                
                return false;
            
        }
    }
}