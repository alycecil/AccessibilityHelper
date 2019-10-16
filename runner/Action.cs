using System;
using AutoIt;

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


        public static void updateWeight(string weight)
        {
            int _weight = Int32.Parse(weight.Trim());

            var __w = caller.updateWeight(Program.ego.Name, _weight);
            Program.ego.Weight = __w.Weight;
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
            var baseHandle= Windows.HandleBaseWindow();
          

        }

        public static void ReadMana()
        {
           
        }
    }
}