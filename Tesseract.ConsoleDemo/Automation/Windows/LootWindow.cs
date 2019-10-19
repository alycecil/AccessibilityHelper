using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Automation;
using AutoIt;

namespace runner
{
    public class LootWindow
    {
        private const int TakeAllX = 40;
        private const int DoneAllX = 314 / 2;
        private const int TakeAllY = 532 / 2;

        public static bool HandleLoot(IntPtr hCntr)
        {
            if (hCntr == IntPtr.Zero) return false;
            ScreenCapturer.GetScale(hCntr, out var sX, out var sY);
            ScreenCapturer.GetBounds(hCntr, out Rectangle rectangle);

            ScreenCapturer.ImageSave("RLoot", ImageFormat.Tiff, ScreenCapturer.Capture(rectangle));
            try
            {
                var ae = AutomationElement.FromHandle(hCntr);
                TreeWalker walker = TreeWalker.ControlViewWalker;
                AutomationElement list = walker.GetFirstChild(ae);

                bool closeWindow;
                while (HandleList(walker, list, out closeWindow)) ;

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

        private static void close(IntPtr hCntr, Rectangle rectangle, float sX, float sY)
        {
            AutoItX.MouseClick("LEFT", (int) (rectangle.Left + sX * DoneAllX), (int) (rectangle.Top + sY * TakeAllY));
        }

        private static bool HandleList(TreeWalker walker, AutomationElement list, out bool closeWindow)
        {
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

                ScreenCapturer.ImageSave("RLoot_" + i + "_" + currentName, ImageFormat.Tiff, cap);
                Console.WriteLine("Loot: {0} @ {1}", currentName, rect);

                if (wanted(cap, currentName))
                {
                    Console.WriteLine("Clicking with Desire @{0}", rect);
                    AutoItX.MouseClick("RIGHT", (int) rect.X + 4, (int) rect.Y + 4);
                    return true;
                }


                child = walker.GetNextSibling(child);
                i++;
                rect.Y += rect.Height;
            }

            closeWindow = true;
            return false;
        }

        private static void TakeAll()
        {
            throw new NotImplementedException();
        }

        private static string[] ignore = new string[]
            {"Mug", "Plate", "Finger", "Pebble", "Candle", "Hammer", "Jet", "Leather Pouch", "Tobac Leaf", "Elder Oak Leaf"};

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