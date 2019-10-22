using System;
using System.Drawing;
using System.Threading;
using AutoIt;
using IO.Swagger.Model;
using runner.ActionWorkers;
using runner.Magic;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public static partial class Action
    {
        static ApiCaller caller = new ApiCaller();


        private static void HandleComplete(Event.ActionEnum actionCompleted, String paramDet = null)
        {
            if (actionCompleted == _currentAction)
            {
                HandleComplete(true);
            }
            else
            {
                Console.WriteLine("Implied Action Completed [{0}]", actionCompleted);
            }

            //if(_currentAction != Idle)
            _currentAction = Idle;
        }


        private static Event currentEvent = null;
        private static Event.ActionEnum _currentAction = Idle;
        public static bool wantToRepair = true;

        public static void handleNextAction(IntPtr baseHandle)
        {
            bool complete = false;
            switch (_currentAction)
            {
                case Idle when ActionIdle.DoIdle():
                    Console.WriteLine("Found a verb to do.");
                    return;
                case Idle:
                    complete = true;
                    break;

                case CheckStatus:
                    break;
                case Move:
                    complete = ActionMove.handle(baseHandle, currentEvent);
                    break;

                case CheckInventory:
                    //TODO better read mana
                    complete = Inventory.handle(baseHandle);
                    break;

                case CheckHpMana:
                {
                    if (Program.getTick() % 100 == 0)
                    {
                        Action.ReadHP();
                    }

                    //Console.WriteLine("Checking Status");
                    break;
                }
                case Teleport when !Program.stateEngine.InState(StateEngine.OutOfCombat):
                    return;
                case Teleport:
                    complete = new SpellWindow("Teleport", SpellType.Teleport).handle(baseHandle, currentEvent);
                    break;

                case Repair:
                    complete = ActionRepair.DoAction();
                    break;
                case SellInventory:
                    complete = ActionSell.DoAction();
                    break;

                case CombatAttack:
                case CombatCast:
                case CombatGuard:
                    Action.doCombat(baseHandle);
                    complete = Windows.getInCombat() == IntPtr.Zero;
                    break;
                default:
                    if (Program.stateEngine.InState(StateEngine.OutOfCombat))
                        throw new NotImplementedException();
                    break;
            }

            //TODO
            if (currentEvent == null)
            {
                GetNextEvent();


                if (_currentAction != Idle)
                {
                    Console.WriteLine("Given Task : [{0}]", currentEvent);
                }
                
                switch (_currentAction)
                {
                    case CheckStatus:
                        Console.WriteLine("Checking Status [{0}]", currentEvent);
                        askForWeight();
                        break;
                    case SellInventory:
                    case Repair:
                        WindowScan.requestScreenScan();
                        break;
                    default:
                        //no start action
                        break;
                }
            }
            else
            {
                HandleComplete(complete);
            }
        }

        private static void GetNextEvent()
        {
//GET NEXT ONE
            currentEvent = caller.nextEvent(Program.ego.Name);
            Event.ActionEnum? currentEventAction = currentEvent?.Action;
            if (currentEventAction != null)
            {
                _currentAction = (Event.ActionEnum) currentEventAction;
            }
            else
            {
                _currentAction = Idle;
            }
        }

        private static void HandleComplete(bool complete)
        {
            if (complete && _currentAction != Idle)
            {
                //tell api we are done
                Console.WriteLine("Complete Event [{0}]+[{1}]", caller.completeEvent(Program.ego.Name, currentEvent),
                    currentEvent);
                currentEvent = null;
                _currentAction = Idle;
            }
        }

        public static void setCurrentAction(Event.ActionEnum status)
        {
            _currentAction = status;
        }
    }
}