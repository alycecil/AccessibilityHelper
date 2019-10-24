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

        private void alert(int state)
        {
            Console.WriteLine("State Change from {0} to {1}", AsString(currentState), AsString(state));
            //TODO send state changes    

            if (state == InCombat)
            {
                WindowScan.flushScreenScan();
                if (currentState == InCombatActing)
                {
                    Action.ReadHP();
                }
                //Program.requestScreenScan();

                Action.inCombat();
            }
            else if (currentState == InCombat 
                     || currentState == InCombatActing && state == InCobmatAfter)
            {
                Action.askForWeight();
                Action.ReadHP();
            }
            else if (state == OutOfCombat)
            {
                WindowScan.requestScreenScan();
                Action.outOfCombat();
            }
        }

        public void HandleStateChanges(IntPtr basehandle)
        {
            if (Windows.getInCombat() != IntPtr.Zero)
            {
                seeCombatOption();
            }
            else if (Windows.getExitCombatControl(basehandle) != IntPtr.Zero
                 && !InState(InCobmatClickingExit)
            )
            {
                seeExitCombat();
            }

            else if (Windows.HandleInventory() != IntPtr.Zero)
            {
                seeInventory();
            }
            else if (InState(InCombat))
            {
                combatWait();
            }
            else if (
                InState(InCobmatClickingExit)
                || InState(LookAt)
                || InState(UNKNOWN)
                || Windows.getSell(basehandle) != IntPtr.Zero
                || Windows.getNothingSelling(basehandle) != IntPtr.Zero
                || Windows.getRepairNothingControl(basehandle) != IntPtr.Zero
                || Windows.getRepair(basehandle) != IntPtr.Zero)
            {
                //we out of combat probably
                state(OutOfCombat);
            }
            else if (Windows.lookatIdentifier(basehandle) != IntPtr.Zero)
            {
                seeLookAt();
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
    }
}