using System;
using IO.Swagger.Model;
using runner.ActionWorkers;
using runner.Magic;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public partial class Action
    {
        private static readonly ApiCaller _caller = new ApiCaller();
        private Event _currentEvent;
        private Event.ActionEnum _currentAction = Idle;
        private readonly Program _program;
        private CombatWindow combatWindow = new CombatWindow();

        public Action(Program program)
        {
            this._program = program;
        }

        public bool wantToRepair = true;

        private long waitUntil = -1;
        public void HandleNextAction(IntPtr baseHandle)
        {
            bool complete = false;
            switch (_currentAction)
            {
                case Idle when ActionIdle.DoIdle(_program, baseHandle):
                    Console.WriteLine("Found a verb to do.");
                    return;
                case Idle:
                    complete = true;
                    break;

                case CheckStatus when _program.GetTick() > waitUntil:
                    complete = true;
                    break;
                case CheckStatus:
                    break;
                case Move:
                    complete = ActionMove.handle(baseHandle, _currentEvent);
                    break;

                case CheckInventory:
                    //TODO better read mana
                    complete = Inventory.handle(baseHandle);
                    break;

                case CheckHpMana:
                {
                    if (_program.GetTick() % 100 == 0)
                    {
                        this.ReadHp(baseHandle);
                    }

                    //Console.WriteLine("Checking Status");
                    break;
                }
                case Teleport when !_program.stateEngine.InState(StateEngine.OutOfCombat):
                    return;
                case Teleport:
                    complete = new SpellWindow("Teleport", SpellType.Teleport).handle(baseHandle, _program,
                        _currentEvent);
                    break;

                case Repair:
                    complete = ActionRepair.DoAction(_program, baseHandle);
                    break;
                case SellInventory:
                    complete = ActionSell.DoAction(_program, baseHandle);
                    break;

                case CombatAttack:
                case CombatCast:
                case CombatGuard:
                    this.DoCombat(baseHandle);
                    complete = Windows.getInCombat(baseHandle) == IntPtr.Zero;
                    break;
                default:
                    if (_program.stateEngine.InState(StateEngine.OutOfCombat))
                        throw new NotImplementedException();
                    break;
            }

            if (_program.stateEngine.InState(StateEngine.InCombatActing))
                return;
            HandleComplete(complete);
        }

        public void GetNextEvent(IntPtr baseHandle)
        {
//TODO
            if (_currentEvent == null || _currentAction == Idle)
            {
                GetNextEvent(_program);


                if (_currentAction != Idle)
                {
                    Console.WriteLine("Given Task : [{0}]", _currentEvent);
                }

                switch (_currentAction)
                {
                    case CheckStatus:
                        //Console.WriteLine("Checking Status [{0}]", currentEvent);
                        AskForWeight(baseHandle);
                        waitUntil = _program.GetTick() + 100; 
                        //wait no longer than 100 tics before we say we did that.
                        break;
                    case SellInventory:
                    case Repair:
                        _program.windowScanManager.requestScreenScan(baseHandle);
                        break;
                    //default:
                    //no start action
                    //    break;
                }
            }
        }

        private void GetNextEvent(Program program)
        {
//GET NEXT ONE
            _currentEvent = _caller.nextEvent(program.ego.Name);
            Event.ActionEnum? currentEventAction = _currentEvent?.Action;
            if (currentEventAction != null)
            {
                _currentAction = (Event.ActionEnum) currentEventAction;
            }
            else
            {
                _currentAction = Idle;
            }
        }

        private void HandleComplete(Event.ActionEnum actionCompleted, string paramDet = null)
        {
            if (actionCompleted == _currentAction)
            {
                HandleComplete(true);
            }
            else
            {
                Console.WriteLine("Implied Action Completed [{0}], State [{1}]", actionCompleted, _program.stateEngine);
            }

            //if(_currentAction != Idle)
            _currentAction = Idle;
        }

        private void HandleComplete(bool complete)
        {
            if (complete && _currentAction != Idle)
            {
                //tell api we are done
                Console.WriteLine("Complete Event:[{0}], State [{2}]\r\n---Api result:[{1}]", 
                    _caller.completeEvent(_program.ego.Name, _currentEvent),
                    _currentEvent, _program.stateEngine);
                _currentEvent = null;
                _currentAction = Idle;
            }
        }

        public void SetCurrentAction(Event.ActionEnum status)
        {
            _currentAction = status;
        }
    }
}