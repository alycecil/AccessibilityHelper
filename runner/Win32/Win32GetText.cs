using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace runner
{
    struct toolinfo
    {
        public uint size;
        public uint flag;

        public IntPtr parent;

       // public int uid;
        public Rectangle rect;

       /// public int nullvalue;
        public string text;
        //public int param;
    }
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


     
        private static IntPtr lastHWnd = IntPtr.Zero;


        public static string GetToolTipText(IntPtr hWnd)
        {   toolinfo tf = new toolinfo();
            StringBuilder title = new StringBuilder(800);

            if (Win32.SendMessage(hWnd, (int) Win32.TTM_GETTEXTA, title.Capacity, title))
            {
                return title.ToString();
            }
            else
            {
                IntPtr tempptr = Marshal.AllocHGlobal(Marshal.SizeOf(tf));
                Marshal.StructureToPtr(tf, tempptr, false);
                
                if (Win32.SendMessage(hWnd, (int) Win32.TTM_GETTOOLINFOA, 0, tempptr))
                {
                    tf = (toolinfo)Marshal.PtrToStructure(tempptr, typeof(toolinfo));
                    return tf.text;
                }
                else
                {
                    return "";
                }
          }
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