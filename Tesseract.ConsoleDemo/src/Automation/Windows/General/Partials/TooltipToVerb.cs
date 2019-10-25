using System;
using System.Drawing;
using System.Threading;
using runner;
using Tesseract.ConsoleDemo;

namespace runner
{
    static internal class TooltipToVerb
    {
        public static string ToolTipHelper(Program program, IntPtr baseHandle, IntPtr hWnd, string ocr,
            Rectangle bounds)
        {
            new Verb(bounds, null).mouseover(baseHandle, out var x, out var y);
            //Console.WriteLine("--XXX-- Mouse move, sleep");
            Thread.Sleep(2);

            ToolTips.setExpected(ExpectedTT.Buttons);

            var tt = ToolTips.handle(program, hWnd);
            //Console.WriteLine(" --{1}--[{0}]", tt, ocr);
            return tt;
        }
    }
}