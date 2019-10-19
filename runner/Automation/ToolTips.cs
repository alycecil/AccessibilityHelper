using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using AutoIt;
using runner;
using static runner.Win32;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class ToolTips
{
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    public static IDictionary<IntPtr, WindowsEventTracker> list(IntPtr basehandle)
    {
        GetWindowThreadProcessId(basehandle, out var main);
        IntPtr shellWindow = GetShellWindow();
        Dictionary<IntPtr, WindowsEventTracker> windows = new Dictionary<IntPtr, WindowsEventTracker>();

        EnumWindows(delegate(IntPtr hWnd, int lParam)
        {
            if (hWnd == shellWindow) return true;
            //if (!IsWindowVisible(hWnd)) return true;

            var clz = Win32GetText.getClassName(hWnd);
            GetWindowThreadProcessId(hWnd, out var thread);

            if ("tooltips_class32".Equals(clz)
                && thread == main)
            {
                int _h = (int) hWnd;

                var ae = AutomationElement.FromHandle(hWnd);

                if (!once)
                {
                    Console.WriteLine("Found Tooltip 0x{0:X8} [{1}]-[{2}] or [{3}]", _h, AutoItX.WinGetTitle(hWnd),
                        AutoItX.WinGetText(hWnd), ae.Current.HelpText);
                   
                }

                //GetToolTipText(hWnd);
                MakeQuick(hWnd);

                var winGetState = AutoItX.WinGetState(hWnd);
                //Console.WriteLine("State : {0}", winGetState);
                if ((winGetState  & 2) != 0)
                {
                    //Console.WriteLine("Visi");
                    ScreenCapturer.CaptureAndSave("alices"+hWnd,hWnd);
                    //TODO HANDLE BETTER
                    
                }
            }

            return true;
        }, 0);

        once = true;
        return windows;
    }

    public static void MakeQuick(IntPtr hWnd)
    {
        if (SendMessage(hWnd, TTM_SETDELAYTIME, TTDT.TTDT_INITIAL, 1) != IntPtr.Zero)
        {
            Console.WriteLine("Added No Delay"); 
        }
    }

    public static bool once = false;

    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    [DllImport("USER32.DLL")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);


    public static string GetToolTipText(IntPtr hWnd)
    {
        /*
         *  'WindowsForms10.tooltips_class32.app.0.141b42a_r9_ad1
        If (sbWindowClass.ToString().Contains("tooltips")) Then
            Dim nProcessId As Integer = 0
            GetWindowThreadProcessId(hwnd, nProcessId)
            Dim hProcess As IntPtr = OpenProcess(PROCESS_VM_OPERATION Or PROCESS_VM_WRITE Or PROCESS_QUERY_INFORMATION Or PROCESS_VM_READ, False, nProcessId)
            If (hProcess) Then
                Dim ti As TOOLINFO = New TOOLINFO()
                ti.cbSize = Marshal.SizeOf(GetType(TOOLINFO))
                Dim pText As IntPtr = VirtualAllocEx(hProcess, 0, 512, MEM_COMMIT, PAGE_READWRITE)
                Dim pBuf As IntPtr = VirtualAllocEx(hProcess, 0, Marshal.SizeOf(GetType(TOOLINFO)), MEM_COMMIT, PAGE_READWRITE)

                Dim nToolCount As Integer = SendMessage(hwnd, TTM_GETTOOLCOUNT, 0, 0)
                For nIndex = 0 To nToolCount - 1
                    Dim pTI As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(TOOLINFO)))
                    Marshal.StructureToPtr(ti, pTI, False)
                    WriteProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)

                    SendMessage(hwnd, TTM_ENUMTOOLSW, nIndex, pBuf)
                    ReadProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)

                    Marshal.PtrToStructure(pTI, ti)
                    ti.lpszText = pText
                    Marshal.StructureToPtr(ti, pTI, False)
                    WriteProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)
                    SendMessage(hwnd, TTM_GETTEXTW, nIndex, pBuf)

                    Dim pszMessage As IntPtr = Marshal.AllocHGlobal(512 * Marshal.SystemDefaultCharSize)
                    ReadProcessMemory(hProcess, pText, pszMessage, 512 * Marshal.SystemDefaultCharSize, Nothing)
                    Dim sMessage As String = Marshal.PtrToStringUni(pszMessage)
                    Console.WriteLine("Tooltip text : {0}", sMessage)
                    Marshal.FreeHGlobal(pszMessage)
                    Marshal.FreeHGlobal(pTI)
                Next

                VirtualFreeEx(hProcess, pBuf, Marshal.SizeOf(GetType(TOOLINFO)), MEM_RELEASE)
                VirtualFreeEx(hProcess, pText, 500, MEM_RELEASE)
                CloseHandle(hProcess)
            End If
        End If
         */
        int sizeOfI = Marshal.SizeOf(typeof(TOOLINFO));
        uint sizeOf = (uint) sizeOfI;


        GetWindowThreadProcessId(hWnd, out var rawProcess);
        var hProcess = OpenProcess(ProcessAccessFlags.QueryInformation |
                                   ProcessAccessFlags.VMRead | ProcessAccessFlags.VMWrite |
                                   ProcessAccessFlags.VMOperation, false, rawProcess);
//        if (hProcess == IntPtr.Zero)
//        {
//            hProcess = OpenProcess(ProcessAccessFlags.QueryInformation, false, rawProcess);
//        }

        if (hProcess == IntPtr.Zero)
        {
            //TODO ERROR
            if (!once)
                Console.WriteLine("Ooops, no handle for tooltip");
            return "ERROR";
        }

        var pText = VirtualAllocEx(hProcess, IntPtr.Zero, 512, AllocationType.Commit,
            MemoryProtection.ExecuteReadWrite);

        var pBuf = VirtualAllocEx(hProcess, IntPtr.Zero, sizeOf, AllocationType.Commit,
            MemoryProtection.ExecuteReadWrite);


        uint count = (uint) SendMessage(hWnd, TTM_GETTOOLCOUNT, 0, 0);
        if (count == 0)
        {
            //TODO ERROR
            if (!once)
                Console.WriteLine("Ooops, no tooltips");
            return "";
        }
        
        string sMessage = "NeverInitialized";
        int wParam = (int) (count - 1);

        
        TOOLINFO tf = new TOOLINFO();
        tf.size = sizeOfI;
        //tf.text = pText;
        tf.rect.bottom = 0;
        tf.rect.top = 0;
        tf.rect.left= 0;
        tf.rect.right = 0;
        
        
        IntPtr pTI = Marshal.AllocHGlobal(sizeOfI);
        Marshal.StructureToPtr(tf, pTI, false);
        WriteProcessMemory(hProcess, pBuf, pTI, sizeOfI, out var bytesWritten);
        WriteProcessMemory(hProcess, pText, "", 0, out var bytesWritten2);

        if (SendMessage(hWnd, TTM_SETDELAYTIME, TTDT.TTDT_INITIAL, 1) != IntPtr.Zero)
        {
           Console.WriteLine("Added No Delay"); 
        }
//        if(SendMessage(hWnd, TTM_ENUMTOOLSA, wParam, pBuf))
//        {
//            ReadProcessMemory(hProcess, pBuf, pTI, sizeOfI, out var bytesIn);
//            parseMemoryAsString( hProcess, tf.text, out sMessage);
//        }
//                 
//        if (SendMessage(hWnd, TTM_GETCURRENTTOOLA, wParam, pBuf))
//        {
//            ReadProcessMemory(hProcess, pBuf, pTI, sizeOfI, out var bytesIX2);
//            tf = (TOOLINFO) Marshal.PtrToStructure(pTI, typeof(TOOLINFO));
//
//
//            parseMemoryAsString(hProcess, tf.text, out sMessage);
//        }
//        
//        if (SendMessage(hWnd, TTM_GETTEXTA, wParam, pBuf))
//        {
//            ReadProcessMemory(hProcess, pBuf, pTI, sizeOfI, out var bytesIX2);
//            tf = (TOOLINFO) Marshal.PtrToStructure(pTI, typeof(TOOLINFO));
//
//
//            parseMemoryAsString(hProcess, tf.text, out sMessage);
//        }
//

//        SendMessage(hWnd, TTM_GETCURRENTTOOLA, 0, pBuf);
//        ReadProcessMemory(hProcess, pBuf, pTI, sizeOfI, out var in2);
//        tf = (TOOLINFO) Marshal.PtrToStructure(pTI, typeof(TOOLINFO));
//        parseMemoryAsString( hProcess, tf.text, out var sMessage2);
//      

        Marshal.FreeHGlobal(pTI);

        VirtualFreeEx(hProcess, pBuf, sizeOfI, AllocationType.Release);
        VirtualFreeEx(hProcess, pText, 500, AllocationType.Release);
        CloseHandle(hProcess);

        return sMessage;
    }
    
    public static class TTDT
    {
        public const int TTDT_AUTOMATIC = 0;
        public const int TTDT_AUTOPOP = 2;
        public const int TTDT_INITIAL = 3;
        public const int TTDT_RESHOW = 1;
    }

    private static void parseMemoryAsString(IntPtr hProcess, IntPtr pText,
        out string sMessage)
    {
        var pszMessage = Marshal.AllocHGlobal(512 * Marshal.SystemDefaultCharSize);
        ReadProcessMemory(hProcess, pText, pszMessage, 512 * Marshal.SystemDefaultCharSize, out var bytesIn2);

        sMessage = Marshal.PtrToStringAnsi(pszMessage);
        if (!string.IsNullOrEmpty(sMessage))
            Console.WriteLine("Tooltip text : [{0}] {1}", sMessage, bytesIn2);

        Marshal.FreeHGlobal(pszMessage);
    }
}


