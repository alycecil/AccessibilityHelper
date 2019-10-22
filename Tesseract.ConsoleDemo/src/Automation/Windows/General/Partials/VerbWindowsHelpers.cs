using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using AutoIt;
using static runner.Win32;

namespace runner
{
    public partial class VerbWindow
    {
        private static VerbWindow fromHandle(IntPtr hWnd, string ocrName, bool lightWeight)
        {
            if (hWnd == IntPtr.Zero) return null;

            if (lightWeight)
            {
                Console.WriteLine("Built New Lightweight VerbWindow");
                return new VerbWindow(hWnd, null, ocrName);
            }

            List<Verb> verbs = new List<Verb>();

            CaptureScreen(hWnd, out var height, out var offset, out var w, out var rect, out var capture);

            var captureHeight = capture.Height;

            FindVerbs(hWnd, captureHeight, height, capture, offset, w, rect, verbs);

            Console.WriteLine("Built New VerbWindow with details");

            return new VerbWindow(hWnd, verbs, ocrName);
        }

        private static void CaptureScreen(IntPtr hWnd, out int height, out int offet, out int w, out Rectangle rect,
            out Bitmap capture)
        {
            ScreenCapturer.GetScale(hWnd, out float sX, out float sY);
            height = (int) (sY * 12);
            offet = (int) (sX * 7);
            w = (int) (sX * 66);

            Thread.Sleep(100);

            ScreenCapturer.GetBounds(hWnd, out rect);
            capture = ScreenCapturer.Capture(rect);
            rect.Width /= 3;
            capture = ImageManip.AdjustThreshold(capture, .9f);
            capture = ImageManip.Max(capture);
        }


        private static bool CleanUpOcr(string ocr, out string s, string resultValue, string match = null)
        {
            if (string.IsNullOrEmpty(match))
            {
                match = resultValue;
            }

            if (string.Equals(ocr, match, StringComparison.OrdinalIgnoreCase)
                || ocr.StartsWith(match, StringComparison.OrdinalIgnoreCase))
            {
                s = resultValue;
                return true;
            }

            s = null;
            return false;
        }

        private static bool isBlack(Color captureTime)
        {
            return captureTime.R == 0
                   && captureTime.G == 0
                   && captureTime.B == 0;
        }


        private static IntPtr _findWindow(IntPtr baseHandle, String mousedOver, bool allowClick)
        {
            var myHandle = __findWindow(baseHandle, mousedOver, allowClick);
            if (myHandle == IntPtr.Zero)
            {
                //var p  = AutoItX.MouseGetPos();
                if (allowClick)
                {
                    Console.WriteLine("Mouse Over New Thing [{0}], Clicking on", mousedOver);
                    AutoItX.MouseClick();
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    AutoIt.AutoItX.MouseMove(0, 0, 1);

                    myHandle = __findWindow(baseHandle, mousedOver, allowClick);
                }
            }

            return myHandle;
        }

        private static IntPtr __findWindow(IntPtr baseHandle, String mousedOver, bool allowClick)
        {
            IntPtr myHandle = IntPtr.Zero;

            GetWindowThreadProcessId(baseHandle, out var main);
            IntPtr shellWindow = GetShellWindow();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;

                var clz = Win32GetText.getClassName(hWnd);
                if (!clz.StartsWith(VerbClass))
                    return true;

                var text = Win32GetText.GetControlText(hWnd);
                if (string.IsNullOrEmpty(text)) return true;

                ScreenCapturer.GetBounds(hWnd, out var rect);
                //Console.WriteLine("Right Class {0:x} @{1}", hWnd.ToInt32(), rect);
                GetWindowThreadProcessId(hWnd, out var thread);

                if (thread != main) return true;
                //Console.WriteLine("Right Owner {0:x}", hWnd.ToInt32());

                if (!IsWindowVisible(hWnd)) return true;
//                Console.WriteLine("Visible {0:x}", hWnd.ToInt32());

                var ae = AutomationElement.FromHandle(hWnd);
                TreeWalker walker = TreeWalker.ControlViewWalker;
                AutomationElement child = walker.GetFirstChild(ae);

                if (child != null) return true;
//                Console.WriteLine("no Children {0}", child);

                myHandle = hWnd;

                return true;
            }, 0);


            return myHandle;
        }

        private static string ToolTipHelper(IntPtr hWnd, string ocr, Rectangle bounds)
        {
            new Verb(bounds, null).mouseover(out var x, out var y);
            //Console.WriteLine("--XXX-- Mouse move, sleep");
            Thread.Sleep(2);

            ToolTips.setExpected(ExpectedTT.Buttons);

            var tt = ToolTips.handle(hWnd);
            //Console.WriteLine(" --{1}--[{0}]", tt, ocr);
            return tt;
        }

        private static Rectangle BuildBounds(Rectangle rect, int offset, int w, int height, int location)
        {
            return new Rectangle(rect.X + offset, rect.Y + location, w, height);
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
    }
}