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

        public static void handle(IntPtr baseHandle)
        {
            var combat = Windows.getInCombat();
            if (combat == IntPtr.Zero) return;

            ScreenCapturer.GetBounds(combat, out var bounds);
            //ScreenCapturer.CaptureAndSave("RCombat", combat);
            ToolTips.setExpected(ExpectedTT.Buttons);


            Dictionary<string, Point> spots = GetClicks(baseHandle, bounds);

            //TODO If Action Hooks
            if (Config.autoGuard() && (
                    Program.ego?.Hp?.Value == null 
                    || Program.ego.Hp.Value * 1.1 > Program.MaxHp
                    )
                )
            {
                string tt = GuardTT;
                foreach (var spot in spots)
                {
                    string spotKey = spot.Key;
                    if (spotKey?.StartsWith(tt) == true)
                    {
                        AutoItX.MouseClick("LEFT",bounds.X + spot.Value.X, bounds.Y + spot.Value.Y, 1, 1);        
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

        private static Dictionary<string, Point> clickingLocations = new Dictionary<string, Point>();

        private static Dictionary<string, Point> GetClicks(IntPtr baseHandle, Rectangle bounds)
        {
            if (clickingLocations.Count > 0)
            {
                return clickingLocations;
            }

            var seenBefore = new HashSet<string>();
            for (int x = 20; x < bounds.Width; x += 40)
            {
                for (int y = 10; y < bounds.Height; y += 30)
                {
                    AutoItX.MouseMove(bounds.X + x, bounds.Y + y, 1);
                    var tt = ToolTips.handle(baseHandle);
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