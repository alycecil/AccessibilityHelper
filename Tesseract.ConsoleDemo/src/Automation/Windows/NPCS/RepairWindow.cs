using System;
using System.Linq;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class RepairWindow
    {
        const int baseX = 414, baseY = 276;

        public static void handle(IntPtr baseHandle, Program program)
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
                    program.action.Repaired();
                }
            }

            repair = Windows.getRepair(baseHandle);
            if (repair != IntPtr.Zero)
            {
                //click repair all
                MouseManager.MouseMoveUnScaled(baseHandle, baseX, baseY);
                MouseManager.MouseClick(baseHandle, "LEFT");
                program.action.Repaired();
            }
        }
    }
}