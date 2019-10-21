using System;
using System.Collections.Generic;
using AutoIt;

namespace runner
{
    /**
     * How found:	Focus
	hwnd=0x00170C7A 32bit class="Afx:00860000:0:00000000:00000000:0001002B" style=0x96000000 ex=0x0
Name:	"AliceDjinn"
ControlType:	UIA_PaneControlTypeId (0xC371)
     */
    public partial class VerbWindow
    {
        private const string texts = "StealCastLokAtFightWRrpOpenETe";
        public IntPtr hWnd;
        public List<Verb> verbs;
        public string ocrText;

        private VerbWindow(IntPtr hWnd, List<Verb> v, string ocrText)
        {
            this.hWnd = hWnd;
            this.ocrText = ocrText;
            this.verbs = v;
        }

        public static VerbWindow last = null;

        private static VerbWindow fromHandle(IntPtr hWnd, string ocrName, bool lightWeight)
        {
            if (hWnd == IntPtr.Zero) return null;

            if (lightWeight)
                return new VerbWindow(hWnd, null, ocrName);

            List<Verb> verbs = new List<Verb>();

            CaptureScreen(hWnd, out var height, out var offset, out var w, out var rect, out var capture);

            var captureHeight = capture.Height;
            FindVerbs(hWnd, captureHeight, height, capture, offset, w, rect, verbs);

            Console.WriteLine("Set VerbWindow.Last");

            return new VerbWindow(hWnd, verbs, ocrName);
        }


        public static VerbWindow findWindow(IntPtr baseHandle, String mousedOver, bool allowClick, bool lightWeight)
        {
            try
            {
                var window = _findWindow(baseHandle, mousedOver, allowClick);

                var verbWindow = fromHandle(window, mousedOver, lightWeight);
                if (!lightWeight)
                    last = verbWindow;
                else last = null;
                return verbWindow;
            }
            catch (Exception)
            {
            }

            return null;
        }

        const string VerbClass = "Afx:00860000:0:00000000:00000000:0001002B";

        public static void click(Verb verb)
        {
            int x = verb.rect.X + 5;
            int y = verb.rect.Y + 5;


            AutoItX.MouseMove(x, y, 1);
            //Console.WriteLine("Moved to ({0},{1})", x, y);
            AutoItX.MouseClick("LEFT", x, y, 1, 1);
        }
    }
}