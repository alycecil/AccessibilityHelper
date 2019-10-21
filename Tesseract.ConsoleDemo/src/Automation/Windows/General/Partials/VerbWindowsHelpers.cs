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
        private static void CaptureScreen(IntPtr hWnd, out int height, out int offet, out int w, out Rectangle rect,
            out Bitmap capture)
        {
            ScreenCapturer.GetScale(hWnd, out float sX, out float sY);
            height = (int) (sY * 12);
            offet = (int) (sX * 7);
            w = (int) (sX * 66);

            ScreenCapturer.GetBounds(hWnd, out rect);
            capture = ScreenCapturer.Capture(rect);
            rect.Width /= 3;
            capture = ImageManip.AdjustThreshold(capture, .9f);
            capture = ImageManip.Max(capture);
        }

        private static bool cleanUpOCRTT(string ocr, out string s)
        {
            if (CleanUpOcr(ocr, out s, Verb.LOOKAT, VerbToolTips.LOOKAT)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Repair, VerbToolTips.Repair)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Fight, VerbToolTips.Fight)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sell, VerbToolTips.Sell)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Shop, VerbToolTips.Shop)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Steal, VerbToolTips.Steal)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Talk, VerbToolTips.Talk)) return true;
            if (CleanUpOcr(ocr, out s, Verb.WalkTo, VerbToolTips.WalkTo)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Cast, VerbToolTips.Cast)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Enter, VerbToolTips.Enter)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Close, VerbToolTips.Close)) return true;

            Console.WriteLine("Dropping Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }
        
        private static bool cleanUpOCR(string ocr, out string s)
        {
            if (CleanUpOcr(ocr, out s, Verb.LOOKAT)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Repair)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Fight)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sell)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Shop)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Steal)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Talk)) return true;
            if (CleanUpOcr(ocr, out s, Verb.WalkTo)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Cast)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Enter)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Close)) return true;

            Console.WriteLine("Dropping Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }
        
        private static bool CleanUpOcr(string ocr, out string s, string which, string match = null)
        {
            if (!string.IsNullOrEmpty(match))
            {
                match = which;
            }

            if (string.Equals(ocr, match, StringComparison.OrdinalIgnoreCase)
                || ocr.StartsWith(match, StringComparison.OrdinalIgnoreCase))
            {
                s = which;
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
                Console.WriteLine("Mouse Over New Thing [{0}], Clicking on", mousedOver);
                //var p  = AutoItX.MouseGetPos();
                if (allowClick)
                {
                    AutoItX.MouseClick();
                    AutoIt.AutoItX.MouseMove(0, 0, 1);
                    Thread.Sleep(1);
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
                if (!VerbClass.Equals(clz))
                    return true;

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

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    }
}