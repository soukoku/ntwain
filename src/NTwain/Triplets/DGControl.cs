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

        PendingXfers _pending;
        internal PendingXfers PendingXfers => _pending ?? (_pending = new PendingXfers(Session));

        CustomDSData _custDSData;
        internal CustomDSData CustomDSData => _custDSData ?? (_custDSData = new CustomDSData(Session));

        Capability _caps;
        /// <summary>
        /// Gets the operations defined for DAT_CAPABILITY.
        /// </summary>
        public Capability Capability => _caps ?? (_caps = new Capability(Session));

        PassThru _passThru;
        /// <summary>
        /// Gets the operations defined for DAT_PASSTHRU.
        /// </summary>
        public PassThru PassThru => _passThru ?? (_passThru = new PassThru(Session));

        SetupMemXfer _memXfer;
        /// <summary>
        /// Gets the operations defined for DAT_SETUPMEMXFER.
        /// </summary>
        public SetupMemXfer SetupMemXfer => _memXfer ?? (_memXfer = new SetupMemXfer(Session));

        SetupFileXfer _fileXfer;
        /// <summary>
        /// Gets the operations defined for DAT_SETUPFILEXFER.
        /// </summary>
        public SetupFileXfer SetupFileXfer => _fileXfer ?? (_fileXfer = new SetupFileXfer(Session));

        FileSystem _fs;
        /// <summary>
        /// Gets the operations defined for DAT_FILESYSTEM.
        /// </summary>
        public FileSystem FileSystem  => _fs ?? (_fs = new FileSystem(Session));

        XferGroup _xferGroup;
        /// <summary>
        /// Gets the operations defined for DAT_XFERGROUP.
        /// </summary>
        public XferGroup XferGroup => _xferGroup ?? (_xferGroup = new XferGroup(Session));

        Metrics _metrics;
        /// <summary>
        /// Gets the operations defined for DAT_METRICS.
        /// </summary>
        public Metrics Metrics => _metrics ?? (_metrics = new Metrics(Session));
    }
}
