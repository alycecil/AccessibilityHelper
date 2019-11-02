using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using runner;

namespace runner
{
    /// <summary>Contains functionality to get all the open windows.</summary>
internal class OpenWindowGetter : User32Delegate
{
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    public static IDictionary<IntPtr, string> GetOpenWindows()
    {
        IntPtr shellWindow = Win32.GetShellWindow();
        Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

        EnumWindows(delegate(IntPtr hWnd, int lParam)
        {
            if (hWnd == shellWindow) return true;
            //if (!IsWindowVisible(hWnd)) return true;

            int length = Win32.GetWindowTextLength(hWnd);
            if (length == 0) return true;

            StringBuilder builder = new StringBuilder(length);
            Win32.GetWindowText(hWnd, builder, length + 1);

            windows[hWnd] = builder.ToString();
            return true;
        }, 0);

        return windows;
    }
}
}