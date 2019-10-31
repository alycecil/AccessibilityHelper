using System;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public abstract class AbstractActionWorker
    {
        internal static bool findVerbWindow(Program program, IntPtr baseHandle, out VerbWindow verbWindow)
        {
            verbWindow = program.lastVerbWindow;

            if (verbWindow == null)
            {
                Console.Write("[...]");
                return false;
            }

            if (verbWindow.verbs.Count == 0)
            {
                Console.WriteLine("Scrap Scan Sucked, Trying Again");
                int type = verbWindow.type;
                verbWindow = VerbWindow.findWindow(program, baseHandle, verbWindow.ocrText, false, false);
                if (verbWindow != null)
                    verbWindow.type = type;
                program.lastVerbWindow = verbWindow;
                Console.WriteLine("Actions Scanned Again");

                if (verbWindow == null || verbWindow.verbs.Count == 0)
                {
                    Console.WriteLine("Nothing To Do Yet, Maybe I should screen scan better");
                    program.lastVerbWindow = null;
                    return false;
                }
            }


            if (!Win32.IsWindowVisible(verbWindow.hWnd) || verbWindow.verbs.Count == 0)
            {
                Console.WriteLine("Lost VerbWindow");
                program.lastVerbWindow = null;
                return false;
            }

            return true;
        }
    }
}