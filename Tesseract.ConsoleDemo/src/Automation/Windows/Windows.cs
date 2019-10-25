using System;
using System.Collections.Generic;
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
        public static IntPtr getTeleport()
        {
            return getHandle("Teleport");
        }

        public static IntPtr getLoot()
        {
            return getHandle(_TreasureList);
        }

        public static IntPtr getInCombat(IntPtr baseHandle)
        {
            return getHandle(_CombatWindow);
        }

        public static IntPtr getExitCombatControl(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(baseHandle, "[Text:" + _ExitCombat + "]");
        }
        
        public static IntPtr getSell(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(baseHandle, "[Text:Sale]");
        }
        
        public static IntPtr getNothingSelling(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.WinGetHandle("Sale");
        }
        
        public static IntPtr getRepair(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(baseHandle, "[Text:Repair]");
        }
        
        public static IntPtr getRepairNothingControl(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.WinGetHandle("Repair");
        }

        public static IntPtr getChatSender(IntPtr baseHandle)
        {
            return AutoItX.ControlGetHandle(baseHandle, "[ID:665]");
        }

        public static IntPtr getChatRoom(IntPtr baseHandle)
        {
            return AutoItX.ControlGetHandle(baseHandle, "[ID:666]");
        }

        public static IntPtr getSpellList(IntPtr baseHandle)
        {
            return getHandle(_SpellList);
        }

        public static IntPtr lookatIdentifier(IntPtr baseHandle)
        {
            //todo this does up by a couple
            var focused = AutoItX.ControlGetHandle(baseHandle, "[CLASS:RichEdit20A]");
            
            if (focused == baseHandle) return IntPtr.Zero;

            var ID = Win32.GetDlgCtrlID(focused);
            if(ID <= 666 || ID == 700) return IntPtr.Zero;
            
            var t = Win32GetText.GetControlText(focused);
            if (!string.IsNullOrEmpty(t))
            {
                return focused;
            }

            return IntPtr.Zero;
        }

        public static IntPtr HandleInventory()
        {
            IntPtr handleInventory = getHandle(_titleInventory);
            if(handleInventory!=IntPtr.Zero)
                AutoItX.WinMove(handleInventory, 0, 800);
            return handleInventory;
        }

        public static List<IntPtr> HandleBaseWindows()
        {
            //TODO AE, listing
            if (AutoItX.WinExists(_baseClass) != 0)
            {
                var handle = AutoItX.WinGetHandle(_baseClass);
                AutoItX.WinMove(handle, 0, 0);

                return new List<IntPtr> {handle};
            }

            return new List<IntPtr>();
        }
    }
}