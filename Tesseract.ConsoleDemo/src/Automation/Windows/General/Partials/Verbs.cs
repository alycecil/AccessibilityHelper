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
        public static readonly string 
            Repair = "Repair",
            Steal = "Pick this",
            WalkTo = "Walk",
            Sell = "Sell",
            Fight = "Start combat",
            Talk = "Talk",
            Shop = "Buy something from",
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

        public void click(out int x, out int y)
        {
            mouseover(out x, out y);
            Console.WriteLine("Clicking on ({0},{1})", x, y);
            Thread.Sleep(1);
            mouseover(out x, out y);
            AutoItX.MouseClick("LEFT", x, y, 1, 1);
        }

        public void mouseover(out int x, out int y)
        {
            x = this.rect.X + 25;
            y = this.rect.Y + 5;

            AutoItX.MouseMove(x, y, 1);
        }
    }

    public partial class VerbWindow
    {
        public static void click(Verb verb)
        {
            verb.click(out var x, out var y);
        }

        private static void FindVerbs(IntPtr hWnd, int captureHeight, int height, Bitmap capture, int offset, int w,
            Rectangle rect, List<Verb> verbs)
        {
            ScreenCapturer.ImageSave("DoVerb", ImageFormat.Tiff, capture);
            var end = captureHeight - height;
            const int stepSize = 3;
            //Console.WriteLine("Possible Checks - [{0}]", end / stepSize);
            for (int location = 0; location < end; location += stepSize)
            {
                Color captureTime = capture.GetPixel(20, location);

                if (isBlack(captureTime))
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


                string ocr = ImageManip.doOcr(sub, texts);


                if (TryGetVerb(hWnd, ocr, rect, offset, w, height, out var item, 0, location))
                {
                    location += height;
                    verbs.Add(item);
                    //Console.WriteLine("Added OCR Verb");
                }
                else if (TryGetVerb(hWnd, ocr, rect, offset, w, height, out item, 1, location))
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


        private static bool TryGetVerb(IntPtr hWnd, string ocr, Rectangle rect, int offset, int w, int height,
            out Verb item, int mode, int location)
        {
            item = null;
            bool wanted;
            string cleaned;
            var bounds = BuildBounds(rect, offset, w, height, location);

            if (mode == 1)
            {
                string tipHelper = ToolTipHelper(hWnd, ocr, bounds);
                if (string.IsNullOrEmpty(tipHelper)) return false;
                
                wanted = cleanUpOCRTT( tipHelper, out cleaned);
            }
            else
            {
                if (string.IsNullOrEmpty(ocr)) return false;
                wanted = cleanUpOCR(ocr, out cleaned);
            }

            if (!wanted) return false;
            ocr = cleaned;


            Console.Write("[{2}] Added Verb [{0}] @ [{1}]", ocr, bounds,
                Win32GetText.GetControlText(hWnd));
            item = new Verb(rect: bounds, what: ocr);
            return true;
        }


        
          private static bool cleanUpOCRTT(string ocr, out string s)
        {
            if (CleanUpOcr(ocr, out s, Verb.Repair, VerbToolTips.Repair)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Fight, VerbToolTips.Fight)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sell, VerbToolTips.Sell)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Shop, VerbToolTips.Shop)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Talk, VerbToolTips.Talk)) return true;
            if (CleanUpOcr(ocr, out s, Verb.WalkTo, VerbToolTips.WalkTo)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Cast, VerbToolTips.Cast)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Enter, VerbToolTips.Enter)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Close, VerbToolTips.Close)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Steal, VerbToolTips.Steal)) return true;
            

            Console.WriteLine("Dropping TT Unknown Verb [{0}]", ocr);
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

            Console.WriteLine("Dropping OCR Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }
        
    }
}