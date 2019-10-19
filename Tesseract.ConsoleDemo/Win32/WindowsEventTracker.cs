using System;
using System.Runtime.InteropServices;

namespace runner
{
    /// <summary>
    /// cant make this shit work
    /// </summary>
 
    public class WindowsEventTracker
    {
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
                hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public const int EVENT_MIN = 0;
        public const int EVENT_MAX = 0x7FFFFFFF;
        
        public const int WINEVENT_OUTOFCONTEXT = 0x0000;
        public const int WINEVENT_SKIPOWNTHREAD = 0x0001;
        public const int WINEVENT_SKIPOWNPROCESS = 0x0002;
        public const int WINEVENT_INCONTEXT = 0x0004;
        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);

        private IntPtr hhook;

        public WindowsEventTracker(uint eventId, uint topEnvent, IntPtr handle)
        {

            Win32.GetWindowThreadProcessId(handle, out var xxx);
            // Listen for EVENT allover
            hhook = SetWinEventHook(eventId, topEnvent, IntPtr.Zero, 
                procDelegate, 
                xxx, 0, 
                WINEVENT_OUTOFCONTEXT);
            
            Console.WriteLine("Open Hooks");

        }

        ~WindowsEventTracker()
        {
            Console.WriteLine("Closing Hooks");
            UnhookWinEvent(hhook);
        }


        static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine("Got Event {0} on {1}", eventType, hwnd);
            // filter out non-HWND namechanges... (eg. items within a listbox)
            if (idObject != 0 || idChild != 0)
            {
                return;
            }

            Console.WriteLine("Text of hwnd changed {0:x8}", hwnd.ToInt32());
        }
    }
}