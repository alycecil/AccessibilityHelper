using System;
using System.Threading;
using AutoIt;
using IO.Swagger.Model;
using runner;
using Action = runner.Action;

namespace Tesseract.ConsoleDemo
{
    internal class Program
    {
        public static WindowScan scan = null;
        public static Player ego = new Player();
        public static StateEngine stateEngine = new StateEngine();
        public static int MaxHp = 1000;

        public static void Main(string[] args)
        {
            ImageManip.testOcr(args);
            //Win32GetText.GetToolTipText((IntPtr)0x47)
            AutoItX.Init();
            ego.Name = Config.get(Config.KEY_ME);


            var basehandle = __base();

            var chat = Windows.getChatRoom(basehandle);
            var roomLogger = new ControlLogger(basehandle, chat);

            Action.ReadHP();

            long tick = 0;
            //////MAIN LOOP
            while (true)
            {
                tick++;
                Thread.Sleep(20);
                __base();


                EveryTick(basehandle);
                FastTick(tick, basehandle, roomLogger);
                CommonTick(tick, basehandle);
                RareTick(tick);

                
                EveryTickLast();

                handleScreenScan(tick, basehandle);
                tick %= Int32.MaxValue;
            }
        }

        private static void EveryTickLast()
        {
            Action.handleNextAction();
        }

        private static void EveryTick(IntPtr basehandle)
        {
            ToolTips.handle(basehandle);
        }

        private static void FastTick(long tick, IntPtr basehandle, ControlLogger roomLogger)
        {
            if (tick % 5 != 0) return;
            roomLogger.LogRoom();
            HoverBox.handle(basehandle);
            stateEngine.HandleStateChanges(basehandle);
        }

        private static void CommonTick(long tick, IntPtr basehandle)
        {
            if (tick % 10 != 0) return;


            if (stateEngine.InState(StateEngine.InCombat))
            {
                Action.doCombat(basehandle);
            }
            else if (stateEngine.InState(StateEngine.InCobmatAfter))
            {
                Action.doLoot(basehandle);
                Action.exitCombat(basehandle);
            }
            else
                //if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                Action.handleRepairControl(basehandle);
                Action.doLoot(basehandle);
                Action.doSell(basehandle);
            }
        }

        private static void RareTick(long tick)
        {
            if (tick % 5000 != 0) return;
            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                Action.ReadHP();
                Action.askForWeight();
            }

            if (tick % 10000 != 0) return;
            //

            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                requestScreenScan();
            }
        }

        private static void handleScreenScan(long tick, IntPtr basehandle)
        {
            if (!__scanPlease) return;
            var _scan = WindowScan.scanScreen(basehandle);
            if (_scan != null)
            {
                scan = _scan;
                __scanPlease = false;
            }
        }


        private static IntPtr __base()
        {
            IntPtr basehandle = Windows.HandleBaseWindow();
            if (basehandle == IntPtr.Zero) throw new Exception("Other App Not Running");
            return basehandle;
        }

        private static bool __scanPlease = false;


        public static void requestScreenScan()
        {
            __scanPlease = true;
            scan = null;
        }
    }
}