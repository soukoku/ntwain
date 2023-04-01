using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
  // this file contains initialization-related things.

  public partial class TwainSession
  {
    static bool __encodingRegistered;

    /// <summary>
    /// Creates TWAIN session with app info derived an executable file.
    /// </summary>
    /// <param name="exeFilePath"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    public TwainSession(string exeFilePath,
        TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA) :
        this(FileVersionInfo.GetVersionInfo(exeFilePath),
          appLanguage, appCountry)
    { }
    /// <summary>
    /// Creates TWAIN session with app info derived from a <see cref="FileVersionInfo"/> object.
    /// </summary>
    /// <param name="appInfo"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    public TwainSession(FileVersionInfo appInfo,
        TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA) :
        this(appInfo.CompanyName, appInfo.ProductName, appInfo.ProductName, new Version(appInfo.FileVersion),
          appInfo.FileDescription, appLanguage, appCountry)
    { }
    /// <summary>
    /// Creates TWAIN session with explicit app info.
    /// </summary>
    /// <param name="companyName"></param>
    /// <param name="productFamily"></param>
    /// <param name="productName"></param>
    /// <param name="productVersion"></param>
    /// <param name="productDescription"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    /// <param name="supportedTypes"></param>
    public TwainSession(string companyName, string productFamily, string productName,
        Version productVersion, string productDescription = "",
        TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA,
        DG supportedTypes = DG.IMAGE)
    {
      if (!__encodingRegistered)
      {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        __encodingRegistered = true;
      }

      _appIdentityLegacy = new()
      {
        Manufacturer = companyName,
        ProductFamily = productFamily,
        ProductName = productName,
        ProtocolMajor = (ushort)TWON_PROTOCOL.MAJOR,
        ProtocolMinor = (ushort)TWON_PROTOCOL.MINOR,
        SupportedGroups = (uint)(supportedTypes | DG.APP2),
        Version = new TW_VERSION
        {
          Country = appCountry,
          Info = productDescription,
          Language = appLanguage,
          MajorNum = (ushort)productVersion.Major,
          MinorNum = (ushort)productVersion.Minor,
        }
      };
      if (TwainPlatform.IsLinux) _appIdentity = _appIdentityLegacy;
      if (TwainPlatform.IsMacOSX) _appIdentityOSX = _appIdentityLegacy;
    }

    // really legacy version is the one to be used (except on mac) or
    // until it doesn't work (special linux)

    /// <summary>
    /// Gets the app identity.
    /// </summary>
    public TW_IDENTITY_LEGACY AppIdentity => _appIdentityLegacy;
    internal TW_IDENTITY_LEGACY _appIdentityLegacy;
    internal TW_IDENTITY _appIdentity;
    internal TW_IDENTITY_MACOSX _appIdentityOSX;

    /// <summary>
    /// Gets the current data source.
    /// </summary>
    public TW_IDENTITY_LEGACY DSIdentity => _dsIdentityLegacy;
    internal TW_IDENTITY_LEGACY _dsIdentityLegacy;
    internal TW_IDENTITY _dsIdentity;
    internal TW_IDENTITY_MACOSX _dsIdentityOSX;
  }
}
