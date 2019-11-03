using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace runner
{
    public class Win32CursorUtils : IDisposable
    {
        public Bitmap Result { get; }
        private readonly int x, y;
        CURSORINFO ci;
        ICONINFO icInfo;
        IntPtr hicon = IntPtr.Zero;
        
        public Win32CursorUtils()
        {
            ci.cbSize = Marshal.SizeOf(ci);
            this.Result = CaptureCursor(out x, out y);
        }

        public void GetPoint(out int _x, out int _y)
        {
            _x = x;
            _y = y;
        }

        private Bitmap CaptureCursor(out int x, out int y)
        {
            Bitmap result = null;
            x = y = 0;

            try
            {
                if (GetCursorInfo(out ci))
                {
                    if (ci.flags == CURSOR_SHOWING)
                    {
                        hicon = CopyIcon(ci.hCursor);
                        if (hicon != IntPtr.Zero
                            && GetIconInfo(hicon, out icInfo))
                        {
                            result = ToBitmap(out x, out y, ci, icInfo, hicon);
                        }
                    }
                }
            }
            catch (ExternalException e)
            {
                throw new ExternalException("Cursor Getting had an issue; likely a memory leak with a not destroyed handle",e);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error encountered capturing cursor, returning null; [{0}]", e);
            }
            finally
            {
                if (hicon != IntPtr.Zero)
                    if (!DestroyIcon(hicon))
                    {
                        Console.Error.WriteLine("Unable to free Icon in finally");
                    }
                    else
                    {
                        hicon = IntPtr.Zero;
                    }
            }

            return result;
        }

        private static Bitmap ToBitmap(out int x, out int y, CURSORINFO ci, ICONINFO icInfo, IntPtr hicon)
        {
            x = ci.ptScreenPos.x - ((int) icInfo.xHotspot);
            y = ci.ptScreenPos.y - ((int) icInfo.yHotspot);
            if (x <= 0 || y <= 0) return null;
            using (var ic = Icon.FromHandle(hicon))
            {
                if (ic.Width > 1 && ic.Height > 1)
                {
                    Bitmap bmp = ic.ToBitmap();
                    return bmp; // return bmp;
                }
            }

            return null;
        }

        #region Class Variables

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        public const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool
                fIcon; // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 

            public Int32
                xHotspot; // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 

            public Int32
                yHotspot; // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 

            public IntPtr
                hbmMask; // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 

            public IntPtr hbmColor; // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize; // Specifies the size, in bytes, of the structure. 
            public Int32 flags; // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor; // Handle to the cursor. 
            public POINT ptScreenPos; // A POINT structure that receives the screen coordinates of the cursor. 
        }

        #endregion


        #region Class Functions

        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        #endregion

        public void Dispose()
        {
            this.Result?.Dispose();
            
            if (hicon != IntPtr.Zero)
                if (!DestroyIcon(hicon))
                {
                    Console.Error.WriteLine("Unable to free Icon in Despose");
                }
        }
    }
}