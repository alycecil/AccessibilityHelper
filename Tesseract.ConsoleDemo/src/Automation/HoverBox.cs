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
        var hasStuff = list(basehandle, out int type);
        if (!string.IsNullOrEmpty(hasStuff))
        {
            var findWindow = VerbWindow.findWindow(basehandle, hasStuff, true, b);
            if(findWindow!=null)
                findWindow.type = type;
            

            return findWindow;
        }

        return null;
    }

    private static readonly int hasHp = Color.FromArgb(0, 16, 113, 9).ToArgb();
    private const int item = 6244104;


//style=0x56000000
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    private static string list(IntPtr basehandle, out int type)
    {
        type = 0;
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

        if (c.Equals(hasHp))
        {
            type = hasHp;
            return DoOcr(rect);
        }
        else if (c.Equals(item))
        {
            type = item;
            return DoOcr(rect);
        }
        else if (c != 497579)
        {
            //Console.WriteLine("Got Color : {0}", c);
            //item color : 6244104
        }
        return null;
    }

    private static string DoOcr(Rectangle rect)
    {
        var capture = ScreenCapturer.Capture(rect);
        capture = ImageManip.AdjustThreshold(capture, .9f);
        capture = ImageManip.Max(capture);
        return ImageManip.doOcr(capture);
    }
}