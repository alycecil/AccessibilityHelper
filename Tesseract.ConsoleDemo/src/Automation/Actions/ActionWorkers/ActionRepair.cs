using System;
using System.Drawing;
using System.Threading;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public class ActionRepair : AbstractActionWorker
    {
        public static bool DoAction(Program program, IntPtr baseHandle)
        {
            bool didSomething = false;
           if (!findVerbWindow(program, baseHandle, out var verbWindow)) return false;

            if (!program.stateEngine.InState(StateEngine.OutOfCombat))
            {
                Console.WriteLine("Can't Repair while not out of combat");
                return false;
            }

            foreach (var verb in verbWindow.verbs)
            {
                if (didSomething) break;
                
                if (
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Repairing");
                    VerbWindow.click(baseHandle,verb);
                    program.action.wantToRepair = false;
                    
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    program.action.handleRepairControl(baseHandle);
                    
                    program.action.Repaired();
                    
                    didSomething = true;
                }
            }

            return didSomething;

        }
    }
}