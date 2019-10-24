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
        private static long tick = 0;

        public static void Main(string[] args)
        {
            ImageManip.testOcr(args);
            //Win32GetText.GetToolTipText((IntPtr)0x47)
            AutoItX.Init();
            ego.Name = Config.get(Config.KEY_ME);

            new ApiCaller().login(ego.Name);

            var basehandle = __base();

            
            var roomLogger = ControlLogger.build(basehandle);
            Action.ReadHP();
            

            
            //////MAIN LOOP
            while (true)
            {
                tick++;
                Thread.Sleep(3);
                __base();


                EveryTickFirst(basehandle);
                
                FastTick(basehandle, roomLogger);
                CommonTick(basehandle);
                RareTick();

                
                EveryTickLast(basehandle);

                
                tick %= Int32.MaxValue;
            }
        }

        const int WARMUP_TICKS = 10;
        private static void EveryTickLast(IntPtr basehandle)
        {
            if(tick>WARMUP_TICKS)
                Action.handleNextAction(basehandle);
        }

        private static void EveryTickFirst(IntPtr basehandle)
        {
            ToolTips.handle(basehandle);
        }

        private static void FastTick(IntPtr basehandle, ControlLogger roomLogger)
        {
            if (tick % 3 != 0) return;
            roomLogger.LogRoom();
            scan?.tickCommon(tick, basehandle);
            WindowScan.handleScreenScan(basehandle);
            HoverBox.handle(basehandle);
            stateEngine.HandleStateChanges(basehandle);
        }

        private static void CommonTick(IntPtr basehandle)
        {
            if (tick % 7 != 0) return;


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

        private static void RareTick()
        {
            
            if (tick % 100 != 0) return;
            //

            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                if(scan == null)
                    WindowScan.requestScreenScan();
            }
            
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
                    WindowScan.requestScreenScan();
            }
        }



        private static IntPtr __base()
        {
            IntPtr basehandle = Windows.HandleBaseWindow();
            if (basehandle == IntPtr.Zero) throw new Exception("Other App Not Running");
            return basehandle;
        }

        public static long getTick()
        {
            return tick;
        }
    }
}