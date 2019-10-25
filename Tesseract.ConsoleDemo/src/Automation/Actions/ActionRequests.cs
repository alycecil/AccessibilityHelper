using System;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public partial class Action
    {
        public void ReadHP(IntPtr baseHandle)
        {
            ToolTips.moveOver(baseHandle, ExpectedTT.Health);
            ToolTips.setExpected(ExpectedTT.Health);
        }

        private void ReadMana(IntPtr baseHandle)
        {
            ToolTips.moveOver(baseHandle, ExpectedTT.Mana);
            ToolTips.setExpected(ExpectedTT.Mana);
        }

        public void handleRepairControl(IntPtr baseHandle)
        {
            RepairWindow.handle(baseHandle);
        }

        public void doSell(IntPtr baseHandle)
        {
            SellWindow.handle(_program, baseHandle);
        }

        public void askForWeight(IntPtr baseHandle)
        {
            var charOut = Windows.getChatSender(baseHandle);
            AutoItX.ControlSend(baseHandle, charOut, "/weight\r\n");
        }

        public void doCombat(IntPtr baseHandle)
        {
            CombatWindow.handle(_program, baseHandle);
        }

        public void doLoot(IntPtr baseHandle)
        {
            var loot = Windows.getLoot();
            if (loot == IntPtr.Zero) return;

            if (LootWindow.HandleLoot(_program, baseHandle, loot))
            {
            }

            AutoItX.WinClose(loot);
        }

        public void exitCombat(IntPtr baseHandle)
        {
            if (!_program.stateEngine.InState(StateEngine.InCobmatAfter)) return;
            if (Windows.getLoot() != IntPtr.Zero) return;

            var exit = Windows.getExitCombatControl(baseHandle);

            if (ExitWindow.Click(baseHandle, exit))
            {
                //todo handle complete
                _program.stateEngine.clickedExitCombat();
            }
        }
    }
}