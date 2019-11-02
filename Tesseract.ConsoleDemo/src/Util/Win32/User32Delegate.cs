using System;
using System.Runtime.InteropServices;

namespace runner
{
    internal class User32Delegate
    {
        internal delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        internal static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        
        internal delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

    }
}