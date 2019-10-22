using System;

namespace runner.ActionWorkers
{
    public abstract class AbstractActionWorker
    {
        internal static bool findVerbWindow(out VerbWindow verbWindow)
        {
            verbWindow = VerbWindow.last;

            if (verbWindow == null)
            {
                //Console.Write("[...]");
                return false;
            }

            if (verbWindow.verbs.Count == 0)
            {
                Console.WriteLine("Scrap Scan Sucked, Trying Again");
                int type = verbWindow.type;
                verbWindow = VerbWindow.findWindow(Windows.HandleBaseWindow(), verbWindow.ocrText, false, false);
                if (verbWindow != null)
                    verbWindow.type = type;

                Console.WriteLine("Actions Scanned Again");

                if (verbWindow == null || verbWindow.verbs.Count == 0)
                {
                    Console.WriteLine("Nothing To Do Yet, Maybe I should screen scan better");
                    return false;
                }
            }


            if (!Win32.IsWindowVisible(verbWindow.hWnd) || verbWindow.verbs.Count == 0)
            {
                Console.WriteLine("Lost VerbWindow");
                VerbWindow.last = null;
                return false;
            }

            return true;
        }
    }
}