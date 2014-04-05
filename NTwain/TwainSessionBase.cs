using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    public class TwainSessionBase : ITwainStateInternal, ITwainOperationInternal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSessionBase(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }
            AppId = appId;
            State = 1;
            EnforceState = true;
        }


        #region ITwainStateInternal Members

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

        void ITwainStateInternal.ChangeState(int newState, bool notifyChange)
        {
            State = newState;
            if (notifyChange)
            {
                RaisePropertyChanged("State");
                OnStateChanged();
            }
        }

        ICommitable ITwainStateInternal.GetPendingStateChanger(int newState)
        {
            return new TentativeStateCommitable(this, newState);
        }

        void ITwainStateInternal.ChangeSourceId(TWIdentity sourceId)
        {
            SourceId = sourceId;
            RaisePropertyChanged("SourceId");
            OnSourceIdChanged();
        }

        #endregion

        #region ITwainState Members

        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>
        /// The app id.
        /// </value>
        public TWIdentity AppId { get; private set; }

        /// <summary>
        /// Gets the source id used for the session.
        /// </summary>
        /// <value>
        /// The source id.
        /// </value>
        public TWIdentity SourceId { get; private set; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State { get; private set; }

        #endregion

        #region ITwainOperation Members

        DGAudio _dgAudio;
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        public DGAudio DGAudio
        {
            get
            {
                if (_dgAudio == null) { _dgAudio = new DGAudio(this); }
                return _dgAudio;
            }
        }

        DGControl _dgControl;
        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        public DGControl DGControl
        {
            get
            {
                if (_dgControl == null) { _dgControl = new DGControl(this); }
                return _dgControl;
            }
        }

        DGImage _dgImage;
        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        public DGImage DGImage
        {
            get
            {
                if (_dgImage == null) { _dgImage = new DGImage(this); }
                return _dgImage;
            }
        }

        #endregion

        #region ITwainOperationInternal Members

        #endregion

        #region INotifyPropertyChanged Members

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
            var hand = PropertyChanged;
            if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #endregion

        #region custom events and overridables

        /// <summary>
        /// Called when <see cref="State"/> changed.
        /// </summary>
        protected virtual void OnStateChanged() { }

        /// <summary>
        /// Called when <see cref="SourceId"/> changed.
        /// </summary>
        protected virtual void OnSourceIdChanged() { }


        #endregion
    }
}
