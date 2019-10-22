using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AutoIt;
using runner;
using static runner.ExpectedTT;
using static runner.Win32;

namespace runner
{
    public enum ExpectedTT
    {
        Health,
        Mana,
        Inventory,
        Buttons,
        Other,
        None
    }


    public static class TTDT
    {
        public const int TTDT_AUTOMATIC = 0;
        public const int TTDT_AUTOPOP = 2;
        public const int TTDT_INITIAL = 3;
        public const int TTDT_RESHOW = 1;
    }

    /// <summary>Contains functionality to get all the open windows.</summary>
    public static class ToolTips
    {
        private static ExpectedTT _expectedTt = Other;

        public static void setExpected(ExpectedTT expectedTt)
        {
            _expectedTt = expectedTt;
        }

        public static string handle(IntPtr basehandle)
        {
            var all = list(basehandle);
            foreach (var keyValuePair in all)
            {
                var winGetState = keyValuePair.Value;
                if ((winGetState & 2) == 0) continue;
                var text = HandleVisible(keyValuePair);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }

            //Console.WriteLine("State : {0}", winGetState);
            return null;
        }

        private static string HandleVisible(KeyValuePair<IntPtr, int> keyValuePair)
        {
//Console.WriteLine("Visi");
            //ScreenCapturer.CaptureAndSave("alices"+hWnd,hWnd);
            //TODO HANDLE BETTER
            var capture = ScreenCapturer.Capture(keyValuePair.Key);
            capture = ImageManip.AdjustThreshold(capture, .3f);
            ScreenCapturer.ImageSave("currentThingRaw", ImageFormat.Tiff, capture);
            capture = ImageManip.Invert(capture);
            capture = ImageManip.Max(capture);
            ScreenCapturer.ImageSave("currentThing1", ImageFormat.Tiff, capture);


            return HandleCapture(capture);
        }

        private static string HandleCapture(Bitmap capture)
        {
            String text = String.Empty;
            String lookingFor = "None";
            switch (_expectedTt)
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
                            Action.ReadManaComplete(current);
                        }
                    }

                    break;
                case Health:
                    lookingFor = "HP";
                    text = HandleHealth(capture);


                    break;
                case Buttons:
                    lookingFor = "Button";
                    text = ImageManip.doOcr(capture);
                    break;

                case Inventory:
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

        private static string HandleHealth(Bitmap capture)
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
                        Action.ReadHPComplete(current, max);
                    }

            return text;
        }


        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        private static IDictionary<IntPtr, int> list(IntPtr basehandle)
        {
            GetWindowThreadProcessId(basehandle, out var main);
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

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);


        public static void moveOver(ExpectedTT expectedTt)
        {
            switch (expectedTt)
            {
                case Health:
                    move(500, 350);
                    break;
                case Mana:
                    move(590, 350);
                    break;
                case Inventory:
                    move(500, 375);
                    break;
                
            }
        }
    }
}