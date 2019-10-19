using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class LootWindow
    {
        private const int TakeAllX = 40;
        private const int DoneAllX = 314 / 2;
        private const int TakeAllY = 532 / 2;

        public static bool HandleLoot(IntPtr hCntr)
        {
            ScreenCapturer.GetScale(hCntr, out var sX, out var sY);
            ScreenCapturer.GetBounds(hCntr, out Rectangle rectangle);

            if (hCntr == IntPtr.Zero) return false;
            if (Program.ego?.Weight?.Value == null)
            {
                Action.askForWeight();
                return false;
            }
            else if (Program.ego?.Weight?.Value > 90)
            {
                close(hCntr, rectangle, sX, sY);
            }

            //ScreenCapturer.ImageSave("RLoot", ImageFormat.Tiff, ScreenCapturer.Capture(rectangle));
            try
            {
                var ae = AutomationElement.FromHandle(hCntr);
                TreeWalker walker = TreeWalker.ControlViewWalker;
                AutomationElement list = walker.GetFirstChild(ae);

                bool closeWindow;

                bool takeAll;
                while (HandleList(walker, list, out closeWindow, out takeAll, sX, sY)) ;

                if (takeAll)
                {
                    TakeAll(hCntr, rectangle, sX, sY);
                }

                if (closeWindow)
                {
                    close(hCntr, rectangle, sX, sY);
                }


                return true;
            }
            catch (Exception ignore)
            {
                return false;
            }
        }

        private static void TakeAll(IntPtr hCntr, Rectangle rectangle, float sX, float sY)
        {
            AutoItX.MouseClick("LEFT", (int) (rectangle.Left + sX * TakeAllX), (int) (rectangle.Top + sY * TakeAllY));
            Thread.Sleep(400);
        }

        private static void close(IntPtr hCntr, Rectangle rectangle, float sX, float sY)
        {
            AutoItX.MouseClick("LEFT", (int) (rectangle.Left + sX * DoneAllX), (int) (rectangle.Top + sY * TakeAllY));
        }

        static string last = String.Empty;
        static int sleeps = 0;

        private static bool HandleList(TreeWalker walker, AutomationElement list
            , out bool closeWindow
            , out bool takeAll
            , float sX, float sY)
        {
            takeAll = false;
            closeWindow = false;
            AutomationElement child = walker.GetFirstChild(list);
            if (child == null) return false;

            System.Windows.Rect bounds = child.Current.BoundingRectangle;
            ScreenCapturer.ConvertRect(out var rect, bounds);
            rect.Width *= 2;
            rect.Height *= 2;

            int i = 0;
            while (child != null)
            {
                var cap = ScreenCapturer.Capture(rect);


                var currentName = child.Current.Name;

                //ScreenCapturer.ImageSave("RLoot_" + i + "_" + currentName, ImageFormat.Tiff, cap);
                //Console.WriteLine("Loot: {0} @ {1}", currentName, rect);

                if (wanted(cap, currentName))
                {
                    Console.WriteLine("Looting with Desire {1}@{0}", rect, currentName);
                    AutoItX.MouseClick("LEFT", (int) (rect.X + 3 * sX), (int) (rect.Y + 4 * sY));
                    AutoItX.MouseClick("RIGHT", (int) (rect.X + 3 * sX), (int) (rect.Y + 4 * sY));
                    Thread.Sleep(10);

                    if (currentName.Equals(last))
                    {
                        sleeps++;
                        if (sleeps > 10)
                        {
                            Action.askForWeight();
                            takeAll = true;
                            closeWindow = true;
                            sleeps = 0;
                            return false;
                        }

                        Thread.Sleep(100);
                        return true;
                    }
                    else
                    {
                        last = currentName;
                        sleeps = 0;
                    }
                }
                else if (Scroller.ScrollElement(list, ScrollAmount.NoAmount, ScrollAmount.SmallDecrement))
                {
                    i++;
                }
                else if (i > 12)
                {
                    takeAll = true;
                    return false;
                }
                else
                {
                    child = walker.GetNextSibling(child);
                    i++;
                    rect.Y += rect.Height;
                }
            }

            closeWindow = true;

            return false;
        }


        private static string[] ignore = new string[]
        {
            "Mug", "Plate", "Finger", "Pebble", "Candle", "Hammer", "Jet", "Leather Pouch", "Tobac Leaf",
            "Elder Oak Leaf", "Bowl", "Basalt Rock", "Zombie Finger", "Tooth", "Clump of Grass", "Jaw Bone"
        };

        private static bool wanted(Bitmap cap, string currentName)
        {
            if (ignore.Contains(currentName))
            {
                return false;
            }

            return true;
        }
    }
}