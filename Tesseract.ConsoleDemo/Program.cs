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


                EveryTick(basehandle, roomLogger);
                FastTick(tick, basehandle);
                CommonTick(tick, basehandle);
                RareTick(tick);


                handleScreenScan(tick, basehandle);
                Action.handleNextAction();
                tick %= Int32.MaxValue;
            }
        }

        private static void EveryTick(IntPtr basehandle, ControlLogger roomLogger)
        {
            ToolTips.handle(basehandle);
            HoverBox.handle(basehandle);
            roomLogger.LogRoom();
        }

        private static void FastTick(long tick, IntPtr basehandle)
        {
            if (tick % 5 != 0) return;
            stateEngine.HandleStateChanges(basehandle);
        }

        private static void CommonTick(long tick, IntPtr basehandle)
        {
            if (tick % 10 != 0) return;


            if (stateEngine.InState(StateEngine.InCombat))
            {
             
                Action.doCombat(basehandle);   
            }else
            if (stateEngine.InState(StateEngine.InCobmatAfter))
            {
                Action.doLoot(basehandle);
                Action.exitCombat(basehandle);
            }else
            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                Action.handleRepairControl(basehandle);
                Action.doLoot(basehandle);
                Action.doSell(basehandle);
            }
            
            
        }

        private static void RareTick(long tick)
        {
            if (tick % 1000 != 0) return;
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