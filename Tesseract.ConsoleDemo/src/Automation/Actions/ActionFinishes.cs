using System;
using AutoIt;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public static partial class Action
    {
        public static void updateWeight(string weight)
        {
            int _weight = Int32.Parse(weight.Trim());

            var __w = caller.updateWeight(Program.ego.Name, _weight);
            Program.ego.Weight = __w.Weight;

            HandleComplete(Event.ActionEnum.CheckStatus, "WEIGHT");
        }
        
        public static void ReadHPComplete(int current, int max)
        {
            caller.updateHp(Program.ego.Name, current, max);

            Program.MaxHp = max;
            ReadMana();
        }

        public static void ReadManaComplete(int current)
        {
            caller.updateMana(Program.ego.Name, current);

            HandleComplete(CheckHpMana);

            ToolTips.setExpected(ExpectedTT.None);
        }
        
        
      
        
        public static void inCombat()
        {
            caller.inCombat(Program.ego.Name);
        }

        public static void outOfCombat()
        {
            caller.outOfCombat(Program.ego.Name);
        }

    }
}