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
            
            ToolTips.moveOver(ExpectedTT.Inventory);
            Thread.Sleep(1);
            AutoItX.MouseClick();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var hWnd = Windows.HandleInventory();

            if (hWnd != IntPtr.Zero)
            {
                AutoItX.WinClose(hWnd);
            }

            return true;
        }
    }
}