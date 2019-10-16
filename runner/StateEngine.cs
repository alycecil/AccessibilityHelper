using System;
using AutoIt;

namespace runner
{
    public class StateEngine
    {
        public static readonly int
            UNKNOWN = -1,
            OutOfCombat = 0,
            InCombat = 1,
            InCobmatAfter = 2;

        private int currentState = UNKNOWN;

        private void alert(int state)
        {
            Console.WriteLine("State Change from {0} to {1}",AsString(currentState),AsString(state));
            //TODO send state changes    


            if (state == InCombat)
            {
                Action.inCombat();
            }
            else if (currentState == InCombat && state == InCobmatAfter)
            {
                Action.askForWeight();
                Action.ReadHP();
                Action.ReadMana();
            }else if (state == OutOfCombat)
            {
                Action.outOfCombat();
            }
        }
        
        public void HandleStateChanges(IntPtr basehandle)
        {
            if (Windows.getInCombat() != IntPtr.Zero)
            {
                seeCombatOption();
            }
            else if (Windows.getExitCombatControl(basehandle) != IntPtr.Zero)
            {
                seeExitCombat();
            }
            else if (Windows.HandleInventory() != IntPtr.Zero)
            {
                seeInventory();
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
                    return "After Comber";
                
                default:
                    return "NOT KNOWN";
                    
            }
           
            
        }


        public void seeCombatOption()
        {
            state(InCombat);
        }

        public void seeInventory()
        {
            if(currentState == UNKNOWN)
                state(OutOfCombat);
        }
        
        public void seeExitCombat()
        {
            state(InCobmatAfter);
        }

        public void clickedExitCombat()
        {
            if (currentState == OutOfCombat || currentState == InCobmatAfter)
            {
                state(OutOfCombat);
            }
        }
        
        
       
    }
}