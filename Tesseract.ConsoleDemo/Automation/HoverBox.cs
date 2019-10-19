using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutoIt;
using runner;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class HoverBox
{
   // private static Tesseract Ocr;

    static HoverBox()
    {
       
    }

    public static void handle(IntPtr basehandle)
    {
        var hasStuff = list(basehandle);
        if (!string.IsNullOrEmpty(hasStuff))
        {
            

            VerbWindow.findWindow(basehandle, hasStuff, true);


        }
    }
//style=0x56000000
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    private static string list(IntPtr basehandle)
    {
        int l = 966, t = 48, r = 1284, b = 74;

        Rectangle rect = new Rectangle(l,t,r-l,b-t);

        var capture = ScreenCapturer.Capture(rect);
        capture = ImageManip.AdjustThreshold(capture, .9f);
        capture = ImageManip.Max(capture);
        
        return ImageManip.doOcr(capture);
    }

    
}