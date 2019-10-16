using System;
using AutoIt;

namespace runner
{
    public class Windows
    {
        private static string _baseClass = "The Realm Online";
        private static string _titleInventory = "Inventory";
        private static string _TreasureList = "Treasure List";
        private static string _ExitCombat = "Exit Combat";
        private static string _CombatWindow = "Choose an action...";
        private static string _SpellList = "Spell List";


        private static IntPtr getHandle(String which)
        {
            if (AutoItX.WinExists(which) != 0)
            {
                return AutoItX.WinGetHandle(which);
            }

            return IntPtr.Zero;
        }
        
        
        public static IntPtr getLoot()
        {
            return getHandle(_TreasureList);
        }
        
        public static IntPtr getInCombat()
        {
            return getHandle(_CombatWindow);
        }

        public static IntPtr getExitCombatControl(IntPtr basehandle)
        {
            if (basehandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(basehandle, "[Text:" + _ExitCombat + "]");
        }

        public static IntPtr getChatSender(IntPtr basehandle)
        {
            return AutoItX.ControlGetHandle(basehandle, "[ID:665]");
        }
        
        public static IntPtr getChatRoom(IntPtr basehandle)
        {
            return AutoItX.ControlGetHandle(basehandle, "[ID:666]");
        }

//        public static IntPtr getWhoIsBar(IntPtr basehandle)
//        {
//            return AutoItX.ControlGetHandle(basehandle, "[ID:9408152]");
//        }
//
//        public static IntPtr getGameBar(IntPtr basehandle)
//        {
//            return AutoItX.ControlGetHandle(basehandle, "[ID:68325664]");
//        }


     


        public static IntPtr HandleInventory()
        {
            return getHandle(_titleInventory);
        }

        public static IntPtr HandleBaseWindow()
        {
            if (AutoItX.WinExists(_baseClass) != 0)
            {
                var handle = AutoItX.WinGetHandle(_baseClass);
                AutoItX.WinMove(handle, 0, 0);
                
                return handle;
            }

            return IntPtr.Zero;
        }


        public static void click(IntPtr window, IntPtr handle)
        {
            //TODO
        }
    }
}