using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using Tesseract.ConsoleDemo;

namespace runner
{
    internal static class VerbWindowHelper
    {
        const int stepSize = 7;

        public static IntPtr findWindow(IntPtr baseHandle, String mousedOver, bool allowClick)
        {
            var myHandle = __findWindow(baseHandle, mousedOver, allowClick);
            if (myHandle == IntPtr.Zero)
            {
                //var p  = MouseManager.MouseGetPos();
                if (allowClick)
                {
                    Console.WriteLine("Mouse Over New Thing [{0}], Clicking on", mousedOver);
                    MouseManager.MouseClick(baseHandle);
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    MouseManager.MouseMove(baseHandle, 0, 0, 1);

                    myHandle = __findWindow(baseHandle, mousedOver, allowClick);
                }
            }

            return myHandle;
        }

        public static VerbWindow fromHandle(Program Program, IntPtr baseHandle, IntPtr hWnd, string ocrName,
            bool lightWeight)
        {
            if (hWnd == IntPtr.Zero) return null;

            if (lightWeight)
            {
                Console.WriteLine("Built New Lightweight VerbWindow");
                return new VerbWindow(hWnd, null, ocrName);
            }

            List<Verb> verbs = new List<Verb>();

            ScreenCapturer.CaptureScreen(hWnd, out var height, out var offset, out var w, out var rect,
                out var capture);

            var captureHeight = capture.Height;

            FindVerbs(Program, baseHandle, hWnd, captureHeight, height, capture, offset, w, rect,
                verbs);

            Console.WriteLine("Built New VerbWindow with details");

            return new VerbWindow(hWnd, verbs, ocrName);
        }

        private static void FindVerbs(Program program, IntPtr baseHandle, IntPtr hWnd, int captureHeight, int height,
            Bitmap capture, int offset, int w,
            Rectangle rect, List<Verb> verbs)
        {
            //ScreenCapturer.ImageSave("DoVerb", ImageFormat.Tiff, capture);
            var end = captureHeight - height;

            //Console.WriteLine("Possible Checks - [{0}]", end / stepSize);
            for (int location = 0; location < end; location += stepSize)
            {
                Color captureTime = capture.GetPixel(20, location);

                if (PixelManager.isBlack(captureTime))
                {
                    //Console.Write("[.]");
                    continue;
                }
//                Console.Write("[X]");


                Rectangle r2 = new Rectangle(offset, location, w, height);
                var sub = new Bitmap(rect.Width, height);
                using (var g = Graphics.FromImage(sub))
                {
                    g.DrawImage(capture, new Rectangle(0, 0, sub.Width, sub.Height),
                        r2,
                        GraphicsUnit.Pixel);
                }


                string ocr = ImageManip.doOcr(sub, VerbWindow.texts);


                if (TryGetVerb(program, baseHandle, hWnd, ocr, rect, offset, w, height, out var item, 0, location))
                {
                    location += height;
                    verbs.Add(item);
                    //Console.WriteLine("Added OCR Verb");
                }
                else if (TryGetVerb(program, baseHandle, hWnd, ocr, rect, offset, w, height, out item, 1, location))
                {
                    location += height;
                    verbs.Add(item);
                    //Console.WriteLine("Added TT Verb");
                }
                else
                {
                    //Console.WriteLine("Skipped");
                }
            }

            //Console.WriteLine("Done.");
        }

        private static IntPtr __findWindow(IntPtr baseHandle, String mousedOver, bool allowClick)
        {
            IntPtr myHandle = IntPtr.Zero;

            Win32.GetWindowThreadProcessId(baseHandle, out var main);
            IntPtr shellWindow = Win32.GetShellWindow();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;

                var clz = Win32GetText.getClassName(hWnd);
                if (!clz.StartsWith(VerbWindow.VerbClass))
                    return true;

                var text = Win32GetText.GetControlText(hWnd);
                if (String.IsNullOrEmpty(text)) return true;

                ScreenCapturer.GetBounds(hWnd, out var rect);
                //Console.WriteLine("Right Class {0:x} @{1}", hWnd.ToInt32(), rect);
                Win32.GetWindowThreadProcessId(hWnd, out var thread);

                if (thread != main) return true;
                //Console.WriteLine("Right Owner {0:x}", hWnd.ToInt32());

                if (!Win32.IsWindowVisible(hWnd)) return true;
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

        private static bool TryGetVerb(Program program, IntPtr baseHandle, IntPtr hWnd, string ocr, Rectangle rect,
            int offset, int w, int height,
            out Verb item, int mode, int location)
        {
            item = null;
            bool wanted;
            string cleaned;
            var bounds = BoundsManager.BuildBounds(rect, offset, w, height, location);

            if (mode == 1)
            {
                string tipHelper = TooltipToVerb.ToolTipHelper(program, baseHandle, hWnd, ocr, bounds);
                if (String.IsNullOrEmpty(tipHelper)) return false;

                wanted = VerbWindowOCR.cleanUpOCRTT(tipHelper, out cleaned);
            }
            else
            {
                if (String.IsNullOrEmpty(ocr)) return false;
                wanted = VerbWindowOCR.cleanUpOCR(ocr, out cleaned);
            }

            if (!wanted) return false;
            ocr = cleaned;


            Console.Write("[{2}] Added Verb [{0}] @ [{1}]", ocr, bounds,
                Win32GetText.GetControlText(hWnd));
            item = new Verb(rect: bounds, what: ocr);
            return true;
        }


        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
    }
}