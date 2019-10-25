using System;
using runner;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class WindowScanManager
    {
        private bool __scanPlease = false,
            __fullScreenPlease = false;

        private Program Program;

        public WindowScanManager(Program program)
        {
            Program = program;
        }

        public void flushScreenScan()
        {
            //Program.scan?.delete();
            Program.scan = null;
            __scanPlease = false;
            __fullScreenPlease = false;
        }

        public void requestScreenScan(IntPtr baseHandle)
        {
            flushScreenScan();
            __scanPlease = true;
        }

        public void requestFullScreenScan()
        {
            flushScreenScan();
            __fullScreenPlease = true;
        }

        public static void GetConfig(
            Program program,
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

            if (!program.stateEngine.InState(StateEngine.InCombat)) return;
            START_X = 10;
            END_X = 640;
            STEP_X = 23;

            START_Y = 20;
        }

        public void handleScreenScan(Program program, IntPtr baseHandle)
        {
            if (!Config.screenScan())
                return;

            if (__scanPlease)
            {
                if (program.scan == null)
                {
                    program.scan = new PartialWindowScan(program);
                }
            }
            else if (__fullScreenPlease)
            {
                var _scan = FullWindowScan.scanScreen(program, baseHandle);
                if (_scan != null)
                {
                    program.scan = _scan;
                    __scanPlease = false;
                }
            }
        }
    }
}