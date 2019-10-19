using System;
using System.Threading;
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
            Win32.move(500, 350);
        }

        public static void ReadMana()
        {  var baseHandle= Windows.HandleBaseWindow();
            //AutoItX.MouseMove(590, 350, 1);
            
        }
    }
}