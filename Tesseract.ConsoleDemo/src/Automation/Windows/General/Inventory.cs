using System;
using System.Threading;
using System.Windows.Automation;
using AutoIt;

namespace runner
{
    public class Inventory
    {
        public static bool handle(IntPtr baseHandle)
        {
            
            ToolTips.MoveOver(baseHandle, ExpectedToolTip.Inventory, true);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            var hWnd = Windows.HandleInventory(baseHandle);

//            if (hWnd != IntPtr.Zero)
//            {
//                AutoItX.WinClose(hWnd);
//            }

            return true;
        }
    }
}