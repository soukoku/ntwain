using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Data
{
    delegate ReturnCode Callback32(TW_IDENTITY origin, TW_IDENTITY destination,
            DataGroups dg, DataArgumentType dat, Message msg, IntPtr data);

}
