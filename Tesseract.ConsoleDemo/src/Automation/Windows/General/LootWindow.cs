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

        public static bool HandleLoot(Program program, IntPtr baseHandle, IntPtr hCntr)
        {
            if (hCntr == IntPtr.Zero) return false;
            
            WindowHandleInfo.GetScale(hCntr, out var sX, out var sY);
            WindowHandleInfo.GetBounds(hCntr, out Rectangle rectangle);

            if (program.ego?.Weight?.Value == null)
            {
                program.action.AskForWeight(baseHandle);
                TakeAll(baseHandle, hCntr, rectangle, sX, sY);
                close(baseHandle, hCntr, rectangle, sX, sY);
                return false;
            }
            else if (program.ego?.Weight?.Value > 98)
            {
                close(baseHandle, hCntr, rectangle, sX, sY);
            }

            //ScreenCapturer.ImageSave("RLoot", ImageFormat.Tiff, ScreenCapturer.Capture(rectangle));
            try
            {
                var ae = AutomationElement.FromHandle(hCntr);
                TreeWalker walker = TreeWalker.ControlViewWalker;
                AutomationElement list = walker.GetFirstChild(ae);

                bool closeWindow;

                bool takeAll;
                while (HandleList(program, baseHandle, walker, list, out closeWindow, out takeAll, sX, sY)) ;

                if (takeAll)
                {
                    TakeAll(baseHandle, hCntr, rectangle, sX, sY);
                }

                if (closeWindow)
                {
                    close(baseHandle, hCntr, rectangle, sX, sY);
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void TakeAll(IntPtr baseHandle, IntPtr hCntr, Rectangle rectangle, float sX, float sY)
        {
            MouseManager.MouseClickAbsolute(baseHandle, MouseButton.LEFT, (int) (rectangle.Left + sX * TakeAllX),
                (int) (rectangle.Top + sY * TakeAllY));
            Thread.Sleep(TimeSpan.FromSeconds(30));
        }

        private static void close(IntPtr baseHandle, IntPtr hCntr, Rectangle rectangle, float sX, float sY)
        {
            MouseManager.MouseClickAbsolute(baseHandle, MouseButton.LEFT, 
                (int) (rectangle.Left + sX * DoneAllX),
                (int) (rectangle.Top + sY * TakeAllY));
            //Action.askForWeight();
        }

        static string last = String.Empty;
        static int sleeps = 0;

        private static bool HandleList(Program program, IntPtr baseHandle, TreeWalker walker, AutomationElement list
            , out bool closeWindow
            , out bool takeAll
            , float sX, float sY)
        {
            takeAll = false;
            closeWindow = false;
            AutomationElement child = walker.GetFirstChild(list);
            if (child == null) return false;

            System.Windows.Rect bounds = child.Current.BoundingRectangle;
            WindowHandleInfo.ConvertRect(out var rect, bounds);
            rect.Width *= 2;
            rect.Height *= 2;

            int i = 0;
            while (child != null)
            {
                var cap = ScreenCapturer.Capture(rect);


                var currentName = child.Current.Name;

                if (Wanted(cap, currentName))
                {
                    Console.WriteLine("Looting with Desire {1}@{0}", rect, currentName);
                    MouseManager.MouseClickAbsolute(baseHandle, MouseButton.LEFT, 
                        (int) (rect.X + 13 * sX),
                        (int) (rect.Y + 4 * sY));
                    MouseManager.MouseClickAbsolute(baseHandle, MouseButton.RIGHT, 
                        (int) (rect.X + 13 * sX),
                        (int) (rect.Y + 4 * sY));
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    if (currentName.Equals(last))
                    {
                        sleeps++;
                        if (sleeps > 7)
                        {
                            program.action.AskForWeight(baseHandle);
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


        private static bool Wanted(Bitmap cap, string currentName)
        {
            if (Config.getIgnoreList().Contains(currentName))
            {
                return false;
            }

            return true;
        }
    }
}