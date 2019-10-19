using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;
using Tesseract.ConsoleDemo;
using static HoverBox;

namespace runner
{
    public class Thing
    {
        public Thing(int x, int y, VerbWindow verbWindow, string name)
        {
            this.x = x;
            this.y = y;
            this.verbWindow = verbWindow;
            this.name = name;
        }

        public int x, y;
        public VerbWindow verbWindow;
        public string name;
    }
    public class WindowScan
    {
        public List<Thing> things;

        public WindowScan(List<Thing> things)
        {
            this.things = things;
        }

        public static WindowScan scanScreen(IntPtr hWnd)
        {
//            if (string.IsNullOrEmpty(verb)) return null;
//
//            verb = verb.ToLower();`                                                                    1
            List<Thing> things = new List<Thing>();
            ScreenCapturer.GetScale(hWnd, out float sX, out float sY);

            Console.WriteLine(DateTime.Now);
            int START_X = 40;
            int END_X = 620;
            int STEP_X = 27;
            int START_Y = 90;
            int STEP_Y = 13;
            int END_y = 288;

            if (Program.stateEngine.InState(StateEngine.InCombat))
            {
                START_X = 10;
                END_X = 640;
                STEP_X = 12;

                START_Y = 20;
            }
            for (int x = START_X; x < END_X; x += STEP_X)

            {
                
                for (int y = START_Y; y < END_y; y += STEP_Y)
                {
                    var scaledX = (int) (x * sX);
                    var scaledY = (int) (y * sY);
                    
                    AutoItX.MouseMove(scaledX, scaledY, 1);
                   Thread.Sleep(TimeSpan.FromMilliseconds(0.8));
                    var h = handle(hWnd, true);
                    
                    if (h == null) continue;
                    var name = Win32GetText.GetControlText(h.hWnd);
                    if (!string.IsNullOrEmpty(name))
                    {
                        things.Add(new Thing(scaledX,scaledY,h,name));
                        y += 50;
                    
                    
                        //DISMISS
                        AutoItX.MouseClick("right", 0, 0, 1, 0);
                    }
                    else
                    {
                        Console.WriteLine("TooFast, last a screen item [{0}] @ [{1},{2}], consider slowing down", h.ocrText,scaledX, scaledY );
                    }
//                    if (h?.verbs == null) continue;
//                    //todo skip on color
                    
//                    //TODO skip if name same as last
//                    foreach (var hVerb in h.verbs)
//                    {
//                        Console.WriteLine("Found Verb on [{0}] - [{1}]", hVerb?.what,name);
//                        if (hVerb.what.ToLower().Equals(verb))
//                        {
//                            int x2 = hVerb.rect.Left + hVerb.rect.Width / 2;
//                            int y2 = hVerb.rect.Top + hVerb.rect.Height / 2;
//                            AutoItX.MouseClick("LEFT", x2, y2);
//                            return null;
//                        }
//                        
//                        
//                    }

                   
                }
            }

            //TODO
            Console.WriteLine("D0ne at {0}",DateTime.Now);
            return new WindowScan(things);
        }
    }
}