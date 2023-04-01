using NTwain.Triplets;
using System.Collections.Generic;
using TWAINWorkingGroup;

namespace NTwain
{
  // this file contains data source utilities

  partial class TwainSession
  {
    /// <summary>
    /// Gets all available sources.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TW_IDENTITY_LEGACY> GetSources()
    {
      var rc = DGControl.Identity.GetFirst(out TW_IDENTITY_LEGACY ds);
      while (rc == STS.SUCCESS)
      {
        yield return ds;
        rc = DGControl.Identity.GetNext(out ds);
      }
    }
  }
}
