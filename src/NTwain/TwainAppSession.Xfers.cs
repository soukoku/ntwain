using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NTwain
{
  // this file contains various xfer methods

  partial class TwainAppSession
  {
    /// <summary>
    /// Start the transfer loop.
    /// This should be called after receiving 
    /// <see cref="MSG.XFERREADY"/> in the background thread.
    /// </summary>
    void EnterTransferRoutine()
    {
      // default options if source don't support them or whatever
      bool xferImage = true;
      bool xferAudio = false;
      var imgXferMech = TWSX.NATIVE;
      var audXferMech = TWSX.NATIVE;
      if (DGControl.XferGroup.Get(ref _appIdentity, ref _currentDS, out DG xferType) == TWRC.SUCCESS)
      {
        xferAudio = (xferType & DG.AUDIO) == DG.AUDIO;
        var dsName = _currentDS.ProductName.ToString();
        // check for Plustek OpticSlim 2680H, this scanner returns wrong xferGroup after first scanning
        if (dsName.IndexOf("Plustek", StringComparison.OrdinalIgnoreCase) > -1 &&
            dsName.IndexOf("OpticSlim", StringComparison.OrdinalIgnoreCase) > -1 &&
            dsName.IndexOf("2680H", StringComparison.OrdinalIgnoreCase) > -1)
        {
          xferImage = true;
        }
        else
        {
          // some DS end up getting none but we will assume it's image
          xferImage = xferType == 0 || (xferType & DG.IMAGE) == DG.IMAGE;
        }
      }

      //if (xferImage)
      //{
      //  imgXferMech = session.CurrentSource.Capabilities.ICapXferMech.GetCurrent();
      //}
      //if (xferAudio)
      //{
      //  audXferMech = session.CurrentSource.Capabilities.ACapXferMech.GetCurrent();
      //}

      TW_PENDINGXFERS pending = default;
      var rc = DGControl.PendingXfers.Get(ref _appIdentity, ref _currentDS, ref pending);
      if (rc == TWRC.SUCCESS)
      {
        do
        {
          // cancel for now
          //rc = DGControl.PendingXfers.Reset(ref _appIdentity, ref _currentDS, ref pending);


          rc = DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending);

          //if (xferType.HasFlag(DG.AUDIO))
          //{
          //  DoTransferAudio();
          //}
          //else // just defaults to image
          //{
          //  DoTransferImage();
          //}

        } while (rc == TWRC.SUCCESS && pending.Count != 0);
      }
      else
      {
        HandleNonSuccessXferCode(rc);
      }
      _uiThreadMarshaller.BeginInvoke(() =>
      {
        DisableSource();
      });
    }

    private void HandleNonSuccessXferCode(TWRC rc)
    {
      switch (rc)
      {
        case TWRC.SUCCESS:
        case TWRC.XFERDONE:
        case TWRC.CANCEL:
          // ok to keep going
          break;
        default:
          var status = GetLastStatus(false);
          var text = GetStatusText(status);
          // TODO: raise error event

          break;
      }
    }
  }
}
