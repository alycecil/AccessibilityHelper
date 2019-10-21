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
    public static class VerbToolTips
    {
        public static readonly string LOOKAT = "Look At",
            Repair = "Repair",
            Steal = "Steal",
            WalkTo = "Walk To",
            Sell = "Hail",
            Fight = "Fight",
            Talk = "Talk",
            Shop = "Shop",
            Cast = "Cast",
            Enter = "Eninr",
            Close = "Close";
    }

    public class Verb
    {
        public static readonly string LOOKAT = "Look At",
            Repair = "Repair",
            Steal = "Steal",
            WalkTo = "Walk To",
            Sell = "Hail",
            Fight = "Fight",
            Talk = "Talk",
            Shop = "Shop",
            Cast = "Cast",
            Enter = "Eninr",
            Close = "Close";

        public Rectangle rect;
        public string what;

        public Verb(Rectangle rect, string what)
        {
            this.rect = rect;
            this.what = what;
        }
    }
    
    public partial class VerbWindow
    {
        private static void FindVerbs(IntPtr hWnd, int captureHeight, int height, Bitmap capture, int offset, int w,
            Rectangle rect, List<Verb> verbs)
        {
            for (int location = 0; location < captureHeight - height; location += 3)
            {
                Color captureTime = capture.GetPixel(20, location);

                if (isBlack(captureTime))
                    continue;


                Rectangle r2 = new Rectangle(offset, location, w, height);
                var sub = new Bitmap(rect.Width, height);
                using (var g = Graphics.FromImage(sub))
                {
                    g.DrawImage(capture, new Rectangle(0, 0, sub.Width, sub.Height),
                        r2,
                        GraphicsUnit.Pixel);
                }


                string ocr = ImageManip.doOcr(sub, texts);

                ToolTips.setExpected(ExpectedTT.Buttons);
                var tt = ToolTips.handle(hWnd);
                Console.WriteLine(" --{1}--[{0}]", tt, ocr);
                if (string.IsNullOrEmpty(ocr)) continue;
                if (!TryGetVerb(hWnd, ocr, rect, offset, w, height, ref location, out var item, 0)) continue;
                if (!TryGetVerb(hWnd, ocr, rect, offset, w, height, ref location, out item, 1)) continue;


                verbs.Add(item);
            }
        }


        private static bool TryGetVerb(IntPtr hWnd, string ocr, Rectangle rect, int offset, int w, int height,
            ref int location,
            out Verb item, int mode)
        {
            item = null;
            bool wanted;
            string cleaned;
            if (mode == 1)
            {
                wanted = cleanUpOCRTT(ocr, out cleaned);
            }
            else
            {
                wanted = cleanUpOCR(ocr, out cleaned);
            }

            if (!wanted) return false;
            ocr = cleaned;


            Rectangle where = new Rectangle(rect.X + offset, rect.Y + location, w, height);

            Console.WriteLine("[{2}] Added Verb [{0}] @ [{1}]", ocr, @where,
                Win32GetText.GetControlText(hWnd));
            item = new Verb(rect: @where, what: ocr);

            location += height;
            return true;
        }
    }
}