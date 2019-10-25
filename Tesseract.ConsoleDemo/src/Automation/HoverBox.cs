using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AutoIt;
using runner;
using runner.Cursor;
using Tesseract.ConsoleDemo;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class HoverBox
{
    // private static Tesseract Ocr;
    internal static bool wantItem = false;

    static HoverBox()
    {
    }

    public static VerbWindow handle(Program program, IntPtr baseHandle, bool b = false)
    {
        var hasStuff = list(baseHandle, out int type);
        if (!string.IsNullOrEmpty(hasStuff))
        {
            var findWindow = VerbWindow.findWindow(program, baseHandle, hasStuff, true, b);
            if (findWindow != null)
                findWindow.type = type;
            //else flushClick()

            return findWindow;
        }

        return null;
    }

    private static readonly int hasHp = Color.FromArgb(0, 16, 113, 9).ToArgb();
    private const int item = 6244104;


//style=0x56000000
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    private static string list(IntPtr baseHandle, out int type)
    {
        type = 0;
        ScreenCapturer.GetScale(baseHandle, out float sX, out float sY);

        int l = (int) (483 * sX),
            t = (int) (24 * sY),
            r = (int) (642 * sX),
            b = (int) (37 * sY);

        Rectangle rect = new Rectangle(l, t, r - l, b - t);
        var c = AutoItX.PixelGetColor(rect.Left + 3, rect.Top + 3);

        switch (c)
        {
            case item when !wantItem:
                return null;
            case item:
                type = item;
                return DoOcr(rect);
            default:
            {
                if (CursorUtil.isCursor(CursorUtil.hand, out int clickx, out int clickY))
                {
                    var ocr = DoOcr(rect);
                    if(!string.IsNullOrEmpty(ocr))
                        return ocr;
                    return "IMPLIED";
                }

                break;
            }
        }

        return null;
    }

    private static string DoOcr(Rectangle rect)
    {
        var capture = ScreenCapturer.Capture(rect);
        capture = ImageManip.AdjustThreshold(capture, .9f);
        capture = ImageManip.Max(capture);

        var ocr = ImageManip.doOcr(capture);
//        if (!string.IsNullOrEmpty(ocr))
//        {
//            Console.WriteLine("ocr [{0}]", ocr);
//            ScreenCapturer.ImageSave("cap_%NOW%_" + ocr + "_", ImageFormat.Tiff, capture);
//        }
//        else
//        {
//            ScreenCapturer.ImageSave("cap_%NOW%_EMPTY_", ImageFormat.Tiff, capture);
//        }

        return ocr;
    }
}