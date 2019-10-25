using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace runner.Cursor
{
    public class CursorUtil
    {
        public static Bitmap left = load("./left.tiff");
        public static Bitmap right = load("./right.tiff");
        public static Bitmap up = load("./up.tiff");
        public static Bitmap down = load("./down.tiff");
        public static Bitmap hand = load("./hand.tiff");
        static Bitmap load(String desired)
        {
            return new Bitmap(desired);
        }

        public static bool isCursor(Bitmap desired, out int x1, out int x2)
        {
            var cursor = Win32CursorUtils.CaptureCursor(out x1, out x2);

            var result = ImageManip.CompareMemCmp(desired, cursor);
            //Console.WriteLine("Oh Nice [{0}]", result);
            return result;
        }
        
        
    }
}