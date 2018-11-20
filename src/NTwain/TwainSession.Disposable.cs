//using NTwain.Data;
//using NTwain.Triplets;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;

//namespace NTwain
//{
//    partial class TwainSession : IDisposable
//    {
//        private bool disposedValue = false; // To detect redundant calls

//        /// <summary>
//        /// Handles actual disposal logic.
//        /// </summary>
//        /// <param name="disposing"></param>
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    StepDown(TwainState.DsmLoaded);
//                    // TODO: dispose managed state (managed objects).
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
//                // TODO: set large fields to null.

//                disposedValue = true;
//            }
//        }

//        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
//        // ~TwainSession() {
//        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//        //   Dispose(false);
//        // }

//        /// <summary>
//        /// Closes any open TWAIN objects.
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            // TODO: uncomment the following line if the finalizer is overridden above.
//            // GC.SuppressFinalize(this);
//        }
//    }
//}
