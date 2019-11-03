using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AutoIt;
using runner;
using Tesseract.ConsoleDemo;
using static runner.ExpectedToolTip;
using static runner.Win32;

namespace runner
{
    /// <summary>Contains functionality to get all the open windows.</summary>
    internal class ToolTips : User32Delegate
    {
        private static ExpectedToolTip _expectedToolTip = Other;

        public static void setExpected(ExpectedToolTip expectedToolTip)
        {
            _expectedToolTip = expectedToolTip;
        }

        public static string handle(Program program,  IntPtr baseHandle)
        {
            var all = list(baseHandle);
            foreach (var keyValuePair in all)
            {
                var winGetState = keyValuePair.Value;
                if ((winGetState & 2) == 0) continue;
                var text = HandleVisible(program, baseHandle, keyValuePair);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }

            //Console.WriteLine("State : {0}", winGetState);
            return null;
        }

        private static string HandleVisible(Program program, IntPtr baseHandle, KeyValuePair<IntPtr, int> keyValuePair)
        {
//Console.WriteLine("Visi");
            //ScreenCapturer.CaptureAndSave("alices"+hWnd,hWnd);
            //TODO HANDLE BETTER
            var capture = ScreenCapturer.Capture(keyValuePair.Key);
            capture = ImageManip.AdjustThreshold(capture, .3f);
//            ScreenCapturer.ImageSave("currentThingRaw", ImageFormat.Tiff, capture);
            capture = ImageManip.Invert(capture);
            capture = ImageManip.Max(capture);
//            ScreenCapturer.ImageSave("currentThing1", ImageFormat.Tiff, capture);


            return HandleCapture(program, baseHandle, capture);
        }

        private static string HandleCapture(Program program, IntPtr baseHandle, Bitmap capture)
        {
            String text = String.Empty;
            String lookingFor = "None";
            switch (_expectedToolTip)
            {
                case Mana:
                    lookingFor = "Mana";
                    text = ImageManip.doOcr(capture, "1234567890");
                    if (text.Contains(" ") && text.IndexOf(" ") == text.LastIndexOf(" ") && text.Length < 10)
                    {
                        text.Remove(text.IndexOf(" "));
                    }

                    if (!text.Contains(" "))
                    {
                        if (int.TryParse(text, out var current))
                        {
                            Console.WriteLine("Mana is at [{0}]", current);

                            setExpected(Other);
                            program.action.ReadManaComplete(current);
                        }
                    }

                    break;
                case Health:
                    lookingFor = "HP";
                    text = HandleHealth(program, baseHandle, capture);


                    break;
                case Buttons:
                    lookingFor = "Button";
                    text = ImageManip.doOcr(capture);
                    break;

                case ExpectedToolTip.Inventory:
                case Other:
                default:
                    break;
            }

//            if (!string.IsNullOrEmpty(text))
//            {
//                Console.WriteLine("TTL [{0}] Looking for [{1}]", text, lookingFor);
//            }

            return text;
        }

        private static string HandleHealth(Program program, IntPtr baseHandle, Bitmap capture)
        {
            string text;
            text = ImageManip.doOcr(capture, "0123456789");
            var splits = text.Split(new string[] {"016 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 301 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 201 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 701 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 01 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 011 "}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 016"}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {" 01"}, StringSplitOptions.RemoveEmptyEntries);
            if(splits.Length!=2) splits = text.Split(new string[] {"01 "}, StringSplitOptions.RemoveEmptyEntries);
            
            if (splits.Length == 2)
                if (int.TryParse(splits[0].Trim(), out var current))
                    if (int.TryParse(splits[1].Trim(), out var max))
                    {
                        if (current > max)
                            current = max;

                        Console.WriteLine("Hp is at [{0}] of [{1}]", current, max);

                        setExpected(Other);
                        program.action.ReadHpComplete(baseHandle, current, max);
                    }

            return text;
        }


        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        private static IDictionary<IntPtr, int> list(IntPtr baseHandle)
        {
            GetWindowThreadProcessId(baseHandle, out var main);
            IntPtr shellWindow = GetShellWindow();
            Dictionary<IntPtr, int> windows = new Dictionary<IntPtr, int>();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                //if (!IsWindowVisible(hWnd)) return true;

                var clz = Win32GetText.getClassName(hWnd);
                GetWindowThreadProcessId(hWnd, out var thread);

                if ("tooltips_class32".Equals(clz)
                    && thread == main)
                {
                    int _h = (int) hWnd;

                    MakeQuick(hWnd);

                    var winGetState = AutoItX.WinGetState(hWnd);
                    windows[hWnd] = winGetState;
                }

                return true;
            }, 0);

            return windows;
        }

        private static void MakeQuick(IntPtr hWnd)
        {
            if (SendMessage(hWnd, TTM_SETDELAYTIME, TTDT.TTDT_INITIAL, 1) != IntPtr.Zero)
            {
                Console.WriteLine("Added No Delay");
            }
        }

        public static void moveOver(IntPtr baseHandle, ExpectedToolTip expectedToolTip, bool click = false)
        {
            int x, y;
            switch (expectedToolTip)
            {
                case Health:
                    x = 500;
                    y = 350;
                    break;
                case Mana:
                    x = 590;
                    y = 350;
                    break;
                case ExpectedToolTip.Inventory:
                    x = 500;
                    y = 375;
                    break;
                case Spells:
                    x = 590;
                    y = 375;
                    break;
                default: throw new NotImplementedException();
                
            }
            AutoItX.WinActivate(baseHandle);
            if (click)
            {
                MouseManager.MouseClick(baseHandle,x,y);
            }
            else
            {
                MouseManager.MouseMoveUnScaled(baseHandle,x,y);
            }
        }
    }
}