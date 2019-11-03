using System;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public abstract class AbstractActionWorker
    {
        internal static bool findVerbWindow(Program program, IntPtr baseHandle, out VerbWindow verbWindow)
        {
            try
            {
                verbWindow = program.lastVerbWindow;

                if (verbWindow == null)
                {
                    //Console.Write("[No Verb Window]");
                    return false;
                }

                if (verbWindow.verbs.Count == 0)
                {
                    Console.WriteLine("Scrap Scan Sucked, Trying Again");
                    int type = verbWindow.type;
                    verbWindow = VerbWindow.FindWindow(program, baseHandle, verbWindow.ocrText, false, false);
                    if (verbWindow != null)
                        verbWindow.type = type;
                    program.lastVerbWindow = verbWindow;
                    Console.WriteLine("Actions Scanned Again");

                    if (verbWindow?.verbs.Count == 0)
                    {
                        Console.WriteLine("Nothing To Do Yet, Maybe I should screen scan better");
                        program.FinishVerbWindow();
                        return false;
                    }
                }


                if (Win32.IsWindowVisible(verbWindow.hWnd) && verbWindow.verbs.Count != 0) return true;
                Console.WriteLine("[Lost VerbWindow]");
                program.FinishVerbWindow();
                return false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Had An issue with find window [{0}]",e);
                verbWindow = null;
                return false;
            }

        }
    }
}