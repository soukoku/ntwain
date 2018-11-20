using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Data.Win32
{
    /// <summary>
    /// Class style flags for a window.
    /// </summary>
    [Flags]
    enum ClassStyles : uint
    {
        /// <summary>
        /// Redraws the entire window if a movement or size adjustment changes the height of the client area.
        /// </summary>
        CS_VREDRAW = 0x0001,
        /// <summary>
        /// Redraws the entire window if a movement or size adjustment changes the width of the client area.
        /// </summary>
        CS_HREDRAW = 0x0002,
        /// <summary>
        /// Sends a double-click message to the window procedure when the user double-clicks the mouse while the cursor is within a window belonging to the class.
        /// </summary>
        CS_DBLCLKS = 0x0008,
        /// <summary>
        /// Allocates a unique device context for each window in the class.
        /// </summary>
        CS_OWNDC = 0x0020,
        /// <summary>
        /// Allocates one device context to be shared by all windows in the class. 
        /// </summary>
        CS_CLASSDC = 0x0040,
        /// <summary>
        /// Sets the clipping rectangle of the child window to that of the parent window so that the child can draw on the parent. 
        /// </summary>
        CS_PARENTDC = 0x0080,
        /// <summary>
        /// Disables Close on the window menu.
        /// </summary>
        CS_NOCLOSE = 0x0200,
        /// <summary>
        /// Saves, as a bitmap, the portion of the screen image obscured by a window of this class. When the window is removed, the system uses the saved bitmap to restore the screen image, including other windows that were obscured.
        /// </summary>
        CS_SAVEBITS = 0x0800,
        /// <summary>
        /// Aligns the window's client area on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.
        /// </summary>
        CS_BYTEALIGNCLIENT = 0x1000,
        /// <summary>
        /// Aligns the window on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.
        /// </summary>
        CS_BYTEALIGNWINDOW = 0x2000,
        /// <summary>
        /// Indicates that the window class is an application global class.
        /// </summary>
        CS_GLOBALCLASS = 0x4000,
        CS_IME = 0x10000,
        /// <summary>
        /// Enables the drop shadow effect on a window. The effect is turned on and off through SPI_SETDROPSHADOW. 
        /// </summary>
        CS_DROPSHADOW = 0x20000,
    }
}
