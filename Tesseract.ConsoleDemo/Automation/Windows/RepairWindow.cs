using System;
using System.Linq;
using AutoIt;

namespace runner
{
    public class RepairWindow
    {
        const int baseX = 414, baseY = 276;

        public static void handle(IntPtr basehandle)
        {
            var repair = Windows.getRepairNothingControl(basehandle);
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

            repair = Windows.getRepair(basehandle);
            if (repair != IntPtr.Zero)
            {
                ScreenCapturer.GetScale(repair, out float sX, out float sY);
                //click repair all
                AutoItX.MouseClick("LEFT", (int) (sX * baseX), (int) (sY * baseY));
            }
        }
    }
}