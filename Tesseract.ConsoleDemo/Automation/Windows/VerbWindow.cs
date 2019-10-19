using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using AutoIt;
using static runner.Win32;

namespace runner
{
    /**
     * How found:	Focus
	hwnd=0x00170C7A 32bit class="Afx:00860000:0:00000000:00000000:0001002B" style=0x96000000 ex=0x0
Name:	"AliceDjinn"
ControlType:	UIA_PaneControlTypeId (0xC371)
     */
    public class Verb
    {
        public Rectangle rect;
        public String what;

        public Verb(Rectangle rect, string what)
        {
            this.rect = rect;
            this.what = what;
        }
    }

    public class VerbWindow
    {
        private const string texts = "StealCastLokAtFightWRrpOpenETe";
        private IntPtr hWnd;
        private List<Verb> verbs;
        private VerbWindow(IntPtr hWnd, List<Verb> v)
        {
            this.hWnd = hWnd;
            this.verbs = v;
        }

        public static VerbWindow fromHandle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero) return null;
            List<Verb> verbs = new List<Verb>();


            ScreenCapturer.GetScale(hWnd, out float sX, out float sY);
            int height = (int) (sY * 12); //pxs
            int offet = (int) (sX * 7); //pxs
            int w = (int) (sX * 66); //pxs

            ScreenCapturer.GetBounds(hWnd, out var rect);
            Bitmap capture = ScreenCapturer.Capture(rect);
            rect.Width /= 3;
            capture = ImageManip.AdjustThreshold(capture, .9f);
            capture = ImageManip.Max(capture);


            ScreenCapturer.ImageSave("CapTakeClicker", ImageFormat.Tiff, capture);

            var captureHeight = capture.Height;
            for (int location = 0; location < captureHeight - height; location++)
            {
                Color captureTime = capture.GetPixel(20, location);

                if (isBlack(captureTime))
                    continue;

                //Console.WriteLine("::: {0}", captureTime.ToString());


                Rectangle r2 = new Rectangle(offet, location, w, height);
                var sub = new Bitmap(rect.Width, height);
                using (var g = Graphics.FromImage(sub))
                {
                    g.DrawImage(capture, new Rectangle(0, 0, sub.Width, sub.Height),
                        r2,
                        GraphicsUnit.Pixel);
                }


                location += height;

                //capture = ImageManip.Invert(capture);

//                ScreenCapturer.ImageSave("CapTakeClicker_" + location,ImageFormat.Tiff, sub);

                string ocr = ImageManip.doOcr(sub, texts);

//                Console.WriteLine("Verb Window[{1}] [{0}]",
//                    ocr, location);
                
                Rectangle where = new Rectangle(rect.X+offet, rect.Y+location, w, height);

                var item = new Verb(rect: where, what:ocr);
                
                verbs.Add(item);
            }


            return new VerbWindow(hWnd, verbs);
        }

        private static bool isBlack(Color captureTime)
        {
            return captureTime.R == 0
                   && captureTime.G == 0
                   && captureTime.B == 0;
        }

        public static VerbWindow findWindow(IntPtr baseHandle, String mousedOver, bool allowClick)
        {
            return fromHandle(_findWindow(baseHandle, mousedOver, allowClick));
        }

        const string VerbClass = "Afx:00860000:0:00000000:00000000:0001002B";

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