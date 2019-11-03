using System;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class StateEngine
    {
        public static readonly int
            UNKNOWN = -1,
            OutOfCombat = 0,
            InCombat = 1,
            InCobmatAfter = 2,
            InCobmatClickingExit = 3,
            InCombatActing = 5,
            LookAt = 4;

        private int currentState = UNKNOWN;
        private IntPtr baseHandle;
        private Program program;

        public StateEngine(Program program, IntPtr baseHandle)
        {
            this.baseHandle = baseHandle;
            this.program = program;
        }

        private void alert(int state)
        {
            Console.WriteLine("State Change from {0} to {1}", AsString(currentState), AsString(state));
            //TODO send state changes    

            if (state == InCombat)
            {
                program.windowScanManager.flushScreenScan();
                if (currentState == InCombatActing)
                {
                    program.action.ReadHp(baseHandle);
                }
                //Program.requestScreenScan();

                program.action.inCombat();
            }
            else if (currentState == InCombat 
                     || currentState == InCombatActing && state == InCobmatAfter)
            {
                program.action.AskForWeight(baseHandle);
                program.action.ReadHp(baseHandle);
            }
            else if (state == OutOfCombat)
            {
                program.windowScanManager.requestScreenScan(baseHandle);
                program.action.outOfCombat();
            }
        }

        public void HandleStateChanges()
        {
            if (Windows.getInCombat(baseHandle) != IntPtr.Zero)
            {
                seeCombatOption();
            }
            else if (Windows.getExitCombatControl(baseHandle) != IntPtr.Zero
                 && !InState(InCobmatClickingExit)
            )
            {
                seeExitCombat();
            }

            else if (Windows.HandleInventory(baseHandle) != IntPtr.Zero)
            {
                seeInventory();
            }
            else if (InState(InCombat))
            {
                combatWait();
            }
            else if (
                InState(LookAt)
                || InState(UNKNOWN)
                || Windows.getSell(baseHandle) != IntPtr.Zero
                || Windows.getNothingSelling(baseHandle) != IntPtr.Zero
                || Windows.getRepairNothingControl(baseHandle) != IntPtr.Zero
                || Windows.getRepair(baseHandle) != IntPtr.Zero)
            {
                //we out of combat probably
                state(OutOfCombat);
            }
            else if (Windows.lookatIdentifier(baseHandle) != IntPtr.Zero)
            {
                seeLookAt();
            }else if (InState(InCobmatClickingExit))
            {
                Console.WriteLine("Clicking inventory open, after combat");
                ToolTips.MoveOver(baseHandle, ExpectedToolTip.Inventory, true);
            }
        }


        private void state(int newState)
        {
            if (currentState == newState) return;
            alert(newState);
            currentState = newState;
        }

        private string AsString(int state)
        {
            switch (state)
            {
                case 0:
                    return "Out of Combat";

                case 1:
                    return "In Combat";

                case 2:
                    return "After Combat";

                case 3:
                    return "Exiting Combat";

                case 5:
                    return "Taking a Turn!";

                case 4:
                    return "Look At";

                default:
                    return "NOT KNOWN";
            }
        }


        private void seeCombatOption()
        {
            state(InCombat);
        }

        private void seeLookAt()
        {
            state(LookAt);
        }

        private void seeInventory()
        {
            if (currentState == UNKNOWN || currentState == InCobmatClickingExit)
                state(OutOfCombat);
        }

        private void seeExitCombat()
        {
            state(InCobmatAfter);
        }

        public void clickedExitCombat()
        {
            state(InCobmatClickingExit);
        }

        private void combatWait()
        {
            state(InCombatActing);
        }


        public bool InState(int state)
        {
            return state == currentState;
        }

        public override string ToString()
        {
            return "StateEngine[" +AsString(currentState)  + "]";
        }
    }
}