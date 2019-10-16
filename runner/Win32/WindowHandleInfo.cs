using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AutoIt;
using runner;

public class WindowHandleInfo
{
    private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);
    
    private IntPtr _MainHandle;

    public WindowHandleInfo(IntPtr handle)
    {
        this._MainHandle = handle;
    }

    public List<IntPtr> GetAllChildHandles()
    {
        List<IntPtr> childHandles = new List<IntPtr>();

        GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
        IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

        try
        {
            EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
            EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
        }
        finally
        {
            gcChildhandlesList.Free();
        }

        return childHandles;
    }

    private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
    {
        GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

        if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
        {
            return false;
        }

        List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
        childHandles.Add(hWnd);

        return true;
    }


    public static HashSet<IntPtr> ListGroupDlgs(IntPtr handle)
    {
        var set = new HashSet<IntPtr>();
        var prev = IntPtr.Zero;
        do
        {
            if (prev != IntPtr.Zero)
            {
                Console.WriteLine("Dlg : {0}:{0:x8}", (int) prev);
                set.Add(prev);
            }

            prev = Win32.GetNextDlgGroupItem(handle, prev, IntPtr.Zero);
        } while (prev != IntPtr.Zero && !set.Contains(prev));

        return set;
    }

    public static HashSet<IntPtr> ListTabDlgs(IntPtr handle)
    {
        var set = new HashSet<IntPtr>();
        var prev = IntPtr.Zero;
        do
        {
            if (prev != IntPtr.Zero)
            {
                Console.WriteLine("Dlg : {0}:{0:x8}", (int) prev);
                set.Add(prev);
            }

            prev = Win32.GetNextDlgTabItem(handle, prev, IntPtr.Zero);
        } while (prev != IntPtr.Zero && !set.Contains(prev));

        return set;
    }

    public static WindowHandleInfo DumpDetails(IntPtr handle)
    {
        var windowHandleInfo = new WindowHandleInfo(handle);
        foreach (var childHandle in windowHandleInfo.GetAllChildHandles())
        {
            Console.WriteLine("--- Child ({0}) : [{1}]:[{2}]]",
                childHandle,
                StringUtil.ToLiteral(StringUtil.SubString(AutoItX.WinGetTitle(childHandle),100)),
                (AutoItX.WinGetText(childHandle)));
        }

        var set = ListGroupDlgs(handle);
        var set2 = ListTabDlgs(handle);
        return windowHandleInfo;
    }
}