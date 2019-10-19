using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace runner
{
    

    public static partial class Win32
        {
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
//        public const int TTM_GETTEXTA = (0x0400 + 11);
//        public const int TTM_GETTEXTW = (0x0400 + 56);

        public const int TTM_BASE = 0x0400,
            TTM_GETTOOLCOUNT = (TTM_BASE + 13),
//            TTM_GETTOOLINFOA = (TTM_BASE + 8),
//            TTM_GETTOOLINFOW = (TTM_BASE + 53),
//            TTM_SETTOOLINFOA = (TTM_BASE + 9),
//            TTM_SETTOOLINFOW = (TTM_BASE + 54),
//            TTM_ENUMTOOLSA = TTM_BASE + 14,
//            TTM_ENUMTOOLSW = TTM_BASE + 58,
            TTM_TRACKACTIVATE = (0x0400 + 17),
            TTM_TRACKPOSITION = (0x0400 + 18),
            TTM_ACTIVATE = (0x0400 + 1),
            TTM_POP = (0x0400 + 28),
            TTM_ADJUSTRECT = (0x400 + 31),
            TTM_SETDELAYTIME = (0x0400 + 3),
//            TTM_SETTITLEA           =(WM_USER + 32),  // wParam = TTI_*, lParam = char* szTitle
//            TTM_SETTITLEW           =(WM_USER + 33), // wParam = TTI_*, lParam = wchar* szTitle
            TTM_ADDTOOLA = (0x0400 + 4),
            TTM_ADDTOOLW = (0x0400 + 50),
            TTM_DELTOOLA = (0x0400 + 5),
            TTM_DELTOOLW = (0x0400 + 51),
            TTM_NEWTOOLRECTA = (0x0400 + 6),
            TTM_NEWTOOLRECTW = (0x0400 + 52),
            TTM_RELAYEVENT = (0x0400 + 7),
            TTM_GETTIPBKCOLOR = (0x0400 + 22),
            TTM_SETTIPBKCOLOR = (0x0400 + 19),
            TTM_SETTIPTEXTCOLOR = (0x0400 + 20),
            TTM_GETTIPTEXTCOLOR = (0x0400 + 23),
            TTM_GETTOOLINFOA = (0x0400 + 8),
            TTM_GETTOOLINFOW = (0x0400 + 53),
            TTM_SETTOOLINFOA = (0x0400 + 9),
            TTM_SETTOOLINFOW = (0x0400 + 54),
            TTM_HITTESTA = (0x0400 + 10),
            TTM_HITTESTW = (0x0400 + 55),
            TTM_GETTEXTA = (0x0400 + 11),
            TTM_GETTEXTW = (0x0400 + 56),
            TTM_UPDATE = (0x0400 + 29),
            TTM_UPDATETIPTEXTA = (0x0400 + 12),
            TTM_UPDATETIPTEXTW = (0x0400 + 57),
            TTM_ENUMTOOLSA = (0x0400 + 14),
            TTM_ENUMTOOLSW = (0x0400 + 58),
            TTM_GETCURRENTTOOLA = (0x0400 + 15),
            TTM_GETCURRENTTOOLW = (0x0400 + 59),
            TTM_WINDOWFROMPOINT = (0x0400 + 16),
            TTM_GETDELAYTIME = (0x0400 + 21),
            TTM_SETMAXTIPWIDTH = (0x0400 + 24),
            TTN_GETDISPINFOA = ((0 - 520) - 0),
            TTN_GETDISPINFOW = ((0 - 520) - 10),
            TTN_SHOW = ((0 - 520) - 1),
            TTN_POP = ((0 - 520) - 2);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

       
        [DllImport("user32.dll", EntryPoint = "SendMessage",
            CharSet = CharSet.Ansi)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        
        
        [DllImport("user32.dll", EntryPoint = "SendMessage",
            CharSet = CharSet.Ansi)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, out int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetDlgItemTextLength(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetDlgItemText(IntPtr hDlg, int nIDDlgItem,
            [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetDlgCtrlID(
            IntPtr hWnd
        );

        [DllImport("USER32.DLL")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("USER32.DLL")]
        public static extern IntPtr GetShellWindow();

        [DllImport("USER32.DLL")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr GetNextDlgGroupItem(
            IntPtr hDlg,
            IntPtr hCtl,
            IntPtr bPrevious
        );

        [DllImport("user32")]
        public static extern IntPtr GetNextDlgTabItem(
            IntPtr hDlg,
            IntPtr hCtl,
            IntPtr bPrevious
        );

        [DllImport("user32.dll", EntryPoint = "SendMessage",
            CharSet = CharSet.Ansi)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);


      

        // Various flavours of SendMessage: plain vanilla, and passing references to various structures
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageLVItem(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);

        //[DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessageLVColumn(IntPtr hWnd, int m, int wParam, ref LVCOLUMN lvc);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageHDItem(IntPtr hWnd, int msg, int wParam, ref HDITEM hdi);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageHDHITTESTINFO(IntPtr hWnd, int Msg, IntPtr wParam,
            [In, Out] HDHITTESTINFO lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr
            SendMessageTOOLINFO(IntPtr hWnd, int Msg, int wParam, TOOLINFO lParam);

        [DllImport("user32.dll", EntryPoint = "GetUpdateRect", CharSet = CharSet.Auto)]
        private static extern int GetUpdateRectInternal(IntPtr hWnd, ref Rectangle r, bool eraseBackground);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        //public static extern bool SetScrollInfo(IntPtr hWnd, int fnBar, SCROLLINFO si, bool fRedraw);

        [DllImport("user32.dll", EntryPoint = "ValidateRect", CharSet = CharSet.Auto)]
        private static extern IntPtr ValidatedRectInternal(IntPtr hWnd, ref Rectangle r);
    }
}