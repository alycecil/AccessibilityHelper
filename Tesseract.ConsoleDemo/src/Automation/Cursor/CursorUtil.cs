using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace runner.Cursor
{
    public class CursorUtil
    {
        public static readonly Bitmap left = load("./left.tiff");
        public static readonly Bitmap right = load("./right.tiff");
        public static readonly Bitmap up = load("./up.tiff");
        public static readonly Bitmap down = load("./down.tiff");
        public static readonly Bitmap hand = load("./hand.tiff");

        static Bitmap load(String desired)
        {
            return new Bitmap(desired);
        }

        public static bool isCursor(Bitmap desired, out int x1, out int x2)
        {
            using (var icon = new Win32CursorUtils())
            {
                icon.GetPoint(out x1, out x2);
                using (var cursor = icon.Result)
                {
                    var result = ImageManip.CompareMemCmp(desired, cursor);
                    //Console.WriteLine("Oh Nice [{0}]", result);
                    return result;
                }
            }
        }
    }
}