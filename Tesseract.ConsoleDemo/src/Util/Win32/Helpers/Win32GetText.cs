using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace runner
{
    public static partial class Win32GetText
    {
        public static string GetControlText(IntPtr hWnd)
        {
            // Get the size of the string required to hold the window title (including trailing null.) 
            Int32 titleSize = Win32.SendMessage((int) hWnd, Win32.WM_GETTEXTLENGTH, 0, 0).ToInt32();

            // If titleSize is 0, there is no title so return an empty string (or null)
            if (titleSize == 0)
                return String.Empty;

            StringBuilder title = new StringBuilder(titleSize + 1);

            Win32.SendMessage(hWnd, (int) Win32.WM_GETTEXT, title.Capacity, title);

            return title.ToString();
        }


        public static string getClassName(IntPtr hWnd)
        {
            int nRet;
            // Pre-allocate 256 characters, since this is the maximum class name length.
            StringBuilder ClassName = new StringBuilder(256);
            nRet = Win32.GetClassName(hWnd, ClassName, ClassName.Capacity);
            if (nRet == 0) return String.Empty;

            return ClassName.ToString();
        }
    }
}