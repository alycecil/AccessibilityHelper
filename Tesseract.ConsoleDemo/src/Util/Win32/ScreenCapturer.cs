using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace runner
{
    public enum CaptureMode
    {
        Screen,
        Window
    }

    public static class ScreenCapturer
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        /// <summary> Capture Active Window, Desktop, Window or Control by hWnd or .NET Contro/Form and save it to a specified file.  </summary>
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %NOW% to put a timestamp in the filename</para></param>
        /// <param name="mode">Optional. The default value is CaptureMode.Window.</param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        public static void CaptureAndSave(string filename, CaptureMode mode = CaptureMode.Window,
            ImageFormat format = null)
        {
            ImageSave(filename, format, Capture(mode));
        }

        /// <summary> Capture a specific window (or control) and save it to a specified file.  </summary>
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %NOW% to put a timestamp in the filename</para></param>
        /// <param name="handle">hWnd (handle) of the window to capture</param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        public static void CaptureAndSave(string filename, IntPtr handle, ImageFormat format = null)
        {
            ImageSave(filename, format, Capture(handle));
        }


        /// <summary> Capture the active window (default) or the desktop and return it as a bitmap </summary>
        /// <param name="mode">Optional. The default value is CaptureMode.Window.</param>
        public static Bitmap Capture(CaptureMode mode = CaptureMode.Window)
        {
            return Capture(mode == CaptureMode.Screen ? GetDesktopWindow() : GetForegroundWindow());
        }


        /// <summary> Capture a specific window and return it as a bitmap </summary>
        /// <param name="handle">hWnd (handle) of the window to capture</param>
        public static Bitmap Capture(IntPtr handle)
        {
            GetScale(handle, out var scaleX, out var scaleY);
            var rect = GetBounds(handle, out var bounds);

            //Console.WriteLine("Screen Shottinas Scaled {0} x ({1},{2})", bounds, scaleX, scaleY);

            return Capture(bounds);
        }

        public static Bitmap Capture(Rectangle bounds)
        {
            try
            {
                var result = new Bitmap(bounds.Width, bounds.Height);
                using (var g = Graphics.FromImage(result))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    //g.CopyFromScreen(new Point(boundsRaw.Left,boundsRaw.Top), Point.Empty, bounds.Size);
                }
                return result;
            }
            catch (Exception){}
            
            return new Bitmap(1,1);
            
        }

        public static Rect GetBounds(IntPtr handle, out Rectangle bounds)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            ConvertRect(out bounds, rect);
            return rect;
        }

        
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
        
        public static void ConvertRect(out Rectangle bounds, Rect rect)
        {
            bounds = new Rectangle(
                (int) (rect.Left),
                (int) (rect.Top),
                (int) ((rect.Right - rect.Left)),
                (int) ((rect.Bottom - rect.Top)));
        }

        private static float x = 0, y = 0;

        public static void GetScale(IntPtr handle, out float scaleX, out float scaleY)
        {
            if (x == 0 || y == 0)
            {
                using (var g1 = Graphics.FromHwnd(handle))
                {
                    x = g1.DpiX / 96f;
                    y = g1.DpiY / 96f;
                }

                if (x < 2)
                    x = 2;
                if (y < 2)
                    y = 2;
                Console.WriteLine("Scale is [{0}, {1}]",x,y);
            }

            scaleX = x;
            scaleY = y;
        }

        /// <summary> Position of the cursor relative to the start of the capture </summary>
        public static Point CursorPosition;


        /// <summary> Save an image to a specific file </summary>
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %NOW% to put a timestamp in the filename</para></param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        /// <param name="image">Image to save.  Usually a BitMap, but can be any
        /// Image.</param>
        public static void ImageSave(string filename, ImageFormat format, Image image)
        {
            format = format ?? ImageFormat.Png;
            if (!filename.Contains("."))
                filename = filename.Trim() + "." + format.ToString().ToLower();

            if (!filename.Contains(@"\"))
                filename = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Temp", filename);

            filename = filename.Replace("%NOW%", DateTime.Now.ToString("yyyy-MM-dd@hh.mm.ss"));
            image.Save(filename, format);
        }

    }
}