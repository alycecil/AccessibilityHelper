using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class PartialWindowScan : WindowScan
    {
        private int currentX,
            currentY,
            START_X,
            END_X,
            STEP_X,
            START_Y,
            STEP_Y,
            END_y;

        public PartialWindowScan(Program program) : base(new List<Thing>(), program)
        {
            WindowScanManager.GetConfig(program,
                out START_X,
                out END_X,
                out STEP_X,
                out START_Y,
                out STEP_Y,
                out END_y);

            var r = new Random();
            currentX = 400+r.Next(1,100);
            currentY = START_Y;
        }


        public override void tickCommon(long tick, Program program, IntPtr baseHandle)
        {
            if (!Config.screenScan())
                return;
            
            bool afterEndY = currentY > END_y-STEP_Y+1;
            bool afterEndX = currentX > (END_X-STEP_X+1);

            WindowScanManager.GetConfig(program,
                out START_X,
                out END_X,
                out STEP_X,
                out START_Y,
                out STEP_Y,
                out END_y);

            if (currentX < START_X) 
            {
                currentX = START_X;
            }

            if (currentY < START_Y)
            {
                currentY = START_Y;
            }

            if (afterEndY
                && afterEndX)
            {
                return;
            }

            else if (afterEndY)
            {
                int y = currentY;
                currentY = START_Y;
                currentX += STEP_X;
                
                //Console.WriteLine("Resetting Current Y from [{0}] to [{1}]", y, currentX);
                return;
            }
            else if (afterEndX)
            {
                var x = currentX;
                currentX = START_X + 3 + (currentX%END_X);
                //Console.WriteLine("Resetting Current X from [{0}] to [{1}]", x, currentX);
            }
            currentY += STEP_Y;


            VerbWindow.findWindow(program, baseHandle,"DISMISS")?.dismiss();
            ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);
            int _y = (int) (currentY * sY);
            int _x = (int) (currentX * sX);
            MouseManager.MouseMove(baseHandle, _x, _y, 1);

        }
    }
}