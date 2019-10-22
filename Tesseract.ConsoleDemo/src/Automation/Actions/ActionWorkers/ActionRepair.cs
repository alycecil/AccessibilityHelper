using System;
using System.Drawing;
using System.Threading;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public class ActionRepair : AbstractActionWorker
    {
        public static bool DoAction()
        {
            bool didSomething = false;
           if (!findVerbWindow(out var verbWindow)) return false;

            if (!Program.stateEngine.InState(StateEngine.OutOfCombat))
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
                    VerbWindow.click(verb);
                    Action.wantToRepair = false;
                    
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    Action.handleRepairControl(Windows.HandleBaseWindow());
                    
                    didSomething = true;
                }
            }

            return didSomething;

        }
    }
}