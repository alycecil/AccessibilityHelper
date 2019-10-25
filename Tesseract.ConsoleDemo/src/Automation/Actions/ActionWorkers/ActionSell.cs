using System;
using System.Drawing;
using System.Threading;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public class ActionSell : AbstractActionWorker
    {
        public static bool DoAction(Program program, IntPtr baseHandle)
        {
            bool didSomething = false;
            if (!findVerbWindow(program, baseHandle,out var verbWindow)) return false;

            if (!program.stateEngine.InState(StateEngine.OutOfCombat))
            {
                Console.WriteLine("Can't Repair while not out of combat");
                return false;
            }

            foreach (var verb in verbWindow.verbs)
            {
                if (didSomething) break;

                if (
                    verb.what.Equals(Verb.Sell))
                {
                    Console.WriteLine("selling");
                    VerbWindow.click(baseHandle,verb);
                    program.action.wantToRepair = false;
                    didSomething = true;
                }
                else if (
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Selling at implied Button from repair");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y + 60 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(baseHandle, implied);
                    program.action.wantToRepair = true;
                    didSomething = true;
                }
                else if (
                    verb.what.Equals(Verb.Talk))
                {
                    Console.WriteLine("Selling at implied Button from Talk");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y - 15 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(baseHandle,implied);
                    program.action.wantToRepair = true;
                    didSomething = true;
                }
            }

            if (didSomething)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                program.lastVerbWindow = null;
                program.windowScanManager.flushScreenScan();
                program.action.doSell(baseHandle);
                
            }
            
            return false;
        }
    }
}