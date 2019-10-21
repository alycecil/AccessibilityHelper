using System;
using System.Drawing;
using AutoIt;
using runner;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class HoverBox
{
    // private static Tesseract Ocr;

    static HoverBox()
    {
    }

    public static VerbWindow handle(IntPtr basehandle, bool b = false)
    {
        var hasStuff = list(basehandle);
        if (!string.IsNullOrEmpty(hasStuff))
        {
            return VerbWindow.findWindow(basehandle, hasStuff, true, b);
        }

        return null;
    }

    private static Color wanted = Color.FromArgb(0, 16, 113, 9);

//style=0x56000000
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    private static string list(IntPtr basehandle)
    {
        ScreenCapturer.GetScale(basehandle, out float sX, out float sY);

        int l = (int) (483 * sX),
            t = (int) (24 * sY),
            r = (int) (642 * sX),
            b = (int) (37 * sY);

        Rectangle rect = new Rectangle(l, t, r - l, b - t);

        var c = AutoItX.PixelGetColor(rect.Left + 3, rect.Top + 3);
        var c2 = AutoItX.PixelGetColor(rect.Left + 5, rect.Top + 5);
        var c3 = AutoItX.PixelGetColor(rect.Left + 17, rect.Top + 4);
        
        if (!c2.Equals(c) || !c2.Equals(c3))
            return null;

        int argb = wanted.ToArgb();
        if (!c.Equals(argb))
        {
            if(c != 497579 && c != 6244104)
                Console.WriteLine("Got Color : {0}", c); 
            //item color : 6244104
            return null;
            
        }

        var capture = ScreenCapturer.Capture(rect);
        capture = ImageManip.AdjustThreshold(capture, .9f);
        capture = ImageManip.Max(capture);
        return ImageManip.doOcr(capture);
    }
}