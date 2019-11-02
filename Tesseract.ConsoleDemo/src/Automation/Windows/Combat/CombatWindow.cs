using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using AutoIt;
using IO.Swagger.Model;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class CombatWindow
    {
        private static string GuardTT = "Passive attack"; //...
        private static string AttackTT = "Normal attack with"; //...
        private static string ChargeTT = "Aggressive attack with"; //...
        private static string CastTT = "Cast"; //...
        private static string FleeTT = "Attempt to run"; //...

        public void handle(Program program, IntPtr baseHandle)
        {
            var combat = Windows.getInCombat(baseHandle);
            if (combat == IntPtr.Zero) return;

            ScreenCapturer.GetBounds(combat, out var bounds);
            //ScreenCapturer.CaptureAndSave("RCombat", combat);
            ToolTips.setExpected(ExpectedToolTip.Buttons);


            Dictionary<string, Point> spots = GetClicks(program, baseHandle, bounds);

            //TODO If Action Hooks
            if (Config.autoGuard() && (
                    program.ego?.Hp?.Value == null 
                    || program.ego.Hp.Value * 1.1 > program.MaxHp
                    )
                )
            {
                string tt = GuardTT;
                foreach (var spot in spots)
                {
                    string spotKey = spot.Key;
                    if (spotKey?.StartsWith(tt) == true)
                    {
                        MouseManager.MouseClick(baseHandle,"LEFT",bounds.X + spot.Value.X, bounds.Y + spot.Value.Y, 1, 1);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("saw [{0}] looking for [{1}]",spotKey,tt);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not sure what to do");
            }
        }

        private Dictionary<string, Point> clickingLocations = new Dictionary<string, Point>();

        private Dictionary<string, Point> GetClicks(Program program, IntPtr baseHandle, Rectangle bounds)
        {
            if (clickingLocations.Count > 0)
            {
                return clickingLocations;
            }
            ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);
            var seenBefore = new HashSet<string>();
            for (int x = 20; x < bounds.Width; x += (int)(40*sX))
            {
                for (int y = 20; y < bounds.Height; y += (int)(21*sY))
                {
                    MouseManager.MouseMoveAbsolute(baseHandle,bounds.X + x, bounds.Y + y, 1);
                    var tt = ToolTips.handle(program, baseHandle);
                    if (!string.IsNullOrEmpty(tt) && !seenBefore.Contains(tt))
                    {
                        seenBefore.Add(tt);
                        Console.WriteLine(tt);
                        clickingLocations.Add(tt, new Point(x, y));
                    }
                }
            }

            return clickingLocations;
        }
    }
}