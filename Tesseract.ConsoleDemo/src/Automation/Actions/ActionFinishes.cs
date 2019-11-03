using System;
using System.Threading;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;
using static IO.Swagger.Model.Event.ActionEnum;

namespace runner
{
    public partial class Action
    {
        private static int min = 0, max = 101;

        public void UpdateWeightOnlyIfAbove(int pmin)
        {
            min = pmin;
        }

        public void UpdateWeightOnlyIfUnder(int pmax)
        {
            max = pmax;
        }

        public void UpdateWeight(string weight)
        {
            int _weight = Int32.Parse(weight.Trim());

            var w = _caller.updateWeight(_program.ego.Name, _weight);
            if (w != null)
            {
                _program.ego.Weight = w.Weight;
            }
            else
            {
                _program.ego.Weight = new Expiringint(false, _weight);
            }

            var notTooMuch = _weight < max;
            if (_weight > min && notTooMuch)
            {
                HandleComplete(CheckStatus, "WEIGHT");
                UpdateWeightOnlyIfUnder(101);
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

        public void ReadHpComplete(IntPtr baseHandle, int current, int max)
        {
            _caller.updateHp(_program.ego.Name, current, max);

            _program.MaxHp = max;
            ReadMana(baseHandle);
        }

        public void ReadManaComplete(int current)
        {
            _caller.updateMana(_program.ego.Name, current);

            HandleComplete(CheckHpMana);

            ToolTips.SetExpected(ExpectedToolTip.None);
        }

        public void SoldInventory()
        {
            Console.WriteLine("Sold Stuff, lets get out weight below 70% or we cant do a thing");
            SetCurrentAction(Event.ActionEnum.CheckStatus);
            UpdateWeightOnlyIfUnder(70);
            AskForWeight(_program.baseHandle);


            //we need to wait for a voice command on what to do, if weight over 70% 
            //HandleComplete(SellInventory);
        }

        public void Repaired()
        {
            HandleComplete(Repair);
        }


        public void inCombat()
        {
            _caller.inCombat(_program.ego.Name);
        }

        public void outOfCombat()
        {
            _caller.outOfCombat(_program.ego.Name);
        }
    }
}