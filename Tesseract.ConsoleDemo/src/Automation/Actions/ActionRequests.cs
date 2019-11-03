using System;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public partial class Action
    {
        public void ReadHp(IntPtr baseHandle)
        {
            ToolTips.MoveOver(baseHandle, ExpectedToolTip.Health);
            ToolTips.SetExpected(ExpectedToolTip.Health);
        }

        private void ReadMana(IntPtr baseHandle)
        {
            ToolTips.MoveOver(baseHandle, ExpectedToolTip.Mana);
            ToolTips.SetExpected(ExpectedToolTip.Mana);
        }

        public void HandleRepairControl(IntPtr baseHandle)
        {
            RepairWindow.handle(baseHandle, _program);
        }

        public void DoSell(IntPtr baseHandle)
        {
            SellWindow.handle(_program, baseHandle);
        }

        public void AskForWeight(IntPtr baseHandle)
        {
            var charOut = Windows.getChatSender(baseHandle);
            AutoItX.ControlSend(baseHandle, charOut, "/weight\r\n");
        }

        public void DoCombat(IntPtr baseHandle)
        {
            combatWindow.handle(_program, baseHandle);
        }

        public void DoLoot(IntPtr baseHandle)
        {
            var loot = Windows.getLoot(baseHandle);
            if (loot == IntPtr.Zero) return;

            if (LootWindow.HandleLoot(_program, baseHandle, loot))
            {
            }

            AutoItX.WinClose(loot);
        }

        public void exitCombat(IntPtr baseHandle)
        {
            if (!_program.stateEngine.InState(StateEngine.InCobmatAfter)) return;
            if (Windows.getLoot(baseHandle) != IntPtr.Zero) return;

            var exit = Windows.getExitCombatControl(baseHandle);

            if (ExitWindow.Click(baseHandle, exit))
            {
                //todo handle complete
                _program.stateEngine.clickedExitCombat();
            }
        }
    }
}