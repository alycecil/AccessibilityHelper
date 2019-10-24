using System;
using System.Threading;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public static partial class Action
    {
        private static int min = 0, max = 101;

        public static void updateWeightOnlyIfAbove(int pmin)
        {
            min = pmin;
        }

        public static void updateWeightOnlyIfUnder(int pmax)
        {
            max = pmax;
        }

        public static void updateWeight(string weight)
        {
            int _weight = Int32.Parse(weight.Trim());

            var __w = caller.updateWeight(Program.ego.Name, _weight);
            Program.ego.Weight = __w.Weight;

            var notTooMuch = _weight < max;
            if (_weight > min && notTooMuch)
            {
                HandleComplete(CheckStatus, "WEIGHT");
                updateWeightOnlyIfUnder(101);
            }
            else
            {
                Console.WriteLine("Ignoring Weight update [{0}], taking a nap.", _weight);

                if (!notTooMuch)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(20));
                    Console.WriteLine("Waking back up");
                }
            }
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

        public static void soldInventory()
        {
            HandleComplete(SellInventory);
        }

        public static void repaired()
        {
            HandleComplete(Repair);
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