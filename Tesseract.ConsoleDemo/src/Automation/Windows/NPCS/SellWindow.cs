using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class SellWindow
    {
        private const int baseX = 414, baseY = 276; //, offX = 5, offY = 20;

        public static void handle(Program program, IntPtr baseHandle)
        {
            var sellForm = Windows.getNothingSelling(baseHandle);
            if (sellForm != IntPtr.Zero)
            {
                AutoItX.WinClose(sellForm);
                return;
            }

            var sellScreen = Windows.getSell(baseHandle);

            if (sellScreen == IntPtr.Zero) return;

            var ae = AutomationElement.FromHandle(sellScreen);

            TreeWalker walker = TreeWalker.ControlViewWalker;
            AutomationElement list = walker.GetFirstChild(ae);
            if (list == null)
            {
                close(program, baseHandle, sellScreen);
                return;
            }

            bool didOnce = false;
            while (SellStuff(program, baseHandle, walker, list, sellScreen))
            {
                didOnce = true;
            }

            if (didOnce)
            {
                close(program, baseHandle, sellScreen);
            }
        }

        private static bool SellStuff(Program program, IntPtr baseHandle, TreeWalker walker, AutomationElement list, IntPtr sellScreen)
        {
            var sellable = walker.GetFirstChild(list);
            if (sellable == null)
            {
                close(program, baseHandle, sellScreen);
                return false;
            }

            try
            {
                if (!sellable.TryGetClickablePoint(out var locBase)) return false;
                var config = Config.getSellableList();

                int count = 0;
                while (sellable != null)
                {
                    ScreenCapturer.ConvertRect(out var rect, sellable.Current.BoundingRectangle);
                    if (!rect.IsEmpty
                        && sellable.TryGetClickablePoint(out var loc2)
                        && wantToSell(sellable, walker, config, out string name)
                    )
                    {
                        //todo click

                        ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);

                        //TODO
                        Console.WriteLine("Selling [{0}] @ false loc {1}", name, loc2);


                        MouseManager.MouseClick(baseHandle,"RIGHT", (int) locBase.X, (int) (locBase.Y + count * rect.Height * sY));
                        return true;
                    }


                    sellable = walker.GetNextSibling(sellable);
                    count++;
                }
            }
            catch (Exception)
            {
                //IGNORE
            }

            return false;
        }


        private static bool wantToSell(AutomationElement sellable, TreeWalker walker, List<Sellable> config,
            out string name)
        {
            //Console.WriteLine(sellable.Current.Name);
            name = sellable.Current.Name;
            string price;
            var cost = walker.GetLastChild(sellable);
            price = cost?.Current.Name;
            if (price == null) return false;


            foreach (var sellable1 in config)
            {
                if (!sellable1.name.Equals(name) || !sellable1.costMatch(price)) continue;


                return true;
            }

            return false;
        }

        private static void close(Program program, IntPtr baseHandle, IntPtr sellScreen)
        {
            ScreenCapturer.GetScale(sellScreen, out float sX, out float sY);
            MouseManager.MouseClick(baseHandle,"LEFT", (int) (sX * baseX), (int) (sY * baseY));
            Thread.Sleep(100);
            
            program.action.SoldInventory();
        }
    }
}