/*
 *  'WindowsForms10.tooltips_class32.app.0.141b42a_r9_ad1
        If (sbWindowClass.ToString().Contains("tooltips")) Then
            Dim nProcessId As Integer = 0
            GetWindowThreadProcessId(hwnd, nProcessId)
            Dim hProcess As IntPtr = OpenProcess(PROCESS_VM_OPERATION Or PROCESS_VM_WRITE Or PROCESS_QUERY_INFORMATION Or PROCESS_VM_READ, False, nProcessId)
            If (hProcess) Then
                Dim ti As TOOLINFO = New TOOLINFO()
                ti.cbSize = Marshal.SizeOf(GetType(TOOLINFO))
                Dim pText As IntPtr = VirtualAllocEx(hProcess, 0, 512, MEM_COMMIT, PAGE_READWRITE)
                Dim pBuf As IntPtr = VirtualAllocEx(hProcess, 0, Marshal.SizeOf(GetType(TOOLINFO)), MEM_COMMIT, PAGE_READWRITE)

                Dim nToolCount As Integer = SendMessage(hwnd, TTM_GETTOOLCOUNT, 0, 0)
                For nIndex = 0 To nToolCount - 1
                    Dim pTI As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(TOOLINFO)))
                    Marshal.StructureToPtr(ti, pTI, False)
                    WriteProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)

                    SendMessage(hwnd, TTM_ENUMTOOLSW, nIndex, pBuf)
                    ReadProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)

                    Marshal.PtrToStructure(pTI, ti)
                    ti.lpszText = pText
                    Marshal.StructureToPtr(ti, pTI, False)
                    WriteProcessMemory(hProcess, pBuf, pTI, Marshal.SizeOf(GetType(TOOLINFO)), Nothing)
                    SendMessage(hwnd, TTM_GETTEXTW, nIndex, pBuf)

                    Dim pszMessage As IntPtr = Marshal.AllocHGlobal(512 * Marshal.SystemDefaultCharSize)
                    ReadProcessMemory(hProcess, pText, pszMessage, 512 * Marshal.SystemDefaultCharSize, Nothing)
                    Dim sMessage As String = Marshal.PtrToStringUni(pszMessage)
                    Console.WriteLine("Tooltip text : {0}", sMessage)
                    Marshal.FreeHGlobal(pszMessage)
                    Marshal.FreeHGlobal(pTI)
                Next

                VirtualFreeEx(hProcess, pBuf, Marshal.SizeOf(GetType(TOOLINFO)), MEM_RELEASE)
                VirtualFreeEx(hProcess, pText, 500, MEM_RELEASE)
                CloseHandle(hProcess)
            End If
        End If
*/