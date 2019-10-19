using System;
using System.Drawing;
using System.IO;
using runner;
using Tesseract;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class HoverBox
{
   // private static Tesseract Ocr;

    static HoverBox()
    {
       
    }

    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    public static void list(IntPtr basehandle)
    {
        int l = 966, t = 48, r = 1284, b = 74;
        Rectangle rect = new Rectangle(l,t,r-l,b-t);

        
        var capture = ScreenCapturer.Capture(rect);
        using (TesseractEngine ocr = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractOnly))
        {
            var ms = new MemoryStream();
            capture.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
            var bytes = ms.ToArray();
            using (var img = Pix.LoadTiffFromMemory(bytes))
            {
                using (var page = ocr.Process(img))
                {
                    Console.WriteLine(page.GetText());
                }
            }
        }
        
        
        Console.WriteLine("OCR START");
       
        Console.WriteLine("OCR END");
       

    }
}