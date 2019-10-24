using System;
using System.Drawing;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public class ActionIdle : AbstractActionWorker
    {
        public static bool DoIdle()
        {
            bool didSomething = false;
            if (!findVerbWindow(out var verbWindow)) return false;

            if (!Program.stateEngine.InState(StateEngine.OutOfCombat))
            {
                Console.WriteLine("Not Lazing About, stopping doIdle");
                return false;
            }

            foreach (var verb in verbWindow.verbs)
            {
                if (didSomething) break;
                //Console.WriteLine("Idle Considering doing[{0}]", verb.what);

                var hpValue = Program.ego?.Hp?.Value;
                var maxHp = Program.MaxHp;
                var weight = Program.ego?.Weight?.Value;

                if (hpValue == null)
                {
                    hpValue = maxHp;
                }

                if (weight == null)
                {
                    weight = 60;
                }

                if (hpValue > 20 &&
                    verb.what.Equals(Verb.Steal))
                {
                    Console.WriteLine("Stealing!!!!");
                    VerbWindow.click(verb);
                    didSomething = true;
                }
                else if (
                    hpValue > 20 &&
                    weight < 90 &&
                    verb.what.Equals(Verb.Fight) 
                    //&& verbWindow.ocrText?.TrimEnd()?.EndsWith("hp", StringComparison.OrdinalIgnoreCase) == true
                )
                {
                    Console.WriteLine("Starting A fight");
                    VerbWindow.click(verb);
                    didSomething = true;
                }
                else if (
                    Action.wantToRepair &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Repairing");
                    VerbWindow.click(verb);
                    Action.wantToRepair = false;
                    didSomething = true;
                }
                else if (
                    weight > 55 &&
                    verb.what.Equals(Verb.Sell))
                {
                    Console.WriteLine("Selling");
                    VerbWindow.click(verb);
                    Action.wantToRepair = true;
                    didSomething = true;
                }
                else if (
                    weight > 55 &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Selling at implied Button");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y + 60 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(implied);
                    Action.wantToRepair = true;
                    didSomething = true;
                }
                else if (
                    weight > 75 &&
                    verb.what.Equals(Verb.Talk))
                {
                    Console.WriteLine("Selling at implied Button from Talk");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y - 15 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(implied);
                    Action.wantToRepair = true;
                    didSomething = true;
                }
                else
                {
                    //Console.WriteLine("-- Nothing doing.");
                }
            }

            if (!didSomething)
            {
                Console.Write("Idle Considered doing [");
                foreach (var verb in verbWindow.verbs)
                    Console.Write("Verb[{0}],", verb.what);

                Console.WriteLine("].");
            }
            else
            {
                VerbWindow.last = null;
                WindowScan.flushScreenScan();
            }


            return didSomething;
        }
    }
}