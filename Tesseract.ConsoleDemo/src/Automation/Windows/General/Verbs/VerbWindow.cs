using System;
using System.Collections.Generic;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    /**
     * How found:	Focus
	hwnd=0x00170C7A 32bit class="Afx:00860000:0:00000000:00000000:0001002B" style=0x96000000 ex=0x0
Name:	"WhatEver"
ControlType:	UIA_PaneControlTypeId (0xC371)
     */
    public class VerbWindow
    {
        public const string VerbClass = "Afx:"; //...
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

        public int type;

        public static VerbWindow FindWindow(Program program, IntPtr baseHandle,
            String mousedOver,
            bool allowClick = false,
            bool lightWeight = false
        )
        {
            try
            {
                var window = VerbWindowHelper.findWindow(baseHandle, mousedOver, allowClick);

                var verbWindow = VerbWindowHelper.fromHandle(program, baseHandle, window, mousedOver, lightWeight);
                program.lastVerbWindow = verbWindow;
                return verbWindow;
            }
            catch (Exception wtfHappened)
            {
                Console.Error.WriteLine(wtfHappened);
            }

            return null;
        }

        public static void Click(IntPtr baseHandle, Verb verb)
        {
            verb.click(baseHandle, out var x, out var y);
        }

        public void Dismiss()
        {
            if (hWnd != IntPtr.Zero)
            {
                Console.WriteLine("Dismissing");
                try
                {
                    AutoItX.WinClose(hWnd);
                }
                catch (Exception)
                {
                    // ignored
                }

                //MouseManager.MouseClick(hWnd, -10,-10);
            }
            else
            {
                Console.WriteLine("Not Happening");
            }
        }
    }
}