using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AutoIt;
using runner;
using runner.Cursor;
using Tesseract.ConsoleDemo;

namespace runner
{
    /// <summary>Contains functionality to get all the open windows.</summary>
    public static class HoverBox
    {
        // private static Tesseract Ocr;
        internal static bool wantItem = false;

        static HoverBox()
        {
        }

        public static VerbWindow handle(Program program, IntPtr baseHandle, bool b = false)
        {
            var hasStuff = list(baseHandle, out int type);
            if (!string.IsNullOrEmpty(hasStuff))
            {
                var findWindow = VerbWindow.FindWindow(program, baseHandle, hasStuff, true, b);
                if (findWindow != null)
                    findWindow.type = type;
                //else flushClick()

                return findWindow;
            }

            return null;
        }

        private static readonly int hasHp = Color.FromArgb(0, 16, 113, 9).ToArgb();
        private const int item = 6244104;


//style=0x56000000
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        private static string list(IntPtr baseHandle, out int type)
        {
            type = 0;
            WindowHandleInfo.GetScale(baseHandle, out float sX, out float sY);

            int l = (int) (483 * sX),
                t = (int) (24 * sY),
                r = (int) (642 * sX),
                b = (int) (37 * sY);

            Rectangle rect = new Rectangle(l, t, r - l, b - t);

            //var c = AutoItX.PixelGetColor(rect.Left + 3, rect.Top + 3);
            using (var capture = ScreenCapturer.Capture(rect))
            {
                var h = new __helper(capture);
                var doIt = h.ShouldClick(out type);
                if (doIt)
                {
                    var ocr = DoOcr(capture);
                    if (!string.IsNullOrEmpty(ocr))
                        return ocr;
                    return "IMPLIED";
                }
            }

            return null;
        }

        private class __helper
        {
            private Bitmap capture;

            internal __helper(Bitmap capture)
            {
                this.capture = capture;
            }

            internal bool ShouldClick(out int type)
            {
                var c = GetPixelColor(3,3);
                
                bool doIt = false;

                switch (c)
                {
                    case item when !wantItem:
                        type = item;
                        break;
                    case item:
                        type = item;
                        doIt = true;
                        break;
                    default:
                    {
                        type = c;
                        if (c == hasHp)
                        {
                            doIt = true;
                        }
                        else if (CursorIsRight())
                        {
                            doIt = true;
                        }

                        break;
                    }
                }

                return doIt;
            }

            private int GetPixelColor(int x, int y)
            {
                return capture.GetPixel(x,y).ToArgb() & 0x00FFFFFF;
            }

            private bool? cursorIsHand = null;
            private bool CursorIsRight()
            {
                if (cursorIsHand == null)
                {
                    cursorIsHand = CursorUtil.isCursor(CursorUtil.hand,
                        out int clickx, out int clickY);
                }
                if (cursorIsHand == null) return false;
                return (bool) cursorIsHand;
            }
        }

        private static string DoOcr(Bitmap capture)
        {
            if (capture == null) return null;

            capture = ImageManip.AdjustThreshold(capture, .9f);
            capture = ImageManip.Max(capture);

            var ocr = ImageManip.doOcr(capture);
            capture.Dispose();

            return ocr;
        }
    }
}