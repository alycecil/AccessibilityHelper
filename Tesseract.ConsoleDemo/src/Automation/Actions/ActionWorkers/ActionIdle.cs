using System;
using System.Drawing;
using Tesseract.ConsoleDemo;

namespace runner.ActionWorkers
{
    public class ActionIdle : AbstractActionWorker
    {
        
        static bool enableTheft = false;
        
        public static bool DoIdle(Program program, IntPtr baseHandle)
        {
            bool didSomething = false;

            if (!findVerbWindow(program, baseHandle, out var verbWindow))
            {
                //Console.WriteLine("-Passive");
                program.scan?.DidWork();
                return false;
            }
            else Console.Write("-Do Idle~");

            if (!program.stateEngine.InState(StateEngine.OutOfCombat))
            {
                Console.WriteLine("Not Lazing About, stopping doIdle");
                return false;
            }

            foreach (var verb in verbWindow.verbs)
            {
                if (didSomething) break;
                Console.Write("Consider [{0}],", verb.what);

                var hpValue = program.ego?.Hp?.Value;
                var maxHp = program.MaxHp;
                var weight = program.ego?.Weight?.Value;

                if (hpValue == null)
                {
                    hpValue = maxHp;
                }

                if (weight == null)
                {
                    weight = 60;
                }

                if (hpValue > 20 &&
                    verb.what.Equals(Verb.Steal) && 
                    enableTheft)
                {
                    Console.WriteLine("Stealing!!!!");
                    VerbWindow.Click(baseHandle, verb);
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
                    VerbWindow.Click(baseHandle, verb);
                    didSomething = true;
                }
                else if (
                    program.action.wantToRepair &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Repairing");
                    VerbWindow.Click(baseHandle, verb);
                    program.action.wantToRepair = false;
                    didSomething = true;
                }
                else if (
                    weight > 55 &&
                    verb.what.Equals(Verb.Sell))
                {
                    Console.WriteLine("Selling");
                    VerbWindow.Click(baseHandle, verb);
                    program.action.wantToRepair = true;
                    didSomething = true;
                }
                else if (
                    weight > 55 &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Selling at implied Button");
                    WindowHandleInfo.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y + 60 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.Click(baseHandle, implied);
                    program.action.wantToRepair = true;
                    didSomething = true;
                }
                else if (
                    weight > 75 &&
                    verb.what.Equals(Verb.Talk))
                {
                    Console.WriteLine("Selling at implied Button from Talk");
                    WindowHandleInfo.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y - 15 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.Click(baseHandle, implied);
                    program.action.wantToRepair = true;
                    didSomething = true;
                }
                else
                {
                    //Console.WriteLine("-- Nothing doing.");
                }
            }

            if (!didSomething)
            {
                Console.Write("\r\nIdle Considered doing [");
                foreach (var verb in verbWindow.verbs)
                    Console.Write("Verb[{0}],", verb.what);

                Console.WriteLine("].");
                program.scan?.DidWork();
            }
            else
            {
                
                program.lastVerbWindow = null;
                program.scan?.DidWork();
                program.windowScanManager.flushScreenScan();
            }


            return didSomething;
        }
    }
}