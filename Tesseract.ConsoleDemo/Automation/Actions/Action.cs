using System;
using System.Drawing;
using AutoIt;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public static partial class Action
    {
        static ApiCaller caller = new ApiCaller();


        private static void HandleComplete(Event.ActionEnum action)
        {
            HandleComplete(action, String.Empty);
        }


        private static void HandleComplete(Event.ActionEnum checkStatus, string weight)
        {
            currentAction = Idle;
        }


        private static Event currentEvent = null;
        private static Event.ActionEnum currentAction = Idle;
        private static bool wantToRepair = true;

        public static void handleNextAction()
        {
            if (currentAction == Idle)
            {
                if (DoIdle()) return;
            }
            else if (currentAction == CheckHpMana || currentAction == CheckStatus)
            {
                Console.WriteLine("Checking Status");
            }
            else
            {
                throw new NotImplementedException();
            }

            //TODO
            if (currentEvent == null)
            {
                //GET NEXT ONE
            }
        }

        private static bool DoIdle()
        {
            var verbWindow = VerbWindow.last;


            if (verbWindow == null)
            {
                //Console.Write("[...]");
                return false;
            }

            if (verbWindow.verbs.Count == 0)
            {
                Console.WriteLine("Scrap Scan Sucked, Trying Again");
                verbWindow = VerbWindow.findWindow(Windows.HandleBaseWindow(), verbWindow.ocrText, false, false);
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
                return true;
            }

            if (!Program.stateEngine.InState(StateEngine.OutOfCombat))
            {
                Console.WriteLine("Not Lazing About, stopping doIdle");
                return false;
            }

            foreach (var verb in verbWindow.verbs)
            {
                Console.WriteLine("Idle Considering doing[{0}]", verb.what);
                var hpValue = Program.ego?.Hp?.Value;
                var weight = Program.ego?.Weight?.Value;
                
                if (hpValue == null)
                {
                    hpValue = 500;
                }
                if (weight == null)
                {
                    weight = 60;
                }
                
                if (
                    hpValue > 20 && 
                    weight< 80 &&
                    verb.what.Equals(Verb.Fight)
                )
                {
                    Console.WriteLine("Starting A fight");
                    VerbWindow.click(verb);
                }
                else if (
                    wantToRepair &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Repairing");
                    VerbWindow.click(verb);
                    wantToRepair = false;
                }
                else if (
                    weight> 55 &&
                    verb.what.Equals(Verb.Sell))
                {
                    Console.WriteLine("Selling");
                    VerbWindow.click(verb);
                    // wantToRepair = true;
                }
                else if (
                    weight> 55 &&
                    verb.what.Equals(Verb.Repair))
                {
                    Console.WriteLine("Selling at implied Button");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y + 60 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(implied);
                    // wantToRepair = true;
                } else if (
                    weight> 75 &&
                    verb.what.Equals(Verb.Talk))
                {
                    Console.WriteLine("Selling at implied Button from Talk");
                    ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
                    var r2 = new Rectangle(verb.rect.X, (int) (verb.rect.Y - 15 * sX), verb.rect.Width,
                        verb.rect.Height);
                    Verb implied = new Verb(r2, Verb.Sell);
                    VerbWindow.click(implied);
                    // wantToRepair = true;
                }
                else
                {
                    Console.WriteLine("-- Nothing doing.");
                }
            }

            return false;
        }
    }
}