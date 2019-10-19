using System;
using AutoIt;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public static class Action
    {
        static ApiCaller caller = new ApiCaller();

        public static void askForWeight()
        {
            var basehandle = Windows.HandleBaseWindow();
            var charOut = Windows.getChatSender(basehandle);
            AutoItX.ControlSend(basehandle, charOut, "/weight\r\n");
        }

        public static void doCombat(IntPtr baseHandle)
        {
            var combat = Windows.getInCombat();
            if(combat==IntPtr.Zero) return;
            
            ScreenCapturer.CaptureAndSave("RCombat", combat);
        }
        
        public static void doLoot(IntPtr baseHandle)
        {
            var loot = Windows.getLoot();
            if (loot == IntPtr.Zero) return;

            if (LootWindow.HandleLoot(loot))
            {
            }

        }

        public static void exitCombat(IntPtr baseHandle)
        {
            if (!Program.stateEngine.InState(StateEngine.InCobmatAfter)) return;
            if (Windows.getLoot() != IntPtr.Zero) return;
            
            var exit = Windows.getExitCombatControl(baseHandle);

            if (ExitWindow.Click(baseHandle, exit))
            {
                //todo handle complete
                Program.stateEngine.clickedExitCombat();
            }


        }


        public static void updateWeight(string weight)
        {
            int _weight = Int32.Parse(weight.Trim());

            var __w = caller.updateWeight(Program.ego.Name, _weight);
            Program.ego.Weight = __w.Weight;
            
            HandleComplete(Event.ActionEnum.CheckStatus, "WEIGHT");
        }


        public static void inCombat()
        {
            caller.inCombat(Program.ego.Name);
        }

        public static void outOfCombat()
        {
            caller.outOfCombat(Program.ego.Name);
        }

        public static void ReadHP()
        {
            ToolTips.moveOver(ExpectedTT.Health);
            ToolTips.setExpected(ExpectedTT.Health);
        }

        private static void ReadMana()
        {
            ToolTips.moveOver(ExpectedTT.Mana);
            ToolTips.setExpected(ExpectedTT.Mana);
       }

        public static void ReadHPComplete(int current, int max)
        {
            caller.updateHp(Program.ego.Name, current, max);

            ReadMana();
        }

        public static void ReadManaComplete(int current)
        {
            caller.updateMana(Program.ego.Name, current);

            HandleComplete(CheckHpMana);
            
            ToolTips.setExpected(ExpectedTT.None);
        }


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

        public static void handleNextAction()
        {
            if (currentAction == Idle)
            {
            }

            //TODO
            if (currentEvent == null)
            {
                //GET NEXT ONE
            }
        }
    }
}