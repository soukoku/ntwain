using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    partial class TwainSession : INotifyPropertyChanged
    {
        private TwainState _state;
        /// <summary>
        /// Gets the logical state of the session.
        /// </summary>
        public TwainState State
        {
            get { return _state; }
            internal set
            {
                _state = value;
                RaisePropertyChanged(nameof(State));
            }
        }

        DGControl dgControl;
        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        public DGControl DGControl => dgControl ?? (dgControl = new DGControl(this));

        DGImage dgImage;
        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        public DGImage DGImage => dgImage ?? (dgImage = new DGImage(this));

        DGAudio dgAudio;
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        public DGAudio DGAudio => dgAudio ?? (dgAudio = new DGAudio(this));

        ///// <summary>
        ///// Gets/sets the direct triplet operation entry for custom values.
        ///// </summary>
        //public DGCustom DGCustom { get; set; }

        /// <summary>
        /// Occurs when an enabled source has been disabled (back to state 4).
        /// </summary>
        public event EventHandler<SourceDisabledEventArgs> SourceDisabled;

        /// <summary>
        /// Raises the <see cref="SourceDisabled"/> event.
        /// </summary>
        /// <param name="e"></param>
        internal protected virtual void OnSourceDisabled(SourceDisabledEventArgs e)
        {
            var handler = SourceDisabled;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the source has generated an event.
        /// </summary>
        public event EventHandler<DeviceEventArgs> DeviceEventReceived;

        /// <summary>
        /// Raises the <see cref="DeviceEventReceived"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDeviceEventReceived(DeviceEventArgs e)
        {
            var handler = DeviceEventReceived;
            handler?.Invoke(this, e);
        }




        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
