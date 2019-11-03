using System;
using System.Collections.Generic;
using AutoIt;

namespace runner
{
    public class Windows
    {
        const int chatSendId = 665;
        const int chatRoomUsualId = 666;

        const string _baseClass = "The Realm Online",
            _titleInventory = "Inventory",
            _TreasureList = "Treasure List",
            _ExitCombat = "Exit Combat",
            _CombatWindow = "Choose an action...",
            _SpellList = "Spell List",
            _Teleport = "Teleport",
            _Sale = "Sale",
            _Repair = "Repair",
            _ChatRoom = "_ChatRoom",
            _ChatSend = "_ChatSend";


        private static IntPtr getHandle(IntPtr baseHandle, string which)
        {
            if (AutoItX.WinExists(which) != 0)
            {
                return AutoItX.WinGetHandle(which);
            }

            return IntPtr.Zero;
        }

        public static IntPtr getTeleport(IntPtr baseHandle)
        {
            return getHandle(baseHandle, _Teleport);
        }

        public static IntPtr getLoot(IntPtr baseHandle)
        {
            return getHandle(baseHandle, _TreasureList);
        }

        public static IntPtr getInCombat(IntPtr baseHandle)
        {
            return getHandle(baseHandle, _CombatWindow);
        }

        public static IntPtr getExitCombatControl(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(baseHandle, "[Text:" + _ExitCombat + "]");
        }


        public static IntPtr getSell(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.ControlGetHandle(baseHandle, "[Text:" + _Sale + "]");
        }

        public static IntPtr getNothingSelling(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.WinGetHandle(_Sale);
        }

        public static IntPtr getRepair(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;


            return AutoItX.ControlGetHandle(baseHandle, "[Text:" + _Repair + "]");
        }

        public static IntPtr getRepairNothingControl(IntPtr baseHandle)
        {
            if (baseHandle == IntPtr.Zero) return IntPtr.Zero;

            return AutoItX.WinGetHandle(_Repair);
        }

        public static IntPtr getChatSender(IntPtr baseHandle)
        {
            return AutoItX.ControlGetHandle(baseHandle, "[ID:" + chatSendId + "]");
        }

        public static IntPtr getSpellList(IntPtr baseHandle)
        {
            return getHandle(baseHandle, _SpellList);
        }

        public static IntPtr lookatIdentifier(IntPtr baseHandle)
        {
            var focused = AutoItX.ControlGetHandle(baseHandle, "[CLASS:RichEdit20A]");

            if (focused == baseHandle) return IntPtr.Zero;

            var ID = Win32.GetDlgCtrlID(focused);
            if (ID <= 666 || ID == 700) return IntPtr.Zero;

            var t = Win32GetText.GetControlText(focused);
            if (!string.IsNullOrEmpty(t))
            {
                return focused;
            }

            return IntPtr.Zero;
        }

        public static IntPtr HandleInventory(IntPtr baseHandle)
        {
            IntPtr handleInventory = getHandle(baseHandle, _titleInventory);
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

        public static string GetKnownWindow(IntPtr hWnd, string text)
        {
            string result = null;
            switch (text)
            {
                case _Repair:
                    result = _Repair;
                    break;

                case _Sale:
                    result = _Sale;
                    break;

                case _Teleport:
                    result = _Teleport;
                    break;
                case _titleInventory:
                    result = _titleInventory;
                    break;
                case _CombatWindow:
                    result = _CombatWindow;
                    break;
                case _ExitCombat:
                    result = _ExitCombat;
                    break;
                case _SpellList:
                    result = _SpellList;
                    break;
                case _TreasureList:
                    result = _TreasureList;
                    break;
            }

            if (result == null)
            {
                var ID = Win32.GetDlgCtrlID(hWnd);
                switch (ID)
                {
                    case chatSendId:
                        result = _ChatSend;
                        break;
                    case chatRoomUsualId:
                        result = _ChatRoom;
                        break;
                }
            }

            return result;
        }
    }
}