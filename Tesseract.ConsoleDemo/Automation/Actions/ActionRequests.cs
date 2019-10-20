using System;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public static partial class Action
    {
       
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
        
        public static void handleRepairControl(IntPtr basehandle)
        {
            RepairWindow.handle(basehandle);
        }

        public static void doSell(IntPtr basehandle)
        {
            SellWindow.handle(basehandle);
        }
        
        public static void askForWeight()
        {
            var basehandle = Windows.HandleBaseWindow();
            var charOut = Windows.getChatSender(basehandle);
            AutoItX.ControlSend(basehandle, charOut, "/weight\r\n");
        }

        public static void doCombat(IntPtr baseHandle)
        {
            CombatWindow.handle(baseHandle);
        }

        public static void doLoot(IntPtr baseHandle)
        {
            var loot = Windows.getLoot();
            if (loot == IntPtr.Zero) return;

            if (LootWindow.HandleLoot(loot))
            {
            }

            AutoItX.WinClose(loot);
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
    }
}