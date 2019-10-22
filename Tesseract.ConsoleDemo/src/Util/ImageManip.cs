using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace runner
{
    public static class ImageManip
    {
        // Perform threshold adjustment on the image.
        public static Bitmap AdjustThreshold(Image image, float threshold)
        {
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

        public static void testOcr(string[] args)
        {
            var testImagePath = "./phototest.tif";
            if (args.Length > 0)
            {
                testImagePath = args[0];
            }

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            Console.WriteLine("Text (iterator):");
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    Console.WriteLine("<BLOCK>");
                                                }

                                                Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            //Console.Write("Press any key to continue . . . ");
            //Console.ReadKey(true);
        }
    }
}