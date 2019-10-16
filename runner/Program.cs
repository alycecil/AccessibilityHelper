using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AutoIt;
using IO.Swagger.Model;

namespace runner
{
    class Program
    {
        public static Player ego = new Player();

        public static void Main(string[] args)
        {
            //Win32GetText.GetToolTipText((IntPtr)0x47)
            AutoItX.Init();
            ego.Name = Config.get(Config.KEY_ME);

            var stateEngine = new StateEngine();
            var basehandle = __base();

            var chat = Windows.getChatRoom(basehandle);
            var roomLogger = new ControlLogger(basehandle, chat);

            DumpWindowsHandles.windowsHandles(args);
            
            //https://stackoverflow.com/questions/9665579/setting-up-hook-on-windows-messages?lq=1
                
            
            

            //////MAIN LOOP
            while (true)
            {
                //new WindowHandleInfo(basehandle).GetAllChildHandles();

                //yield
                Thread.Sleep(100);

                __base();

                stateEngine.HandleStateChanges(basehandle);
                Action.ReadHP();


                roomLogger.LogRoom();
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