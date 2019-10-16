using System;
using System.Runtime.InteropServices;
using System.Text;

namespace runner
{
    public static class Win32
    {
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int TTM_GETTEXTA = (0x0400 + 11);
        public const int TTM_GETTEXTW = (0x0400 + 56);
        public const int TTM_GETTOOLINFOA = (0x0400 + 8);

        [DllImport("user32.dll", EntryPoint = "SendMessage",
            CharSet = CharSet.Ansi)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

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
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr GetNextDlgGroupItem(
            IntPtr hDlg,
            IntPtr hCtl,
            IntPtr bPrevious
        );

        [DllImport("user32")]
        public static extern IntPtr GetNextDlgTabItem (
            IntPtr hDlg,
            IntPtr hCtl,
            IntPtr bPrevious
        );

        [DllImport("user32.dll", EntryPoint = "SendMessage",
            CharSet = CharSet.Ansi)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
    }
}