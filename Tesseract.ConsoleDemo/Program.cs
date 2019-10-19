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
                //new WindowHandleInfo(basehandle).GetAllChildHandles();
                //ToolTips.list(basehandle);
                //yield
                Thread.Sleep(20);

                __base();


                ToolTips.handle(basehandle);
                HoverBox.handle(basehandle);

                roomLogger.LogRoom();

                if (tick % 5 == 0)
                {
                    stateEngine.HandleStateChanges(basehandle);
                }           
                
                if (tick % 10 == 0)
                {
                    Action.doCombat(basehandle);
                    Action.doLoot(basehandle);
                    Action.exitCombat(basehandle);
                    
                }            
                


                if (tick % 100000 == 0)
                {
                    Action.askForWeight();
                }
                Action.handleNextAction();
                tick %= Int32.MaxValue;
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