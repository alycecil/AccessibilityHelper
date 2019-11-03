using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace runner
{
    public static partial class ScreenCapturer
    {
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
            return Capture(mode == CaptureMode.Screen ? GetDesktopWindow() : WindowHandleInfo.GetForegroundWindow());
        }


        /// <summary> Capture a specific window and return it as a bitmap </summary>
        /// <param name="handle">hWnd (handle) of the window to capture</param>
        public static Bitmap Capture(IntPtr handle)
        {
            WindowHandleInfo.GetScale(handle, out var scaleX, out var scaleY);
            var rect = WindowHandleInfo.GetBounds(handle, out var bounds);

            //Console.WriteLine("Screen Shottinas Scaled {0} x ({1},{2})", bounds, scaleX, scaleY);

            return Capture(bounds);
        }

        public static Bitmap Capture(Rectangle bounds)
        {
            try
            {
                int width = bounds.Width;
                int height = bounds.Height;
                int boundsLeft = bounds.Left;
                int boundsTop = bounds.Top;
                if (width > 1 && height > 1 && boundsLeft >= 0 && boundsTop >= 0)
                {
                    var result = new Bitmap(width, height);
                    using (var g = Graphics.FromImage(result))
                    {
                        g.CopyFromScreen(
                            new Point(boundsLeft, boundsTop),
                            Point.Empty,
                            bounds.Size);
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error Encountered in Screen Capture {0}", e);
            }
            Console.WriteLine("Nothing to capture");
            return null;
        }

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
#if DEBUG
            format = format ?? ImageFormat.Png;
            if (!filename.Contains("."))
                filename = filename.Trim() + "." + format.ToString().ToLower();

            if (!filename.Contains(@"\"))
                filename = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Temp", filename);

            filename = filename.Replace("%NOW%", DateTime.Now.ToString("yyyy-MM-dd@hh.mm.ss"));
            image.Save(filename, format);
#else
            Console.WriteLine("Not Saved");
#endif
        }
    }
}