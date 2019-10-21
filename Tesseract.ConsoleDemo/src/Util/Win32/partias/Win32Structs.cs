using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace runner
{
    public static partial class Win32
    {
        
        public enum ScrollBarCommands {

            SB_LINEUP       = 0,
            SB_LINELEFT     = 0,
            SB_LINEDOWN     = 1,
            SB_LINERIGHT    = 1,
            SB_PAGEUP       = 2,
            SB_PAGELEFT     = 2,
            SB_PAGEDOWN     = 3,
            SB_PAGERIGHT    = 3,
            SB_THUMBPOSITION    = 4,
            SB_THUMBTRACK       = 5,
            SB_TOP          = 6,
            SB_LEFT         = 6,
            SB_BOTTOM       = 7,
            SB_RIGHT        = 7,
            SB_ENDSCROLL    = 8
        }       
        
        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public int mask;
            public int cxy;
            public IntPtr pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public int fmt;
            public IntPtr lParam;
            public int iImage;

            public int iOrder;

            //if (_WIN32_IE >= 0x0500)
            public int type;
            public IntPtr pvFilter;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class HDHITTESTINFO
        {
            public int pt_x;
            public int pt_y;
            public int flags;
            public int iItem;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVCOLUMN
        {
            public int mask;
            public int fmt;
            public int cx;
            [MarshalAs(UnmanagedType.LPTStr)] public string pszText;
            public int cchTextMax;

            public int iSubItem;

            // These are available in Common Controls >= 0x0300
            public int iImage;
            public int iOrder;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVFINDINFO
        {
            public int flags;
            public string psz;
            public IntPtr lParam;
            public int ptX;
            public int ptY;
            public int vkDirection;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVHITTESTINFO
        {
            public int pt_x;
            public int pt_y;
            public int flags;
            public int iItem;
            public int iSubItem;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)] public string pszText;
            public int cchTextMax;
            public int iImage;

            public IntPtr lParam;

            // These are available in Common Controls >= 0x0300
            public int iIndent;

            // These are available in Common Controls >= 0x056
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        };

        /// <summary>
        /// Notify m header structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHEADER
        {
            public NMHDR nhdr;
            public int iItem;
            public int iButton;
            public IntPtr pHDITEM;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMLISTVIEW
        {
            public NMHDR hdr;
            public int iItem;
            public int iSubItem;
            public int uNewState;
            public int uOldState;
            public int uChanged;
            public IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMLVFINDITEM
        {
            public NMHDR hdr;
            public int iStart;
            public LVFINDINFO lvfi;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct RECT
        {
            public uint left;
            public uint top;
            public uint right;
            public uint bottom;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 44, CharSet = CharSet.Ansi)]
        public struct TOOLINFO
        {
            public int size;
            public uint flags;
            public UIntPtr hwnd;
            public UIntPtr id;
            public RECT rect;
            public UIntPtr hinst;

            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources",
                Justification = "Not a security threat since its used by designer scenarios only")]
            public IntPtr text;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct TOOLTIPTEXT
        {
            public NMHDR hdr;
            public string lpszText;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szText;

            public IntPtr hinst;
            public int uFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }
    }
}