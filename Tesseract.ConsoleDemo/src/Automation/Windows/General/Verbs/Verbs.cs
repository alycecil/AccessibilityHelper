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
            Sit = "Sit",
            Stand = "Stand",
            Take = "Take",
            Close = "Close";

        public Rectangle rect;
        public string what;

        public Verb(Rectangle rect, string what)
        {
            this.rect = rect;
            this.what = what;
        }

        public void click(IntPtr baseHandle, out int x, out int y)
        {
            mouseover(baseHandle, out x, out y);
            Console.WriteLine("Clicking on ({0},{1})", x, y);
            Thread.Sleep(1);
            mouseover(baseHandle, out x, out y);
            MouseManager.MouseClick(baseHandle, "LEFT", x, y, 1, 1);
        }

        public void mouseover(IntPtr baseHandle, out int x, out int y)
        {
            x = this.rect.X + 25;
            y = this.rect.Y + 5;

            MouseManager.MouseMove(baseHandle, x, y, 1);
        }
    }

    public partial class VerbWindow
    {
        public static void click(IntPtr baseHandle, Verb verb)
        {
            verb.click(baseHandle, out var x, out var y);
        }
    }
}