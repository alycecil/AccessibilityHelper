using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace runner
{
    class DumpWindowsHandles
    {
        public static void windowsHandles(string[] args)
        {
            StringBuilder builder;
            
            //Console.WriteLine(Win32GetText.GetToolTipText((IntPtr)0x470FA0));

            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                int _h = (int) handle;
                string title = window.Value;
                string _class = Win32GetText.getClassName(handle);
                
                
                Console.WriteLine("Found 0x{2:x8} ({2}) [{0}] [{1}]", title, _class, _h);
                //if (title.ToLower().Contains("realm online"))
                var _tt = Win32GetText.GetControlText(handle);
                if (!string.IsNullOrEmpty(_tt))
                {
                    Console.WriteLine("    [{0}]", _tt);
                }
                if(true)
                {
                    int iHandle = (int) handle;
                    Console.WriteLine("SEARCHED : {0}({2:x8}): {1}", handle, title, iHandle);
                    List<IntPtr> x = new WindowHandleInfo(handle).GetAllChildHandles();
                    foreach (IntPtr hWnd in x)
                    {
                        title = "";

                        int length = Win32.GetWindowTextLength(hWnd);
                        if (length == 0)
                        {
                            title = "=NONE=";
                        }
                        else
                        {
                            builder = new StringBuilder(length);
                            Win32.GetWindowText(hWnd, builder, Math.Min(100, length + 1));
                            title = builder.ToString();
                        }

                        int myInt = (int) hWnd;
                        int res = Win32.GetDlgCtrlID(hWnd);
                        string className = Win32GetText.getClassName(hWnd);
                        string toolTip = Win32GetText.GetToolTipText(hWnd);
                        Console.WriteLine(" >>> : {0}({3:x8}) c {4}({4:x8}): [{1}] {2} {5}", hWnd, title, length, myInt,
                            res, className);

                        length = 10000; //GetDlgItemTextLength(hWnd, res);
                        if (length == 0) continue;
                        builder = new StringBuilder(length);
                        Win32.GetDlgItemText(hWnd, res, builder, length);
                        Console.WriteLine(" >>> --- --- : [{0}] [{1}]", builder, toolTip);
                    }
                }
            }
        }
    }
}