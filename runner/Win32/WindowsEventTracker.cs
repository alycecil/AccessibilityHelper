using System;
using System.Runtime.InteropServices;

namespace runner
{
    class WindowsEventTracker
    {
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
                hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        readonly uint EVENT;
        const uint WINEVENT_OUTOFCONTEXT = 0;

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);

        private IntPtr hhook;

        WindowsEventTracker(uint eventId)
        {
            EVENT = eventId;
            
            // Listen for EVENT allover
            hhook = SetWinEventHook(EVENT, EVENT, IntPtr.Zero,
                procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        ~WindowsEventTracker()
        {
            UnhookWinEvent(hhook);
        }


        static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // filter out non-HWND namechanges... (eg. items within a listbox)
            if (idObject != 0 || idChild != 0)
            {
                return;
            }

            Console.WriteLine("Text of hwnd changed {0:x8}", hwnd.ToInt32());
        }
    }
}