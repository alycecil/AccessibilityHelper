using System;
using System.Linq;
using AutoIt;

namespace runner
{
    public class RepairWindow
    {
        const int baseX = 414, baseY = 276;

        public static void handle(IntPtr baseHandle)
        {
            var repair = Windows.getRepairNothingControl(baseHandle);
            if (repair != IntPtr.Zero)
            {
                var text =
                    //Win32GetText.GetControlText(repair);
                    AutoItX.WinGetText(repair);
                if (text.Contains("You do not have anything that needs to be repaired."))
                {
                    AutoItX.WinClose(repair);
                }
            }

            repair = Windows.getRepair(baseHandle);
            if (repair != IntPtr.Zero)
            {
                ScreenCapturer.GetScale(repair, out float sX, out float sY);
                //click repair all
                MouseManager.MouseClick(baseHandle, "LEFT", (int) (sX * baseX), (int) (sY * baseY));
            }
        }
    }
}