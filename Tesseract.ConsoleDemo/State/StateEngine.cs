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

            if (state == InCobmatClickingExit)
            {
            }

            if (state == InCombat)
            {
                //Program.requestScreenScan();

                Action.inCombat();
            }
            else if (currentState == InCombat && state == InCobmatAfter)
            {
                Action.askForWeight();
                Action.ReadHP();
            }
            else if (state == OutOfCombat)
            {
                Program.requestScreenScan();
                Action.outOfCombat();
            }
        }

        public void HandleStateChanges(IntPtr basehandle)
        {
            if (Windows.getInCombat() != IntPtr.Zero)
            {
                seeCombatOption();
            }
            else if (Windows.lookatIdentifier(basehandle) != IntPtr.Zero)
            {
                seeLookAt();
            }
            else if (Windows.getExitCombatControl(basehandle) != IntPtr.Zero)
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
        }


        private void state(int newState)
        {
            if (currentState != newState)
            {
                alert(newState);
                currentState = newState;
            }
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


        public void seeCombatOption()
        {
            state(InCombat);
        }

        public void seeLookAt()
        {
            state(LookAt);
        }

        public void seeInventory()
        {
            if (currentState == UNKNOWN || currentState == InCobmatClickingExit)
                state(OutOfCombat);
        }

        public void seeExitCombat()
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