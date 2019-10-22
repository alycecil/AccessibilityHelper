using System;
using System.Collections.Generic;
using Tesseract.ConsoleDemo;

namespace runner
{
    public abstract class WindowScan : ITickable
    {
        public List<Thing> things;

        public WindowScan(List<Thing> things)
        {
            this.things = things;
        }

        private static bool __scanPlease = false,
            __fullScreenPlease = false;

        public static void handleScreenScan(IntPtr basehandle)
        {
            if (!Config.screenScan())
                return;

            if (__scanPlease)
            {
                if (Program.scan == null)
                {
                    Program.scan = new PartialWindowScan();
                }
            }
            else if (__fullScreenPlease)
            {
                var _scan = FullWindowScan.scanScreen(basehandle);
                if (_scan != null)
                {
                    Program.scan = _scan;
                    __scanPlease = false;
                }
            }
        }

        public static void flushScreenScan()
        {
            //Program.scan?.delete();
            Program.scan = null;
            __scanPlease = false;
            __fullScreenPlease = false;
        }

        public static void requestScreenScan()
        {
            flushScreenScan();
            __scanPlease = true;
        }

        public static void requestFullScreenScan()
        {
            flushScreenScan();
            __fullScreenPlease = true;
        }

        public static void GetConfig(
            out int START_X,
            out int END_X,
            out int STEP_X,
            out int START_Y,
            out int STEP_Y,
            out int END_y)
        {
            START_X = 30;
            END_X = 620;
            STEP_X = 23;
            START_Y = 90;
            STEP_Y = 13;
            END_y = 288;

            if (!Program.stateEngine.InState(StateEngine.InCombat)) return;
            START_X = 10;
            END_X = 640;
            STEP_X = 23;

            START_Y = 20;
        }

        public abstract void tickCommon(long tick, IntPtr baseHandle);
    }
}