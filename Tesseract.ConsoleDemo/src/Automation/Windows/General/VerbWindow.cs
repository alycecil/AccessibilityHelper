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
        public int type;

        public static VerbWindow findWindow(IntPtr baseHandle,
            String mousedOver,
            bool allowClick = false, 
            bool lightWeight = false
        ){
            try
            {
                var window = _findWindow(baseHandle, mousedOver, allowClick);

                var verbWindow = fromHandle(window, mousedOver, lightWeight);
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

        const string VerbClass = "Afx:";//...


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