using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace runner
{
    public static class ImageManip
    {
        // Perform threshold adjustment on the image.
        public static Bitmap AdjustThreshold(Image image, float threshold)
        {
            if (image == null) return null;
            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);

            // Make the ImageAttributes object and set the threshold.
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetThreshold(threshold);

            // Draw the image onto the new bitmap
            // while applying the new ColorMatrix.
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect =
                new Rectangle(0, 0, image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }
            image.Dispose();

            // Return the result.
            return bm;
        }
        

        public static Bitmap Max(Bitmap source)
        {
            // create the negative color matrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {1, 1, 1, 0, 0},
                new float[] {1, 1, 1, 0, 0},
                new float[] {1, 1, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });


            return Transform(source, colorMatrix);
        }
        
        public static Bitmap Invert(Bitmap source)
        {
            // create the negative color matrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {-1, 0, 0, 0, 0},
                new float[] {0, -1, 0, 0, 0},
                new float[] {0, 0, -1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {1, 1, 1, 0, 1}
            });


            return Transform(source, colorMatrix);
        }

        private static Bitmap Transform(Bitmap source, ColorMatrix colorMatrix)
        {
            if (source == null) return null;
//create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(source.Width, source.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            // create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            source.Dispose();

            return newBitmap;
        }


        public static string doOcr(Bitmap capture, string charset = null)
        {
            string test;


//        ScreenCapturer.ImageSave("CornerBox", ImageFormat.Tiff, capture);
            string result = String.Empty;
            using (TesseractEngine ocr = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractOnly))
            {
                if (!string.IsNullOrEmpty(charset))
                {
                    ocr.SetVariable("tessedit_char_whitelist",
                        charset);
                }
                else
                {
                    ocr.SetVariable("tessedit_char_whitelist",
                        "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ:-");
                }

                var ms = new MemoryStream();
                capture.Save(ms, ImageFormat.Tiff);
                var bytes = ms.ToArray();
                using (var img = Pix.LoadTiffFromMemory(bytes))
//            using (var img = Pix.LoadFromFile(Environment.GetEnvironmentVariable("TEMP")+"/CornerBox.tiff"))
                {
                    using (var page = ocr.Process(img))
                    {
                        test = page.GetText();
                        if (!String.IsNullOrEmpty(test))
                        {
                            result = test.Trim();
                        }
                    }
                }
            }
            return result;
        }


        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public static bool CompareMemCmp(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }
    }
}