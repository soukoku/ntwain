using NTwain.Data;
using NTwain.Triplets.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceEvent = NTwain.Triplets.Control.DeviceEvent;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataGroups.Control"/>.
	/// </summary>
    public partial class DGControl : BaseTriplet
    {
        internal DGControl(TwainSession session) : base(session) { }

        Parent _parent;
        internal Parent Parent => _parent ?? (_parent = new Parent(Session));

        EntryPoint _entryPoint;
        internal EntryPoint EntryPoint => _entryPoint ?? (_entryPoint = new EntryPoint(Session));

        Identity _identity;
        internal Identity Identity => _identity ?? (_identity = new Identity(Session));

        Callback _callback;
        internal Callback Callback => _callback ?? (_callback = new Callback(Session));

        Callback2 _callback2;
        internal Callback2 Callback2 => _callback2 ?? (_callback2 = new Callback2(Session));

        Status _status;
        internal Status Status => _status ?? (_status = new Status(Session));

        StatusUtf8 _statusUtf8;
        internal StatusUtf8 StatusUtf8 => _statusUtf8 ?? (_statusUtf8 = new StatusUtf8(Session));

        DeviceEvent _devEvent;
        internal DeviceEvent DeviceEvent => _devEvent ?? (_devEvent = new DeviceEvent(Session));

        UserInterface _ui;
        internal UserInterface UserInterface => _ui ?? (_ui = new UserInterface(Session));

        Event _event;
        internal Event Event => _event ?? (_event = new Event(Session));

        XferGroup _xferGroup;
        /// <summary>
        /// Gets the operations defined for DAT_XFERGROUP.
        /// </summary>
        public XferGroup XferGroup => _xferGroup ?? (_xferGroup = new XferGroup(Session));
    }
}
