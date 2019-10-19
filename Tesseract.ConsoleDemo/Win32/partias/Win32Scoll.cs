using System;
using System.Runtime.InteropServices;

namespace runner
{
    public partial  class Win32
    {
        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos,
            out int lpMaxPos);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
    }
}