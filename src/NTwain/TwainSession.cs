using NTwain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    public class TwainSession : INotifyPropertyChanged, IDisposable
    {
        private TwainConfig _config;
        public TwainSession(TwainConfig config)
        {
            _config = config;
            var rc = Open();
            if (rc != ReturnCode.Success)
            {

            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string property)
        {
            var handle = PropertyChanged;
            handle?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        ReturnCode Open()
        {
            if (State < TwainState.DsmOpened)
            {

            }
            throw new NotImplementedException();
        }

        ReturnCode StepDown(TwainState targetState)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StepDown(TwainState.DsmLoaded);
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TwainSession() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
