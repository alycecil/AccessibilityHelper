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
            if (!findVerbWindow(program, baseHandle, out var verbWindow))
            {
                program.scan?.DidWork();
                return false;
            }

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
                    
                    
                    
                    didSomething = true;
                }else if (
                    verb.what.Equals(Verb.Sell))
                {
                    Console.WriteLine("Repairing at implied by sell");
                    ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y - 59 * sY), 
                        verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(baseHandle, implied);
                    program.action.wantToRepair = false;
                    didSomething = true;
                }
            }

            if (didSomething)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                program.action.HandleRepairControl(baseHandle);
                    
            }

            program.scan?.DidWork();
            return didSomething;

        }
    }
}