using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Data.Win32
{
    /// <summary>
	/// Style flags for a window.
	/// </summary>
	[Flags]
    enum WindowStyles : uint
    {
        /// <summary>
        /// Creates an overlapped window. An overlapped window usually has a caption and a border.
        /// </summary>
        WS_OVERLAPPED = 0,
        /// <summary>
        /// Creates a pop-up window. Cannot be used with the WS_CHILD style.
        /// </summary>
        WS_POPUP = 0x80000000,
        /// <summary>
        /// Creates a child window. Cannot be used with the WS_POPUP style.
        /// </summary>
        WS_CHILD = 0x40000000,
        /// <summary>
        /// Creates a window that is initially minimized. For use with the WS_OVERLAPPED style only.
        /// </summary>
        WS_MINIMIZE = 0x20000000,
        /// <summary>
        /// Creates a window that is initially visible.
        /// </summary>
        WS_VISIBLE = 0x10000000,
        /// <summary>
        /// Creates a window that is initially disabled.
        /// </summary>
        WS_DISABLED = 0x8000000,
        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a paint message, the WS_CLIPSIBLINGS style clips all other overlapped child windows out of the region of the child window to be updated. (If WS_CLIPSIBLINGS is not given and child windows overlap, when you draw within the client area of a child window, it is possible to draw within the client area of a neighboring child window.) For use with the WS_CHILD style only.
        /// </summary>
        WS_CLIPSIBLINGS = 0x4000000,
        /// <summary>
        /// Excludes the area occupied by child windows when you draw within the parent window. Used when you create the parent window.
        /// </summary>
        WS_CLIPCHILDREN = 0x2000000,
        /// <summary>
        /// Creates a window of maximum size.
        /// </summary>
        WS_MAXIMIZE = 0x1000000,
        /// <summary>
        /// Creates a window that has a title bar (implies the WS_BORDER style). Cannot be used with the WS_DLGFRAME style.
        /// </summary>
        WS_CAPTION = 0xc00000,
        /// <summary>
        /// Creates a window that has a border.
        /// </summary>
        WS_BORDER = 0x800000,
        /// <summary>
        /// Creates a window with a double border but no title.
        /// </summary>
        WS_DLGFRAME = 0x400000,
        /// <summary>
        /// Creates a window that has a vertical scroll bar.
        /// </summary>
        WS_VSCROLL = 0x200000,
        /// <summary>
        /// Creates a window that has a horizontal scroll bar.
        /// </summary>
        WS_HSCROLL = 0x100000,
        /// <summary>
        /// Creates a window that has a Control-menu box in its title bar. Used only for windows with title bars.
        /// </summary>
        WS_SYSMENU = 0x80000,
        /// <summary>
        /// Creates a window with a thick frame that can be used to size the window.
        /// </summary>
        WS_THICKFRAME = 0x40000,
        /// <summary>
        /// Specifies the first control of a group of controls in which the user can move from one control to the next with the arrow keys. All controls defined with the WS_GROUP style FALSE after the first control belong to the same group. The next control with the WS_GROUP style starts the next group (that is, one group ends where the next begins).
        /// </summary>
        WS_GROUP = 0x20000,
        /// <summary>
        /// Specifies one of any number of controls through which the user can move by using the TAB key. The TAB key moves the user to the next control specified by the WS_TABSTOP style.
        /// </summary>
        WS_TABSTOP = 0x10000,
        /// <summary>
        /// Creates a window that has a Minimize button.
        /// </summary>
        WS_MINIMIZEBOX = 0x20000,
        /// <summary>
        /// Creates a window that has a Maximize button.
        /// </summary>
        WS_MAXIMIZEBOX = 0x10000,
        /// <summary>
        /// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style.
        /// </summary>
        WS_TILED = 0,
        /// <summary>
        /// Creates a window that is initially minimized. Same as the WS_MINIMIZE style.
        /// </summary>
        WS_ICONIC = 0x20000000,
        /// <summary>
        /// Creates a window that has a sizing border. Same as the WS_THICKFRAME style.
        /// </summary>
        WS_SIZEBOX = 0x40000,

        /// <summary>
        /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_OVERLAPPEDWINDOW style.
        /// </summary>
        WS_TILEDWINDOW = 0xcf0000,
        /// <summary>
        /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles.
        /// </summary>
        WS_OVERLAPPEDWINDOW = 0xcf0000,
        /// <summary>
        /// Creates a pop-up window with the WS_BORDER, WS_POPUP, and WS_SYSMENU styles. The WS_CAPTION style must be combined with the WS_POPUPWINDOW style to make the Control menu visible.
        /// </summary>
        WS_POPUPWINDOW = 0x80880000,
        /// <summary>
        /// Same as the WS_CHILD style.
        /// </summary>
        WS_CHILDWINDOW = 0x40000000,

    }
}
