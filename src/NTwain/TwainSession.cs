using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Text;
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
        this(appInfo.CompanyName ?? "",
          appInfo.ProductName ?? "",
          appInfo.ProductName ?? "",
          new Version(appInfo.FileVersion ?? "1.0"),
          appInfo.FileDescription ?? "", appLanguage, appCountry)
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
        SupportedGroups = (uint)(supportedTypes | DG.CONTROL | DG.APP2),
        Version = new TW_VERSION
        {
          Country = appCountry,
          Info = productDescription,
          Language = appLanguage,
          MajorNum = (ushort)productVersion.Major,
          MinorNum = (ushort)productVersion.Minor,
        }
      };
      //if (TwainPlatform.IsLinux) _appIdentity = _appIdentityLegacy;
      if (TwainPlatform.IsMacOSX) _appIdentityOSX = _appIdentityLegacy;

      DGControl = new DGControl(this);
      DGImage = new DGImage(this);
      DGAudio = new DGAudio(this);
    }
  }
}
