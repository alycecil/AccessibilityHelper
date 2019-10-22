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


            var basehandle = __base();

            var chat = Windows.getChatRoom(basehandle);
            var roomLogger = new ControlLogger(basehandle, chat);

            Action.ReadHP();

            
            //////MAIN LOOP
            while (true)
            {
                tick++;
                Thread.Sleep(20);
                __base();


                EveryTickFirst(basehandle);
                
                FastTick(basehandle, roomLogger);
                CommonTick(basehandle);
                RareTick();

                
                EveryTickLast(basehandle);

                
                tick %= Int32.MaxValue;
            }
        }

        private static void EveryTickLast(IntPtr basehandle)
        {
            Action.handleNextAction();
        }

        private static void EveryTickFirst(IntPtr basehandle)
        {
            ToolTips.handle(basehandle);
        }

        private static void FastTick(IntPtr basehandle, ControlLogger roomLogger)
        {
            if (tick % 5 != 0) return;
            roomLogger.LogRoom();
            HoverBox.handle(basehandle);
            stateEngine.HandleStateChanges(basehandle);
        }

        private static void CommonTick(IntPtr basehandle)
        {
            //if (tick % 1 != 0) return;


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

            scan?.tickCommon(tick);
            WindowScan.handleScreenScan(basehandle);
        }

        private static void RareTick()
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
                WindowScan.requestScreenScan();
            }
        }



        private static IntPtr __base()
        {
            IntPtr basehandle = Windows.HandleBaseWindow();
            if (basehandle == IntPtr.Zero) throw new Exception("Other App Not Running");
            return basehandle;
        }
    }
}