using System;
using System.Collections.Generic;
using AutoIt;
using Tesseract.ConsoleDemo;

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
        public const string VerbClass = "Afx:";//...
        public const string texts = "StealCastLokAtFightWRrpOpenETe";
        public IntPtr hWnd;
        public List<Verb> verbs;
        public string ocrText;

        internal VerbWindow(IntPtr hWnd, List<Verb> v, string ocrText)
        {
            this.hWnd = hWnd;
            this.ocrText = ocrText;
            this.verbs = v;
        }

        public static VerbWindow last = null;
        public int type;

        public static VerbWindow findWindow(Program program, IntPtr baseHandle,
            String mousedOver,
            bool allowClick = false, 
            bool lightWeight = false
        ){
            try
            {
                var window = VerbWindowHelper.findWindow(baseHandle, mousedOver, allowClick);

                var verbWindow = VerbWindowHelper.fromHandle(program, baseHandle, window, mousedOver, lightWeight);
                if (!lightWeight)
                    last = verbWindow;
                else last = null;
                return verbWindow;
            }
            catch (Exception wtfHappened)
            {
                Console.Error.WriteLine(wtfHappened);
            }

            return null;
        }


        public void dismiss()
        {
            if (hWnd != IntPtr.Zero)
            {
                Console.WriteLine("Dismissing");
                AutoItX.WinClose(hWnd);
            }
            else
            {
                Console.WriteLine("Not Happening");
            }
        }
    }
}