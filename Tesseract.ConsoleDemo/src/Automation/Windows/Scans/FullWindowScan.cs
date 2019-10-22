using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class FullWindowScan : WindowScan 
    {
        public FullWindowScan(List<Thing> things) : base(things)
        {
        }

        //Does Nothing, scanned already
        public override void tickCommon(long tick)
        {}
        
        public static WindowScan scanScreen(IntPtr hWnd)
        {
//            if (string.IsNullOrEmpty(verb)) return null;
//
//            verb = verb.ToLower();`                                                                    1
            List<Thing> things = new List<Thing>();
            ScreenCapturer.GetScale(hWnd, out float sX, out float sY);

            Console.WriteLine(DateTime.Now);
            GetConfig(out var START_X, 
                out var END_X, 
                out var STEP_X, 
                out var START_Y, 
                out var STEP_Y,
                out var END_y);

            
            Random r = new Random();
            for (int x = START_X; x < END_X; x += STEP_X)
            {
                x += (int)(r.Next() % STEP_X * .25*(r.Next() % 5 -3));
                
                for (int y = START_Y; y < END_y; y += STEP_Y)
                {
                    y += (int) (r.Next() % STEP_Y * .25 * (r.Next() % 5 - 3));
                    
                    var scaledX = (int) (x * sX);
                    var scaledY = (int) (y * sY);
                    
                    AutoItX.MouseMove(scaledX, scaledY, 1);
                    Thread.Sleep(TimeSpan.FromMilliseconds(0.8));
                    var h = HoverBox.handle(hWnd, true);
                    
                    if (h == null) continue;
                    var name = Win32GetText.GetControlText(h.hWnd);
                    if (!string.IsNullOrEmpty(name))
                    {
                        things.Add(new Thing(scaledX,scaledY,h,name));
                        y += 50;
                        x += 7;
                    
                    
                        //DISMISS
                        AutoItX.MouseClick("middle", 40, 40, 1, 0);
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
            return new FullWindowScan(things);
        }
    }
}