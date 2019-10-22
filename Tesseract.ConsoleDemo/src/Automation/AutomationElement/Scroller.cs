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
            if (targetControl == null)
            {
                throw new ArgumentNullException(
                    "AutomationElement argument cannot be null.");
            }

            ScrollPattern scrollPattern = GetScrollPattern(targetControl);

            if (scrollPattern == null)
            {
//                Win32.SendMessage((IntPtr) targetControl.Current.NativeWindowHandle, 
//                    WM
//                    Win32.ScrollBarCommands.SB_LINEDOWN,
//                    Win32.ScrollBarCommands.SB_LINEDOWN);
//                
                IntPtr hWnd = (IntPtr) targetControl.Current.NativeWindowHandle;
                Win32.GetScrollRange(hWnd,
                    0, out int lpMinPos,
                    out int lpMaxPos);
                if (lpMaxPos > 1)
                {
                    
                    int pos = Win32.GetScrollPos(hWnd,
                        0);

                        Win32.SetScrollPos(hWnd, 0, pos + 20, true);
                        
                    
                    int pos2 = Win32.GetScrollPos(hWnd,
                        0);
                    if(pos != pos2)
                        return true;
                    
                    

                }
                
                return false;
            }

            try
            {
                scrollPattern.Scroll(hScrollAmount, vScrollAmount);
                return true;
            }
            catch (InvalidOperationException)
            {
                // Control not able to scroll in the direction requested;
                // when scrollable property of that direction is False
                // TO DO: error handling.
                return false;
            }
            catch (ArgumentException)
            {
                // If a control supports SmallIncrement values exclusively 
                // for horizontal or vertical scrolling but a LargeIncrement 
                // value (NaN if not supported) is passed in.
                // TO DO: error handling.
                return false;
            }
            catch (Exception)
            {
                // If a control supports SmallIncrement values exclusively 
                // for horizontal or vertical scrolling but a LargeIncrement 
                // value (NaN if not supported) is passed in.
                // TO DO: error handling.
                return false;
            }
        }
    }
}