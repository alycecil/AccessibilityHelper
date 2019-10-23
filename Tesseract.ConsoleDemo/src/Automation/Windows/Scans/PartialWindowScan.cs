using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;

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

        public PartialWindowScan() : base(new List<Thing>())
        {
            GetConfig(out START_X,
                out END_X,
                out STEP_X,
                out START_Y,
                out STEP_Y,
                out END_y);

            currentX = START_X;
            currentY = START_Y;
        }


        public override void tickCommon(long tick, IntPtr baseHandle)
        {
            if (!Config.screenScan())
                return;
            
            bool afterEndY = currentY > END_y-STEP_Y;
            bool afterEndX = currentX > END_X-STEP_X;

            GetConfig(out START_X,
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
                currentY = START_Y;
                currentX += STEP_X;
                return;
            }
            else if (afterEndX)
            {
                currentX = START_X + 3 + currentX%END_X;
            }
            currentY += STEP_Y;


            ScreenCapturer.GetScale(IntPtr.Zero, out float sX, out float sY);
            VerbWindow.findWindow(baseHandle,"DISMISS")?.dismiss();
            AutoItX.MouseMove((int) (currentX * sX), (int) (currentY * sY), 1);

        }
    }
}