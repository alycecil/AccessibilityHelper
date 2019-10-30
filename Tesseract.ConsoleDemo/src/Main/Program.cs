using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;
using IO.Swagger.Model;
using runner;
using Action = runner.Action;

namespace Tesseract.ConsoleDemo
{
    public class Program
    {
        public WindowScan scan = null;
        public readonly Player ego = new Player();
        public readonly StateEngine stateEngine;
        public int MaxHp = 1000;
        private long tick = 0;
        public readonly WindowScanManager windowScanManager;
        public readonly IntPtr baseHandle;
        private readonly ControlLogger roomLogger;
        public readonly Action action;
        public VerbWindow lastVerbWindow;

        public Program(IntPtr baseHandle)
        {
            this.baseHandle = baseHandle;
            this.action = new Action(this);
            this.stateEngine = new StateEngine(this, baseHandle);
            this.windowScanManager = new WindowScanManager(this);
            this.roomLogger = ControlLogger.build(this, baseHandle);
        }

        public static void Main(string[] args)
        {
            ImageManip.testOcr(args);
            //Win32GetText.GetToolTipText((IntPtr)0x47)
            AutoItX.Init();


            //var baseHandle
            List<IntPtr> handleBaseWindows = Windows.HandleBaseWindows();
            if (handleBaseWindows.Count < 1)
            {
                throw new Exception("Other App Not Running");
            }
            
            foreach (IntPtr baseHandle in handleBaseWindows)
            {
                //todo refactor into threads
                new Program(baseHandle).MainLoop();
            }

            //Action.ReadHP();


           
        }

        private void MainLoop()
        {
            Login();
//////MAIN LOOP
            while (true)
            {
                Loop();
            }
        }

        private void Login()
        {
            ego.Name = Config.get(Config.KEY_ME);

            new ApiCaller().login(ego.Name);
        }

        private void Loop()
        {
            tick++;
            Thread.Sleep(1);
            __base();


            EveryTickFirst();

            FastTick();
            CommonTick();
            RareTick();


            EveryTickLast();


            tick %= Int32.MaxValue;
        }

        const int WARMUP_TICKS = 10;

        private void EveryTickLast()
        {
            if (tick > WARMUP_TICKS)
                action.HandleNextAction(baseHandle);
        }

        private void EveryTickFirst()
        {
            ToolTips.handle(this, baseHandle);
        }

        private void FastTick()
        {
            if (tick % 3 != 0) return;
            roomLogger.LogRoom();
            scan?.tickCommon(tick, this, baseHandle);
            windowScanManager.handleScreenScan(this, baseHandle);
            HoverBox.handle(this, baseHandle);
            stateEngine.HandleStateChanges();
        }

        private void CommonTick()
        {
            if (tick % 7 != 0) return;


            if (stateEngine.InState(StateEngine.InCombat))
            {
                action.doCombat(baseHandle);
            }
            else if (stateEngine.InState(StateEngine.InCobmatAfter))
            {
                action.doLoot(baseHandle);
                action.exitCombat(baseHandle);
            }
            else
                //if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                action.handleRepairControl(baseHandle);
                action.doLoot(baseHandle);
                action.doSell(baseHandle);
            }
        }

        private void RareTick()
        {
            if (tick % 100 != 0) return;
            

            action.GetNextEvent(baseHandle);
            //

            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                if (scan == null)
                    windowScanManager.requestScreenScan(baseHandle);
            }

            if (tick % 5000 != 0) return;
            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                action.ReadHP(baseHandle);
                action.askForWeight(baseHandle);
            }

            if (tick % 10000 != 0) return;
            //

            if (stateEngine.InState(StateEngine.OutOfCombat))
            {
                windowScanManager.requestScreenScan(baseHandle);
            }
        }


        private IntPtr __base()
        {
            if (AutoItX.WinExists(baseHandle) == 0)
            {
                throw new Exception("Other App Not Running");
            }
            return baseHandle;
        }

        public long getTick()
        {
            return tick;
        }
    }
}