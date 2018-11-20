using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Data.Win32
{
    /// <summary>
	/// Extended style flags for a window.
	/// </summary>
	[Flags]
    enum WindowStylesEx : uint
    {
        /// <summary>
        /// Designates a window with a double border that may (optionally) be created with a title bar when you specify the WS_CAPTION style flag in the dwStyle parameter.
        /// </summary>
        WS_EX_DLGMODALFRAME = 1,
        /// <summary>
        ///  Specifies that a child window created with this style will not send the WM_PARENTNOTIFY message to its parent window when the child window is created or destroyed.
        /// </summary>
        WS_EX_NOPARENTNOTIFY = 4,
        /// <summary>
        /// Specifies that a window created with this style should be placed above all nontopmost windows and stay above them even when the window is deactivated. An application can use the SetWindowPos member function to add or remove this attribute.
        /// </summary>
        WS_EX_TOPMOST = 8,
        /// <summary>
        /// Specifies that a window created with this style accepts drag-and-drop files.
        /// </summary>
        WS_EX_ACCEPTFILES = 0x10,
        /// <summary>
        /// Specifies that a window created with this style is to be transparent. That is, any windows that are beneath the window are not obscured by the window. A window created with this style receives WM_PAINT messages only after all sibling windows beneath it have been updated.
        /// </summary>
        WS_EX_TRANSPARENT = 0x20,
        /// <summary>
        /// Creates an MDI child window.
        /// </summary>
        WS_EX_MDICHILD = 0x40,
        /// <summary>
        /// Creates a tool window, which is a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the task bar or in the window that appears when the user presses ALT+TAB.
        /// </summary>
        WS_EX_TOOLWINDOW = 0x80,
        /// <summary>
        /// Specifies that a window has a border with a raised edge.
        /// </summary>
        WS_EX_WINDOWEDGE = 0x100,
        /// <summary>
        /// Specifies that a window has a 3D look — that is, a border with a sunken edge.
        /// </summary>
        WS_EX_CLIENTEDGE = 0x200,
        /// <summary>
        ///  Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
        /// </summary>
        WS_EX_CONTEXTHELP = 0x400,
        /// <summary>
        /// Gives a window generic right-aligned properties. This depends on the window class.
        /// </summary>
        WS_EX_RIGHT = 0x1000,
        /// <summary>
        /// Gives window generic left-aligned properties. This is the default.
        /// </summary>
        WS_EX_LEFT = 0,
        /// <summary>
        /// Displays the window text using right-to-left reading order properties.
        /// </summary>
        WS_EX_RTLREADING = 0x2000,
        /// <summary>
        /// Displays the window text using left-to-right reading order properties. This is the default.
        /// </summary>
        WS_EX_LTRREADING = 0,
        /// <summary>
        /// Places a vertical scroll bar to the left of the client area.
        /// </summary>
        WS_EX_LEFTSCROLLBAR = 0x4000,
        /// <summary>
        /// Places a vertical scroll bar (if present) to the right of the client area. This is the default.
        /// </summary>
        WS_EX_RIGHTSCROLLBAR = 0,
        /// <summary>
        /// Allows the user to navigate among the child windows of the window by using the TAB key.
        /// </summary>
        WS_EX_CONTROLPARENT = 0x10000,
        /// <summary>
        /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        WS_EX_STATICEDGE = 0x20000,
        /// <summary>
        /// Forces a top-level window onto the taskbar when the window is visible.
        /// </summary>
        WS_EX_APPWINDOW = 0x40000,
        /// <summary>
        /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles
        /// </summary>
        WS_EX_OVERLAPPEDWINDOW = 0x300,
        /// <summary>
        /// Combines the WS_EX_WINDOWEDGE and WS_EX_TOPMOST styles.
        /// </summary>
        WS_EX_PALETTEWINDOW = 0x188,
        WS_EX_LAYERED = 0x80000,
        WS_EX_NOINHERITLAYOUT = 0x100000,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,


        WS_EX_LAYOUTRTL = 0x400000,
        WS_EX_COMPOSITED = 0x2000000,
        WS_EX_NOACTIVATE = 0x8000000,

    }
}
