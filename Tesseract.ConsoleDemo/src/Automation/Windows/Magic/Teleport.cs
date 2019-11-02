using System;
using System.Threading;
using AutoIt;
using IO.Swagger.Model;
using runner;

namespace Tesseract
{
    public static class Teleport
    {
        public static void close(IntPtr baseHandle)
        {
            var w = Windows.getTeleport(baseHandle);
            if (w == IntPtr.Zero) return;

            AutoItX.WinClose(w);

        }
        public static bool teleport(IntPtr baseHandle, Event curEvent)
        {
            if (curEvent.Targets.Count != 1)
            {
                throw new NotImplementedException();
            }

            var target = curEvent.Targets[0].Name;
            var window = IntPtr.Zero;
            //wait for render
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                window = Windows.getTeleport(baseHandle);
                AutoItX.WinMove(window, 0, 0);

                if (window != IntPtr.Zero) break;
            }

            
            if (window == IntPtr.Zero) return false;

            string keyX = Config.KEY_TELEPORT + target + "X",
                keyY = Config.KEY_TELEPORT + target + "Y";

            if (Config.click(baseHandle, keyX, keyY))
            {
                return true;
            }
            else
            {
                throw new NotImplementedException();
            }
            
            
        }
    }
}