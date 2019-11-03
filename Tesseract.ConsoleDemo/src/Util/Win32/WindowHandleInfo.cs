using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace runner
{
    class WindowHandleInfo : User32Delegate
    {
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

        public static ScreenCapturer.Rect GetBounds(IntPtr handle, out Rectangle bounds)
        {
            var rect = new ScreenCapturer.Rect();
            GetWindowRect(handle, ref rect);
            ConvertRect(out bounds, rect);
            return rect;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref ScreenCapturer.Rect rect);

        public static void ConvertRect(out Rectangle bounds, System.Windows.Rect rect)
        {
            //GetScale(IntPtr.Zero, out float scaleX, out float scaleY);
            
            bounds = new Rectangle(
                (int) (rect.Left),
                (int) (rect.Top),
                (int) ((rect.Right - rect.Left)),
                (int) ((rect.Bottom - rect.Top))
            );
        }

        public static void ConvertRect(out Rectangle bounds, ScreenCapturer.Rect rect)
        {
            bounds = new Rectangle(
                (int) (rect.Left),
                (int) (rect.Top),
                (int) ((rect.Right - rect.Left)),
                (int) ((rect.Bottom - rect.Top)));
        }

        private static float x = 0;
        private static float y = 0;

        public static void GetScale(IntPtr handle, out float scaleX, out float scaleY)
        {
            if (x == 0 || y == 0)
            {
                using (var g1 = Graphics.FromHwnd(handle))
                {
                    x = g1.DpiX / 96f;
                    y = g1.DpiY / 96f;
                }

                if (x < 2) x = 2;
                if (y < 2) y = 2;
                Console.WriteLine("Scale is [{0}, {1}]", x, y);
            }

            scaleX = x;
            scaleY = y;
        }
    }
}