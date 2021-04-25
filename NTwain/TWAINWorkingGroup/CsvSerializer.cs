///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  These are the definitions for TWAIN.  They're essentially the C/C++
//  TWAIN.H file contents translated to C#, with modifications that
//  recognize the differences between Windows, Linux and Mac OS X.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
//  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
//  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
//  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
//  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
//  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
//  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
//  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
//  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
//  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2020 Kodak Alaris Inc.
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;

namespace TWAINWorkingGroup
{
    public static class CsvSerializer
    {
        /// <summary>
        /// Convert the contents of an audio info to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twaudioinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string AudioinfoToCsv(TW_AUDIOINFO a_twaudioinfo)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twaudioinfo.Name.Get());
                csv.Add(a_twaudioinfo.Reserved.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a callback to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcallback">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string CallbackToCsv(TW_CALLBACK a_twcallback)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcallback.CallBackProc.ToString());
                csv.Add(a_twcallback.RefCon.ToString());
                csv.Add(a_twcallback.Message.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an callback structure...
        /// </summary>
        /// <param name="a_twcallback">A TWAIN structure</param>
        /// <param name="a_szCallback">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToCallback(ref TW_CALLBACK a_twcallback, string a_szCallback)
        {
            // Init stuff...
            a_twcallback = default(TW_CALLBACK);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCallback);

                // Grab the values...
                a_twcallback.CallBackProc = (IntPtr)UInt64.Parse(asz[0]);
                a_twcallback.RefCon = uint.Parse(asz[1]);
                a_twcallback.Message = ushort.Parse(asz[2]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a callback2 to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcallback2">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string Callback2ToCsv(TW_CALLBACK2 a_twcallback2)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcallback2.CallBackProc.ToString());
                csv.Add(a_twcallback2.RefCon.ToString());
                csv.Add(a_twcallback2.Message.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an callback2 structure...
        /// </summary>
        /// <param name="a_twcallback2">A TWAIN structure</param>
        /// <param name="a_szCallback2">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToCallback2(ref TW_CALLBACK2 a_twcallback2, string a_szCallback2)
        {
            // Init stuff...
            a_twcallback2 = default(TW_CALLBACK2);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCallback2);

                // Grab the values...
                a_twcallback2.CallBackProc = (IntPtr)UInt64.Parse(asz[0]);
                a_twcallback2.RefCon = (UIntPtr)UInt64.Parse(asz[1]);
                a_twcallback2.Message = ushort.Parse(asz[2]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }


        /// <summary>
        /// Convert the contents of a device event to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twdeviceevent">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string DeviceeventToCsv(TW_DEVICEEVENT a_twdeviceevent)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((TWDE)a_twdeviceevent.Event).ToString());
                csv.Add(a_twdeviceevent.DeviceName.Get());
                csv.Add(a_twdeviceevent.BatteryMinutes.ToString());
                csv.Add(a_twdeviceevent.BatteryPercentage.ToString());
                csv.Add(a_twdeviceevent.PowerSupply.ToString());
                csv.Add(((double)a_twdeviceevent.XResolution.Whole + ((double)a_twdeviceevent.XResolution.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twdeviceevent.YResolution.Whole + ((double)a_twdeviceevent.YResolution.Frac / 65536.0)).ToString());
                csv.Add(a_twdeviceevent.FlashUsed2.ToString());
                csv.Add(a_twdeviceevent.AutomaticCapture.ToString());
                csv.Add(a_twdeviceevent.TimeBeforeFirstCapture.ToString());
                csv.Add(a_twdeviceevent.TimeBetweenCaptures.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of an entry point to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twentrypoint">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string EntrypointToCsv(TW_ENTRYPOINT a_twentrypoint)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twentrypoint.Size.ToString());
                csv.Add("0x" + ((a_twentrypoint.DSM_Entry == null) ? "0" : a_twentrypoint.DSM_Entry.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemAllocate == null) ? "0" : a_twentrypoint.DSM_MemAllocate.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemFree == null) ? "0" : a_twentrypoint.DSM_MemFree.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemLock == null) ? "0" : a_twentrypoint.DSM_MemLock.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemUnlock == null) ? "0" : a_twentrypoint.DSM_MemUnlock.ToString("X")));
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of an event to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twevent">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string EventToCsv(TW_EVENT a_twevent)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twevent.pEvent.ToString());
                csv.Add(a_twevent.TWMessage.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of an extimageinfo to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twextimageinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ExtimageinfoToCsv(TW_EXTIMAGEINFO a_twextimageinfo)
        {
            try
            {
                uint uTwinfo = 0;
                CSV csv = new CSV();
                csv.Add(a_twextimageinfo.NumInfos.ToString());
                for (int ii = 0; (ii < a_twextimageinfo.NumInfos) && (ii < 200); ii++)
                {
                    TWEI twei;
                    TWTY twty;
                    STS sts;
                    TW_INFO twinfo = default(TW_INFO);
                    a_twextimageinfo.Get(uTwinfo, ref twinfo);
                    twei = (TWEI)twinfo.InfoId;
                    if (twei.ToString() != twinfo.InfoId.ToString())
                    {
                        csv.Add("TWEI_" + twei.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.InfoId));
                    }
                    twty = (TWTY)twinfo.ItemType;
                    if (twty.ToString() != twinfo.ItemType.ToString())
                    {
                        csv.Add("TWTY_" + twty.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.ItemType));
                    }
                    csv.Add(twinfo.NumItems.ToString());
                    sts = (STS)twinfo.ReturnCode;
                    if (sts.ToString() != twinfo.ReturnCode.ToString())
                    {
                        csv.Add("TWRC_" + sts.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.ReturnCode));
                    }
                    csv.Add(twinfo.ReturnCode.ToString());
                    csv.Add(twinfo.Item.ToString());
                    uTwinfo += 1;
                }
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an extimageinfo structure,
        /// note that we don't have to worry about containers going in this
        /// direction...
        /// </summary>
        /// <param name="a_twextimageinfo">A TWAIN structure</param>
        /// <param name="a_szExtimageinfo">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToExtimageinfo(ref TW_EXTIMAGEINFO a_twextimageinfo, string a_szExtimageinfo)
        {
            // Init stuff...
            a_twextimageinfo = default(TW_EXTIMAGEINFO);

            // Build the string...
            try
            {
                int iField;
                uint uTwinfo;
                string[] asz = CSV.Parse(a_szExtimageinfo);

                // Set the number of entries (this is the easy bit)...
                uint.TryParse(asz[0], out a_twextimageinfo.NumInfos);
                if (a_twextimageinfo.NumInfos > 200)
                {
                    Log.Error("***error*** - we're limited to 200 entries, if this is a problem, just add more, and fix this code...");
                    return (false);
                }

                // Okay, walk all the entries in steps of TW_INFO...
                uTwinfo = 0;
                for (iField = 1; iField < asz.Length; iField += 5)
                {
                    UInt64 u64;
                    TWEI twei;
                    TW_INFO twinfo = default(TW_INFO);
                    if ((iField + 5) > asz.Length)
                    {
                        Log.Error("***error*** - badly constructed list, should be: num,(twinfo),(twinfo)...");
                        return (false);
                    }
                    if (TWEI.TryParse(asz[iField + 0].Replace("TWEI_", ""), out twei))
                    {
                        twinfo.InfoId = (ushort)twei;
                    }
                    else
                    {
                        if (asz[iField + 0].ToLowerInvariant().StartsWith("0x"))
                        {
                            ushort.TryParse(asz[iField + 0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out twinfo.InfoId);
                        }
                        else
                        {
                            ushort.TryParse(asz[iField + 0], out twinfo.InfoId);
                        }
                    }
                    // We really don't care about these...
                    ushort.TryParse(asz[iField + 1], out twinfo.ItemType);
                    ushort.TryParse(asz[iField + 2], out twinfo.NumItems);
                    ushort.TryParse(asz[iField + 3], out twinfo.ReturnCode);
                    UInt64.TryParse(asz[iField + 4], out u64);
                    twinfo.Item = (UIntPtr)u64;
                    a_twextimageinfo.Set(uTwinfo, ref twinfo);
                    uTwinfo += 1;
                }
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a filesystem to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twfilesystem">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string FilesystemToCsv(TW_FILESYSTEM a_twfilesystem)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twfilesystem.InputName.Get());
                csv.Add(a_twfilesystem.OutputName.Get());
                csv.Add(a_twfilesystem.Context.ToString());
                csv.Add(a_twfilesystem.Recursive.ToString());
                csv.Add(a_twfilesystem.FileType.ToString());
                csv.Add(a_twfilesystem.Size.ToString());
                csv.Add(a_twfilesystem.CreateTimeDate.Get());
                csv.Add(a_twfilesystem.ModifiedTimeDate.Get());
                csv.Add(a_twfilesystem.FreeSpace.ToString());
                csv.Add(a_twfilesystem.NewImageSize.ToString());
                csv.Add(a_twfilesystem.NumberOfFiles.ToString());
                csv.Add(a_twfilesystem.NumberOfSnippets.ToString());
                csv.Add(a_twfilesystem.DeviceGroupMask.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to a filesystem structure...
        /// </summary>
        /// <param name="a_twfilesystem">A TWAIN structure</param>
        /// <param name="a_szFilesystem">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToFilesystem(ref TW_FILESYSTEM a_twfilesystem, string a_szFilesystem)
        {
            // Init stuff...
            a_twfilesystem = default(TW_FILESYSTEM);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szFilesystem);

                // Grab the values...
                a_twfilesystem.InputName.Set(asz[0]);
                a_twfilesystem.OutputName.Set(asz[1]);
                a_twfilesystem.Context = (IntPtr)UInt64.Parse(asz[2]);
                a_twfilesystem.Recursive = int.Parse(asz[3]);
                a_twfilesystem.FileType = int.Parse(asz[4]);
                a_twfilesystem.Size = uint.Parse(asz[5]);
                a_twfilesystem.CreateTimeDate.Set(asz[6]);
                a_twfilesystem.ModifiedTimeDate.Set(asz[7]);
                a_twfilesystem.FreeSpace = (uint)UInt64.Parse(asz[8]);
                a_twfilesystem.NewImageSize = (uint)UInt64.Parse(asz[9]);
                a_twfilesystem.NumberOfFiles = (uint)UInt64.Parse(asz[10]);
                a_twfilesystem.NumberOfSnippets = (uint)UInt64.Parse(asz[11]);
                a_twfilesystem.DeviceGroupMask = (uint)UInt64.Parse(asz[12]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of an iccprofile to a string that we can
        /// show in our simple GUI...
        /// </summary>
        /// <param name="a_twmemory">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string IccprofileToCsv(TW_MEMORY a_twmemory)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twmemory.Flags.ToString());
                csv.Add(a_twmemory.Length.ToString());
                csv.Add(a_twmemory.TheMem.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of an identity to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twidentity">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string IdentityToCsv(TW_IDENTITY a_twidentity)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twidentity.Id.ToString());
                csv.Add(a_twidentity.Version.MajorNum.ToString());
                csv.Add(a_twidentity.Version.MinorNum.ToString());
                csv.Add(a_twidentity.Version.Language.ToString());
                csv.Add(a_twidentity.Version.Country.ToString());
                csv.Add(a_twidentity.Version.Info.Get());
                csv.Add(a_twidentity.ProtocolMajor.ToString());
                csv.Add(a_twidentity.ProtocolMinor.ToString());
                csv.Add("0x" + a_twidentity.SupportedGroups.ToString("X"));
                csv.Add(a_twidentity.Manufacturer.Get());
                csv.Add(a_twidentity.ProductFamily.Get());
                csv.Add(a_twidentity.ProductName.Get());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an identity structure...
        /// </summary>
        /// <param name="a_twidentity">A TWAIN structure</param>
        /// <param name="a_szIdentity">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToIdentity(ref TW_IDENTITY a_twidentity, string a_szIdentity)
        {
            // Init stuff...
            a_twidentity = default(TW_IDENTITY);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szIdentity);

                // Grab the values...
                a_twidentity.Id = ulong.Parse(asz[0]);
                a_twidentity.Version.MajorNum = ushort.Parse(asz[1]);
                a_twidentity.Version.MinorNum = ushort.Parse(asz[2]);
                if (asz[3] != "0") a_twidentity.Version.Language = (TWLG)Enum.Parse(typeof(TWLG), asz[3]);
                if (asz[4] != "0") a_twidentity.Version.Country = (TWCY)Enum.Parse(typeof(TWCY), asz[4]);
                a_twidentity.Version.Info.Set(asz[5]);
                a_twidentity.ProtocolMajor = ushort.Parse(asz[6]);
                a_twidentity.ProtocolMinor = ushort.Parse(asz[7]);
                a_twidentity.SupportedGroups = asz[8].ToLower().StartsWith("0x") ? Convert.ToUInt32(asz[8].Remove(0, 2), 16) : Convert.ToUInt32(asz[8], 16);
                a_twidentity.Manufacturer.Set(asz[9]);
                a_twidentity.ProductFamily.Set(asz[10]);
                a_twidentity.ProductName.Set(asz[11]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a image info to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twimageinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImageinfoToCsv(TW_IMAGEINFO a_twimageinfo)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((double)a_twimageinfo.XResolution.Whole + ((double)a_twimageinfo.XResolution.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimageinfo.YResolution.Whole + ((double)a_twimageinfo.YResolution.Frac / 65536.0)).ToString());
                csv.Add(a_twimageinfo.ImageWidth.ToString());
                csv.Add(a_twimageinfo.ImageLength.ToString());
                csv.Add(a_twimageinfo.SamplesPerPixel.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_0.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_1.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_2.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_3.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_4.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_5.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_6.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_7.ToString());
                csv.Add(a_twimageinfo.Planar.ToString());
                csv.Add("TWPT_" + (TWPT)a_twimageinfo.PixelType);
                csv.Add("TWCP_" + (TWCP)a_twimageinfo.Compression);
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an callback structure...
        /// </summary>
        /// <param name="a_twimageinfo">A TWAIN structure</param>
        /// <param name="a_szImageinfo">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImageinfo(ref TW_IMAGEINFO a_twimageinfo, string a_szImageinfo)
        {
            // Init stuff...
            a_twimageinfo = default(TW_IMAGEINFO);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImageinfo);

                // Grab the values...
                a_twimageinfo.XResolution.Whole = (short)double.Parse(asz[0]);
                a_twimageinfo.XResolution.Frac = (ushort)((double.Parse(asz[0]) - (double)a_twimageinfo.XResolution.Whole) * 65536.0);
                a_twimageinfo.YResolution.Whole = (short)double.Parse(asz[1]);
                a_twimageinfo.YResolution.Frac = (ushort)((double.Parse(asz[1]) - (double)a_twimageinfo.YResolution.Whole) * 65536.0);
                a_twimageinfo.ImageWidth = (short)double.Parse(asz[2]);
                a_twimageinfo.ImageLength = int.Parse(asz[3]);
                a_twimageinfo.SamplesPerPixel = short.Parse(asz[4]);
                a_twimageinfo.BitsPerSample_0 = short.Parse(asz[5]);
                a_twimageinfo.BitsPerSample_1 = short.Parse(asz[6]);
                a_twimageinfo.BitsPerSample_2 = short.Parse(asz[7]);
                a_twimageinfo.BitsPerSample_3 = short.Parse(asz[8]);
                a_twimageinfo.BitsPerSample_4 = short.Parse(asz[9]);
                a_twimageinfo.BitsPerSample_5 = short.Parse(asz[10]);
                a_twimageinfo.BitsPerSample_6 = short.Parse(asz[11]);
                a_twimageinfo.BitsPerSample_7 = short.Parse(asz[12]);
                a_twimageinfo.Planar = ushort.Parse(CvtCapValueFromEnum(CAP.ICAP_PLANARCHUNKY, asz[13]));
                a_twimageinfo.PixelType = (TWPT)ushort.Parse(CvtCapValueFromEnum(CAP.ICAP_PIXELTYPE, asz[14]));
                a_twimageinfo.Compression = (TWCP)ushort.Parse(CvtCapValueFromEnum(CAP.ICAP_COMPRESSION, asz[15]));
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a image layout to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twimagelayout">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImagelayoutToCsv(TW_IMAGELAYOUT a_twimagelayout)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((double)a_twimagelayout.Frame.Left.Whole + ((double)a_twimagelayout.Frame.Left.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Top.Whole + ((double)a_twimagelayout.Frame.Top.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Right.Whole + ((double)a_twimagelayout.Frame.Right.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Bottom.Whole + ((double)a_twimagelayout.Frame.Bottom.Frac / 65536.0)).ToString());
                csv.Add(a_twimagelayout.DocumentNumber.ToString());
                csv.Add(a_twimagelayout.PageNumber.ToString());
                csv.Add(a_twimagelayout.FrameNumber.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an image layout structure...
        /// </summary>
        /// <param name="a_twimagelayout">A TWAIN structure</param>
        /// <param name="a_szImagelayout">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImagelayout(ref TW_IMAGELAYOUT a_twimagelayout, string a_szImagelayout)
        {
            // Init stuff...
            a_twimagelayout = default(TW_IMAGELAYOUT);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImagelayout);

                // Sort out the frame...
                a_twimagelayout.Frame.Left.Whole = (short)double.Parse(asz[0]);
                a_twimagelayout.Frame.Left.Frac = (ushort)((double.Parse(asz[0]) - (double)a_twimagelayout.Frame.Left.Whole) * 65536.0);
                a_twimagelayout.Frame.Top.Whole = (short)double.Parse(asz[1]);
                a_twimagelayout.Frame.Top.Frac = (ushort)((double.Parse(asz[1]) - (double)a_twimagelayout.Frame.Top.Whole) * 65536.0);
                a_twimagelayout.Frame.Right.Whole = (short)double.Parse(asz[2]);
                a_twimagelayout.Frame.Right.Frac = (ushort)((double.Parse(asz[2]) - (double)a_twimagelayout.Frame.Right.Whole) * 65536.0);
                a_twimagelayout.Frame.Bottom.Whole = (short)double.Parse(asz[3]);
                a_twimagelayout.Frame.Bottom.Frac = (ushort)((double.Parse(asz[3]) - (double)a_twimagelayout.Frame.Bottom.Whole) * 65536.0);

                // And now the counters...
                a_twimagelayout.DocumentNumber = (uint)int.Parse(asz[4]);
                a_twimagelayout.PageNumber = (uint)int.Parse(asz[5]);
                a_twimagelayout.FrameNumber = (uint)int.Parse(asz[6]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of an image mem xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twimagememxfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImagememxferToCsv(TW_IMAGEMEMXFER a_twimagememxfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add("TWCP_" + (TWCP)a_twimagememxfer.Compression);
                csv.Add(a_twimagememxfer.BytesPerRow.ToString());
                csv.Add(a_twimagememxfer.Columns.ToString());
                csv.Add(a_twimagememxfer.Rows.ToString());
                csv.Add(a_twimagememxfer.XOffset.ToString());
                csv.Add(a_twimagememxfer.YOffset.ToString());
                csv.Add(a_twimagememxfer.BytesWritten.ToString());
                csv.Add(a_twimagememxfer.Memory.Flags.ToString());
                csv.Add(a_twimagememxfer.Memory.Length.ToString());
                csv.Add(a_twimagememxfer.Memory.TheMem.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to an image mem xfer structure...
        /// </summary>
        /// <param name="a_twimagememxfer">A TWAIN structure</param>
        /// <param name="a_szImagememxfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImagememxfer(ref TW_IMAGEMEMXFER a_twimagememxfer, string a_szImagememxfer)
        {
            // Init stuff...
            a_twimagememxfer = default(TW_IMAGEMEMXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImagememxfer);

                // Sort out the structure...
                a_twimagememxfer.Compression = ushort.Parse(CvtCapValueFromEnum(CAP.ICAP_COMPRESSION, asz[0]));
                a_twimagememxfer.BytesPerRow = uint.Parse(asz[1]);
                a_twimagememxfer.Columns = uint.Parse(asz[2]);
                a_twimagememxfer.Rows = uint.Parse(asz[3]);
                a_twimagememxfer.XOffset = uint.Parse(asz[4]);
                a_twimagememxfer.YOffset = uint.Parse(asz[5]);
                a_twimagememxfer.BytesWritten = uint.Parse(asz[6]);
                a_twimagememxfer.Memory.Flags = ushort.Parse(asz[7]);
                a_twimagememxfer.Memory.Length = uint.Parse(asz[8]);
                a_twimagememxfer.Memory.TheMem = (IntPtr)ulong.Parse(asz[9]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a metrics structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twmetrics">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string MetricsToCsv(TW_METRICS a_twmetrics)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twmetrics.SizeOf.ToString());
                csv.Add(a_twmetrics.ImageCount.ToString());
                csv.Add(a_twmetrics.SheetCount.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a patthru structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string PassthruToCsv(TW_PASSTHRU a_twpassthru)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twpassthru.pCommand.ToString());
                csv.Add(a_twpassthru.CommandBytes.ToString());
                csv.Add(a_twpassthru.Direction.ToString());
                csv.Add(a_twpassthru.pData.ToString());
                csv.Add(a_twpassthru.DataBytes.ToString());
                csv.Add(a_twpassthru.DataBytesXfered.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to a passthru structure...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <param name="a_szPassthru">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToPassthru(ref TW_PASSTHRU a_twpassthru, string a_szPassthru)
        {
            // Init stuff...
            a_twpassthru = default(TW_PASSTHRU);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szPassthru);

                // Sort out the frame...
                a_twpassthru.pCommand = (IntPtr)UInt64.Parse(asz[0]);
                a_twpassthru.CommandBytes = uint.Parse(asz[1]);
                a_twpassthru.Direction = int.Parse(asz[2]);
                a_twpassthru.pData = (IntPtr)UInt64.Parse(asz[3]);
                a_twpassthru.DataBytes = uint.Parse(asz[4]);
                a_twpassthru.DataBytesXfered = uint.Parse(asz[5]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a pending xfers structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string PendingxfersToCsv(TW_PENDINGXFERS a_twpendingxfers)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twpendingxfers.Count.ToString());
                csv.Add(a_twpendingxfers.EOJ.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to a pendingxfers structure...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <param name="a_szPassthru">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToPendingXfers(ref TW_PENDINGXFERS a_twpendingxfers, string a_szPendingxfers)
        {
            // Init stuff...
            a_twpendingxfers = default(TW_PENDINGXFERS);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szPendingxfers);

                // Sort out the frame...
                a_twpendingxfers.Count = ushort.Parse(asz[0]);
                a_twpendingxfers.EOJ = uint.Parse(asz[1]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a setup file xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string SetupfilexferToCsv(TW_SETUPFILEXFER a_twsetupfilexfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twsetupfilexfer.FileName.Get());
                csv.Add("TWFF_" + a_twsetupfilexfer.Format);
                csv.Add(a_twsetupfilexfer.VRefNum.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert a string to a setupfilexfer...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <param name="a_szSetupfilexfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToSetupfilexfer(ref TW_SETUPFILEXFER a_twsetupfilexfer, string a_szSetupfilexfer)
        {
            // Init stuff...
            a_twsetupfilexfer = default(TW_SETUPFILEXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szSetupfilexfer);

                // Sort out the values...
                a_twsetupfilexfer.FileName.Set(asz[0]);
                a_twsetupfilexfer.Format = (TWFF)ushort.Parse(CvtCapValueFromEnum(CAP.ICAP_IMAGEFILEFORMAT, asz[1]));
                a_twsetupfilexfer.VRefNum = short.Parse(asz[2]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a setup mem xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupmemxfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string SetupmemxferToCsv(TW_SETUPMEMXFER a_twsetupmemxfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twsetupmemxfer.MinBufSize.ToString());
                csv.Add(a_twsetupmemxfer.MaxBufSize.ToString());
                csv.Add(a_twsetupmemxfer.Preferred.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert a string to a setupmemxfer...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <param name="a_szSetupfilexfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToSetupmemxfer(ref TW_SETUPMEMXFER a_twsetupmemxfer, string a_szSetupmemxfer)
        {
            // Init stuff...
            a_twsetupmemxfer = default(TW_SETUPMEMXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szSetupmemxfer);

                // Sort out the values...
                a_twsetupmemxfer.MinBufSize = uint.Parse(asz[0]);
                a_twsetupmemxfer.MaxBufSize = uint.Parse(asz[1]);
                a_twsetupmemxfer.Preferred = uint.Parse(asz[2]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a status structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twstatus">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string StatusToCsv(TW_STATUS a_twstatus)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twstatus.ConditionCode.ToString());
                csv.Add(a_twstatus.Data.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert a string to a twaindirect...
        /// </summary>
        /// <param name="a_twtwaindirect">A TWAIN structure</param>
        /// <param name="a_szTwaindirect">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToTwaindirect(ref TW_TWAINDIRECT a_twtwaindirect, string a_szTwaindirect)
        {
            // Init stuff...
            a_twtwaindirect = default(TW_TWAINDIRECT);

            // Build the string...
            try
            {
                long lTmp;
                string[] asz = CSV.Parse(a_szTwaindirect);

                // Sort out the values...
                if (!uint.TryParse(asz[0], out a_twtwaindirect.SizeOf))
                {
                    return (false);
                }
                if (!ushort.TryParse(asz[1], out a_twtwaindirect.CommunicationManager))
                {
                    return (false);
                }
                if (!long.TryParse(asz[2], out lTmp))
                {
                    return (false);
                }
                a_twtwaindirect.Send = new IntPtr(lTmp);
                if (!uint.TryParse(asz[3], out a_twtwaindirect.SendSize))
                {
                    return (false);
                }
                if (!long.TryParse(asz[4], out lTmp))
                {
                    return (false);
                }
                a_twtwaindirect.Receive = new IntPtr(lTmp);
                if (!uint.TryParse(asz[5], out a_twtwaindirect.ReceiveSize))
                {
                    return (false);
                }
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a twaindirect structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twtwaindirect">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string TwaindirectToCsv(TW_TWAINDIRECT a_twtwaindirect)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twtwaindirect.SizeOf.ToString());
                csv.Add(a_twtwaindirect.CommunicationManager.ToString());
                csv.Add(a_twtwaindirect.Send.ToString());
                csv.Add(a_twtwaindirect.SendSize.ToString());
                csv.Add(a_twtwaindirect.Receive.ToString());
                csv.Add(a_twtwaindirect.ReceiveSize.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a userinterface to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twuserinterface">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string UserinterfaceToCsv(TW_USERINTERFACE a_twuserinterface)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(CvtCapValueToEnumHelper<bool>(a_twuserinterface.ShowUI.ToString()));
                csv.Add(CvtCapValueToEnumHelper<bool>(a_twuserinterface.ModalUI.ToString()));
                csv.Add(a_twuserinterface.hParent.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a transfer group to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_u32Xfergroup">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string XfergroupToCsv(UInt32 a_u32Xfergroup)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add("0x" + a_u32Xfergroup.ToString("X"));
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to a transfer group...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <param name="a_szCustomdsdata">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToXfergroup(ref UInt32 a_u32Xfergroup, string a_szXfergroup)
        {
            // Init stuff...
            a_u32Xfergroup = 0;

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szXfergroup);

                // Grab the values...
                a_u32Xfergroup = asz[0].ToLower().StartsWith("0x") ? Convert.ToUInt32(asz[0].Remove(0, 2), 16) : Convert.ToUInt32(asz[0], 16);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// This mess is what tries to turn numeric constants into something
        /// a bit more readable...
        /// </summary>
        /// <typeparam name="T">type for the conversion</typeparam>
        /// <param name="a_szValue">value to convert</param>
        /// <returns></returns>
        private static string CvtCapValueToEnumHelper<T>(string a_szValue)
        {
            T t;
            Int32 i32 = 0;
            UInt32 u32 = 0;
            string szCvt = "";

            // Adjust our value, as needed...
            if (a_szValue.StartsWith(typeof(T).Name + "_"))
            {
                a_szValue = a_szValue.Substring((typeof(T).Name + "_").Length);
            }

            // Handle enums with negative numbers...
            if (a_szValue.StartsWith("-"))
            {
                if (Int32.TryParse(a_szValue, out i32))
                {
                    t = (T)Enum.Parse(typeof(T), a_szValue, true);
                    szCvt = t.ToString();
                    if (szCvt != i32.ToString())
                    {
                        return (typeof(T).Name + "_" + szCvt);
                    }
                }
            }

            // Everybody else...
            else if (UInt32.TryParse(a_szValue, out u32))
            {
                // Handle bool...
                if (typeof(T) == typeof(bool))
                {
                    if ((a_szValue == "1") || (a_szValue.ToLowerInvariant() == "true"))
                    {
                        return ("TRUE");
                    }
                    return ("FALSE");
                }

                // Handle DAT (which is a weird one)..
                else if (typeof(T) == typeof(DAT))
                {
                    UInt32 u32Dg = u32 >> 16;
                    UInt32 u32Dat = u32 & 0xFFFF;
                    string szDg = ((DG)u32Dg).ToString();
                    string szDat = ((DAT)u32Dat).ToString();
                    szDg = (szDg != u32Dg.ToString()) ? ("DG_" + szDg) : string.Format("0x{0:X}", u32Dg);
                    szDat = (szDat != u32Dat.ToString()) ? ("DAT_" + szDat) : string.Format("0x{0:X}", u32Dat);
                    return (szDg + "|" + szDat);
                }

                // Everybody else is on their own...
                else
                {
                    // mono hurls on this, .net doesn't, so gonna help...
                    switch (a_szValue)
                    {
                        default: break;
                        case "65535": a_szValue = "-1"; break;
                        case "65534": a_szValue = "-2"; break;
                        case "65533": a_szValue = "-3"; break;
                        case "65532": a_szValue = "-4"; break;
                    }
                    t = (T)Enum.Parse(typeof(T), a_szValue, true);
                }

                // Check to see if we changed anything...
                szCvt = t.ToString();
                if (szCvt != u32.ToString())
                {
                    // CAP is in its final form...
                    if (typeof(T) == typeof(CAP))
                    {
                        return (szCvt);
                    }
                    // Everybody else needs the name decoration removed...
                    else
                    {
                        return (typeof(T).Name + "_" + szCvt);
                    }
                }

                // We're probably a custom value...
                else
                {
                    return (string.Format("0x{0:X}", u32));
                }
            }

            // We're a string...
            return (a_szValue);
        }

        /// <summary>
        /// This mess is what tries to turn readable stuff into numeric constants...
        /// </summary>
        /// <typeparam name="T">type for the conversion</typeparam>
        /// <param name="a_szValue">value to convert</param>
        /// <returns></returns>
        internal static string CvtCapValueFromEnumHelper<T>(string a_szValue)
        {
            // We can figure this one out on our own...
            if ((typeof(T).Name == "bool") || (typeof(T).Name == "Boolean"))
            {
                return (((a_szValue.ToLowerInvariant() == "true") || (a_szValue == "1")) ? "1" : "0");
            }

            // Look for the enum prefix...
            if (a_szValue.ToLowerInvariant().StartsWith(typeof(T).Name.ToLowerInvariant() + "_"))
            {
                return (a_szValue.Substring((typeof(T).Name + "_").Length));
            }

            // Er...
            return (a_szValue);
        }

        /// <summary>
        /// This mess is what tries to turn readable stuff into numeric constants...
        /// </summary>
        /// <typeparam name="T">type for the conversion</typeparam>
        /// <param name="a_szValue">value to convert</param>
        /// <returns></returns>
        private static string CvtCapValueFromTwlg(string a_szValue)
        {
            // mono goes "hork", probably because the enum is wackadoodle, this
            // does work on .net, but what'cha gonna do?
            if (a_szValue.ToUpperInvariant().StartsWith("TWLG_"))
            {
                switch (a_szValue.ToUpperInvariant().Substring(5))
                {
                    default: break;
                    case "USERLOCALE": return ("65535"); // -1, kinda...
                    case "DAN": return ("0");
                    case "DUT": return ("1");
                    case "ENG": return ("2");
                    case "FCF": return ("3");
                    case "FIN": return ("4");
                    case "FRN": return ("5");
                    case "GER": return ("6");
                    case "ICE": return ("7");
                    case "ITN": return ("8");
                    case "NOR": return ("9");
                    case "POR": return ("10");
                    case "SPA": return ("11");
                    case "SWE": return ("12");
                    case "USA": return ("13");
                    case "AFRIKAANS": return ("14");
                    case "ALBANIA": return ("15");
                    case "ARABIC": return ("16");
                    case "ARABIC_ALGERIA": return ("17");
                    case "ARABIC_BAHRAIN": return ("18");
                    case "ARABIC_EGYPT": return ("19");
                    case "ARABIC_IRAQ": return ("20");
                    case "ARABIC_JORDAN": return ("21");
                    case "ARABIC_KUWAIT": return ("22");
                    case "ARABIC_LEBANON": return ("23");
                    case "ARABIC_LIBYA": return ("24");
                    case "ARABIC_MOROCCO": return ("25");
                    case "ARABIC_OMAN": return ("26");
                    case "ARABIC_QATAR": return ("27");
                    case "ARABIC_SAUDIARABIA": return ("28");
                    case "ARABIC_SYRIA": return ("29");
                    case "ARABIC_TUNISIA": return ("30");
                    case "ARABIC_UAE": return ("31");
                    case "ARABIC_YEMEN": return ("32");
                    case "BASQUE": return ("33");
                    case "BYELORUSSIAN": return ("34");
                    case "BULGARIAN": return ("35");
                    case "CATALAN": return ("36");
                    case "CHINESE": return ("37");
                    case "CHINESE_HONGKONG": return ("38");
                    case "CHINESE_PRC": return ("39");
                    case "CHINESE_SINGAPORE": return ("40");
                    case "CHINESE_SIMPLIFIED": return ("41");
                    case "CHINESE_TAIWAN": return ("42");
                    case "CHINESE_TRADITIONAL": return ("43");
                    case "CROATIA": return ("44");
                    case "CZECH": return ("45");
                    case "DANISH": return (((int)TWLG.DAN).ToString());
                    case "DUTCH": return (((int)TWLG.DUT).ToString());
                    case "DUTCH_BELGIAN": return ("46");
                    case "ENGLISH": return (((int)TWLG.ENG).ToString());
                    case "ENGLISH_AUSTRALIAN": return ("47");
                    case "ENGLISH_CANADIAN": return ("48");
                    case "ENGLISH_IRELAND": return ("49");
                    case "ENGLISH_NEWZEALAND": return ("50");
                    case "ENGLISH_SOUTHAFRICA": return ("51");
                    case "ENGLISH_UK": return ("52");
                    case "ENGLISH_USA": return (((int)TWLG.USA).ToString());
                    case "ESTONIAN": return ("53");
                    case "FAEROESE": return ("54");
                    case "FARSI": return ("55");
                    case "FINNISH": return (((int)TWLG.FIN).ToString());
                    case "FRENCH": return (((int)TWLG.FRN).ToString());
                    case "FRENCH_BELGIAN": return ("56");
                    case "FRENCH_CANADIAN": return (((int)TWLG.FCF).ToString());
                    case "FRENCH_LUXEMBOURG": return ("57");
                    case "FRENCH_SWISS": return ("58");
                    case "GERMAN": return (((int)TWLG.GER).ToString());
                    case "GERMAN_AUSTRIAN": return ("59");
                    case "GERMAN_LUXEMBOURG": return ("60");
                    case "GERMAN_LIECHTENSTEIN": return ("61");
                    case "GERMAN_SWISS": return ("62");
                    case "GREEK": return ("63");
                    case "HEBREW": return ("64");
                    case "HUNGARIAN": return ("65");
                    case "ICELANDIC": return (((int)TWLG.ICE).ToString());
                    case "INDONESIAN": return ("66");
                    case "ITALIAN": return (((int)TWLG.ITN).ToString());
                    case "ITALIAN_SWISS": return ("67");
                    case "JAPANESE": return ("68");
                    case "KOREAN": return ("69");
                    case "KOREAN_JOHAB": return ("70");
                    case "LATVIAN": return ("71");
                    case "LITHUANIAN": return ("72");
                    case "NORWEGIAN": return (((int)TWLG.NOR).ToString());
                    case "NORWEGIAN_BOKMAL": return ("73");
                    case "NORWEGIAN_NYNORSK": return ("74");
                    case "POLISH": return ("75");
                    case "PORTUGUESE": return (((int)TWLG.POR).ToString());
                    case "PORTUGUESE_BRAZIL": return ("76");
                    case "ROMANIAN": return ("77");
                    case "RUSSIAN": return ("78");
                    case "SERBIAN_LATIN": return ("79");
                    case "SLOVAK": return ("80");
                    case "SLOVENIAN": return ("81");
                    case "SPANISH": return (((int)TWLG.SPA).ToString());
                    case "SPANISH_MEXICAN": return ("82");
                    case "SPANISH_MODERN": return ("83");
                    case "SWEDISH": return (((int)TWLG.SWE).ToString());
                    case "THAI": return ("84");
                    case "TURKISH": return ("85");
                    case "UKRANIAN": return ("86");
                    case "ASSAMESE": return ("87");
                    case "BENGALI": return ("88");
                    case "BIHARI": return ("89");
                    case "BODO": return ("90");
                    case "DOGRI": return ("91");
                    case "GUJARATI": return ("92");
                    case "HARYANVI": return ("93");
                    case "HINDI": return ("94");
                    case "KANNADA": return ("95");
                    case "KASHMIRI": return ("96");
                    case "MALAYALAM": return ("97");
                    case "MARATHI": return ("98");
                    case "MARWARI": return ("99");
                    case "MEGHALAYAN": return ("100");
                    case "MIZO": return ("101");
                    case "NAGA": return ("102");
                    case "ORISSI": return ("103");
                    case "PUNJABI": return ("104");
                    case "PUSHTU": return ("105");
                    case "SERBIAN_CYRILLIC": return ("106");
                    case "SIKKIMI": return ("107");
                    case "SWEDISH_FINLAND": return ("108");
                    case "TAMIL": return ("109");
                    case "TELUGU": return ("110");
                    case "TRIPURI": return ("111");
                    case "URDU": return ("112");
                    case "VIETNAMESE": return ("113");
                }
            }

            // Er...
            return (a_szValue);
        }

        /// <summary>
        /// Convert a value to the 'friendly' name, based on the capability...
        /// </summary>
        /// <param name="a_cap">capability driving the conversion</param>
        /// <param name="szValue">value to convert</param>
        /// <returns></returns>
        public static string CvtCapValueToEnum(CAP a_cap, string a_szValue)
        {
            switch (a_cap)
            {
                default: return (a_szValue);
                case CAP.ACAP_XFERMECH: return (CvtCapValueToEnumHelper<TWSX>(a_szValue));
                case CAP.CAP_ALARMS: return (CvtCapValueToEnumHelper<TWAL>(a_szValue));
                case CAP.CAP_ALARMVOLUME: return (a_szValue);
                case CAP.CAP_AUTHOR: return (a_szValue);
                case CAP.CAP_AUTOFEED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOMATICCAPTURE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOMATICSENSEMEDIUM: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOSCAN: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_BATTERYMINUTES: return (a_szValue);
                case CAP.CAP_BATTERYPERCENTAGE: return (a_szValue);
                case CAP.CAP_CAMERAENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_CAMERAORDER: return (CvtCapValueToEnumHelper<TWPT>(a_szValue));
                case CAP.CAP_CAMERAPREVIEWUI: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_CAMERASIDE: return (CvtCapValueToEnumHelper<TWCS>(a_szValue));
                case CAP.CAP_CAPTION: return (a_szValue);
                case CAP.CAP_CLEARPAGE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_CUSTOMDSDATA: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_CUSTOMINTERFACEGUID: return (a_szValue);
                case CAP.CAP_DEVICEEVENT: return (CvtCapValueToEnumHelper<TWDE>(a_szValue));
                case CAP.CAP_DEVICEONLINE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_DEVICETIMEDATE: return (a_szValue);
                case CAP.CAP_DOUBLEFEEDDETECTION: return (CvtCapValueToEnumHelper<TWDF>(a_szValue));
                case CAP.CAP_DOUBLEFEEDDETECTIONLENGTH: return (a_szValue);
                case CAP.CAP_DOUBLEFEEDDETECTIONRESPONSE: return (CvtCapValueToEnumHelper<TWDP>(a_szValue));
                case CAP.CAP_DOUBLEFEEDDETECTIONSENSITIVITY: return (CvtCapValueToEnumHelper<TWUS>(a_szValue));
                case CAP.CAP_DUPLEX: return (CvtCapValueToEnumHelper<TWDX>(a_szValue));
                case CAP.CAP_DUPLEXENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_ENABLEDSUIONLY: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_ENDORSER: return (a_szValue);
                case CAP.CAP_EXTENDEDCAPS: return (CvtCapValueToEnumHelper<CAP>(a_szValue));
                case CAP.CAP_FEEDERALIGNMENT: return (CvtCapValueToEnumHelper<TWFA>(a_szValue));
                case CAP.CAP_FEEDERENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDERLOADED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDERORDER: return (CvtCapValueToEnumHelper<TWFO>(a_szValue));
                case CAP.CAP_FEEDERPOCKET: return (CvtCapValueToEnumHelper<TWFP>(a_szValue));
                case CAP.CAP_FEEDERPREP: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDPAGE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_INDICATORS: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_INDICATORSMODE: return (CvtCapValueToEnumHelper<TWCI>(a_szValue));
                case CAP.CAP_JOBCONTROL: return (CvtCapValueToEnumHelper<TWJC>(a_szValue));
                case CAP.CAP_LANGUAGE: return (CvtCapValueToEnumHelper<TWLG>(a_szValue));
                case CAP.CAP_MAXBATCHBUFFERS: return (a_szValue);
                case CAP.CAP_MICRENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_PAPERDETECTABLE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_PAPERHANDLING: return (CvtCapValueToEnumHelper<TWPH>(a_szValue));
                case CAP.CAP_POWERSAVETIME: return (a_szValue);
                case CAP.CAP_POWERSUPPLY: return (CvtCapValueToEnumHelper<TWPS>(a_szValue));
                case CAP.CAP_PRINTER: return (CvtCapValueToEnumHelper<TWPR>(a_szValue));
                case CAP.CAP_PRINTERCHARROTATION: return (a_szValue);
                case CAP.CAP_PRINTERENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_PRINTERFONTSTYLE: return (CvtCapValueToEnumHelper<TWPF>(a_szValue));
                case CAP.CAP_PRINTERINDEX: return (a_szValue);
                case CAP.CAP_PRINTERINDEXLEADCHAR: return (a_szValue);
                case CAP.CAP_PRINTERINDEXMAXVALUE: return (a_szValue);
                case CAP.CAP_PRINTERINDEXNUMDIGITS: return (a_szValue);
                case CAP.CAP_PRINTERINDEXSTEP: return (a_szValue);
                case CAP.CAP_PRINTERINDEXTRIGGER: return (CvtCapValueToEnumHelper<TWCT>(a_szValue));
                case CAP.CAP_PRINTERMODE: return (CvtCapValueToEnumHelper<TWPM>(a_szValue));
                case CAP.CAP_PRINTERSTRING: return (a_szValue);
                case CAP.CAP_PRINTERSTRINGPREVIEW: return (a_szValue);
                case CAP.CAP_PRINTERSUFFIX: return (a_szValue);
                case CAP.CAP_PRINTERVERTICALOFFSET: return (a_szValue);
                case CAP.CAP_REACQUIREALLOWED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_REWINDPAGE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_SEGMENTED: return (CvtCapValueToEnumHelper<TWSG>(a_szValue));
                case CAP.CAP_SERIALNUMBER: return (a_szValue);
                case CAP.CAP_SHEETCOUNT: return (a_szValue);
                case CAP.CAP_SUPPORTEDCAPS: return (CvtCapValueToEnumHelper<CAP>(a_szValue));
                case CAP.CAP_SUPPORTEDCAPSSEGMENTUNIQUE: return (CvtCapValueToEnumHelper<CAP>(a_szValue));
                case CAP.CAP_SUPPORTEDDATS: return (CvtCapValueToEnumHelper<DAT>(a_szValue));
                case CAP.CAP_THUMBNAILSENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_TIMEBEFOREFIRSTCAPTURE: return (a_szValue);
                case CAP.CAP_TIMEBETWEENCAPTURES: return (a_szValue);
                case CAP.CAP_TIMEDATE: return (a_szValue);
                case CAP.CAP_UICONTROLLABLE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.CAP_XFERCOUNT: return (a_szValue);
                case CAP.ICAP_AUTOBRIGHT: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTODISCARDBLANKPAGES: return (a_szValue);
                case CAP.ICAP_AUTOMATICBORDERDETECTION: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICCOLORENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE: return (CvtCapValueToEnumHelper<TWPT>(a_szValue));
                case CAP.ICAP_AUTOMATICCROPUSESFRAME: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICDESKEW: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICLENGTHDETECTION: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICROTATE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOSIZE: return (CvtCapValueToEnumHelper<TWAS>(a_szValue));
                case CAP.ICAP_BARCODEDETECTIONENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_BARCODEMAXRETRIES: return (a_szValue);
                case CAP.ICAP_BARCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case CAP.ICAP_BARCODESEARCHMODE: return (CvtCapValueToEnumHelper<TWBD>(a_szValue));
                case CAP.ICAP_BARCODESEARCHPRIORITIES: return (CvtCapValueToEnumHelper<TWBT>(a_szValue));
                case CAP.ICAP_BARCODETIMEOUT: return (a_szValue);
                case CAP.ICAP_BITDEPTH: return (a_szValue);
                case CAP.ICAP_BITDEPTHREDUCTION: return (CvtCapValueToEnumHelper<TWBR>(a_szValue));
                case CAP.ICAP_BITORDER: return (CvtCapValueToEnumHelper<TWBO>(a_szValue));
                case CAP.ICAP_BITORDERCODES: return (CvtCapValueToEnumHelper<TWBO>(a_szValue));
                case CAP.ICAP_BRIGHTNESS: return (a_szValue);
                case CAP.ICAP_CCITTKFACTOR: return (a_szValue);
                case CAP.ICAP_COLORMANAGEMENTENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_COMPRESSION: return (CvtCapValueToEnumHelper<TWCP>(a_szValue));
                case CAP.ICAP_CONTRAST: return (a_szValue);
                case CAP.ICAP_CUSTHALFTONE: return (a_szValue);
                case CAP.ICAP_EXPOSURETIME: return (a_szValue);
                case CAP.ICAP_EXTIMAGEINFO: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_FEEDERTYPE: return (CvtCapValueToEnumHelper<TWFE>(a_szValue));
                case CAP.ICAP_FILMTYPE: return (CvtCapValueToEnumHelper<TWFM>(a_szValue));
                case CAP.ICAP_FILTER: return (CvtCapValueToEnumHelper<TWFT>(a_szValue));
                case CAP.ICAP_FLASHUSED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_FLASHUSED2: return (CvtCapValueToEnumHelper<TWFL>(a_szValue));
                case CAP.ICAP_FLIPROTATION: return (CvtCapValueToEnumHelper<TWFR>(a_szValue));
                case CAP.ICAP_FRAMES: return (a_szValue);
                case CAP.ICAP_GAMMA: return (a_szValue);
                case CAP.ICAP_HALFTONES: return (a_szValue);
                case CAP.ICAP_HIGHLIGHT: return (a_szValue);
                case CAP.ICAP_ICCPROFILE: return (CvtCapValueToEnumHelper<TWIC>(a_szValue));
                case CAP.ICAP_IMAGEDATASET: return (a_szValue);
                case CAP.ICAP_IMAGEFILEFORMAT: return (CvtCapValueToEnumHelper<TWFF>(a_szValue));
                case CAP.ICAP_IMAGEFILTER: return (CvtCapValueToEnumHelper<TWIF>(a_szValue));
                case CAP.ICAP_IMAGEMERGE: return (CvtCapValueToEnumHelper<TWIM>(a_szValue));
                case CAP.ICAP_IMAGEMERGEHEIGHTTHRESHOLD: return (a_szValue);
                case CAP.ICAP_JPEGPIXELTYPE: return (CvtCapValueToEnumHelper<TWPT>(a_szValue));
                case CAP.ICAP_JPEGQUALITY: return (CvtCapValueToEnumHelper<TWJQ>(a_szValue));
                case CAP.ICAP_JPEGSUBSAMPLING: return (CvtCapValueToEnumHelper<TWJS>(a_szValue));
                case CAP.ICAP_LAMPSTATE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_LIGHTPATH: return (CvtCapValueToEnumHelper<TWLP>(a_szValue));
                case CAP.ICAP_LIGHTSOURCE: return (CvtCapValueToEnumHelper<TWLS>(a_szValue));
                case CAP.ICAP_MAXFRAMES: return (a_szValue);
                case CAP.ICAP_MINIMUMHEIGHT: return (a_szValue);
                case CAP.ICAP_MINIMUMWIDTH: return (a_szValue);
                case CAP.ICAP_MIRROR: return (CvtCapValueToEnumHelper<TWMR>(a_szValue));
                case CAP.ICAP_NOISEFILTER: return (CvtCapValueToEnumHelper<TWNF>(a_szValue));
                case CAP.ICAP_ORIENTATION: return (CvtCapValueToEnumHelper<TWOR>(a_szValue));
                case CAP.ICAP_OVERSCAN: return (CvtCapValueToEnumHelper<TWOV>(a_szValue));
                case CAP.ICAP_PATCHCODEDETECTIONENABLED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_PATCHCODEMAXRETRIES: return (a_szValue);
                case CAP.ICAP_PATCHCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case CAP.ICAP_PATCHCODESEARCHMODE: return (CvtCapValueToEnumHelper<TWBD>(a_szValue));
                case CAP.ICAP_PATCHCODESEARCHPRIORITIES: return (CvtCapValueToEnumHelper<TWPCH>(a_szValue));
                case CAP.ICAP_PATCHCODETIMEOUT: return (a_szValue);
                case CAP.ICAP_PHYSICALHEIGHT: return (a_szValue);
                case CAP.ICAP_PHYSICALWIDTH: return (a_szValue);
                case CAP.ICAP_PIXELFLAVOR: return (CvtCapValueToEnumHelper<TWPF>(a_szValue));
                case CAP.ICAP_PIXELFLAVORCODES: return (CvtCapValueToEnumHelper<TWPF>(a_szValue));
                case CAP.ICAP_PIXELTYPE: return (CvtCapValueToEnumHelper<TWPT>(a_szValue));
                case CAP.ICAP_PLANARCHUNKY: return (CvtCapValueToEnumHelper<TWPC>(a_szValue));
                case CAP.ICAP_ROTATION: return (a_szValue);
                case CAP.ICAP_SHADOW: return (a_szValue);
                case CAP.ICAP_SUPPORTEDBARCODETYPES: return (CvtCapValueToEnumHelper<TWBT>(a_szValue));
                case CAP.ICAP_SUPPORTEDEXTIMAGEINFO: return (CvtCapValueToEnumHelper<TWEI>(a_szValue));
                case CAP.ICAP_SUPPORTEDPATCHCODETYPES: return (CvtCapValueToEnumHelper<TWPCH>(a_szValue));
                case CAP.ICAP_SUPPORTEDSIZES: return (CvtCapValueToEnumHelper<TWSS>(a_szValue));
                case CAP.ICAP_THRESHOLD: return (a_szValue);
                case CAP.ICAP_TILES: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_TIMEFILL: return (a_szValue);
                case CAP.ICAP_UNDEFINEDIMAGESIZE: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case CAP.ICAP_UNITS: return (CvtCapValueToEnumHelper<TWUN>(a_szValue));
                case CAP.ICAP_XFERMECH: return (CvtCapValueToEnumHelper<TWSX>(a_szValue));
                case CAP.ICAP_XNATIVERESOLUTION: return (a_szValue);
                case CAP.ICAP_XRESOLUTION: return (a_szValue);
                case CAP.ICAP_XSCALING: return (a_szValue);
                case CAP.ICAP_YNATIVERESOLUTION: return (a_szValue);
                case CAP.ICAP_YRESOLUTION: return (a_szValue);
                case CAP.ICAP_YSCALING: return (a_szValue);
                case CAP.ICAP_ZOOMFACTOR: return (a_szValue);
            }
        }

        /// <summary>
        /// Convert a 'friendly' name to a numeric value...
        /// </summary>
        /// <param name="a_cap">capability driving the conversion</param>
        /// <param name="szValue">value to convert</param>
        /// <returns></returns>
        public static string CvtCapValueFromEnum(CAP a_cap, string a_szValue)
        {
            int ii;

            // Turn hex into a decimal...
            if (a_szValue.ToLowerInvariant().StartsWith("0x"))
            {
                return (int.Parse(a_szValue.Substring(2), NumberStyles.HexNumber).ToString());
            }

            // Skip numbers...
            if (int.TryParse(a_szValue, out ii))
            {
                return (a_szValue);
            }

            // Process text...
            switch (a_cap)
            {
                default: return (a_szValue);
                case CAP.ACAP_XFERMECH: { TWSX twsx; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWSX>(a_szValue), out twsx) ? ((int)twsx).ToString() : a_szValue); };
                case CAP.CAP_ALARMS: { TWAL twal; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWAL>(a_szValue), out twal) ? ((int)twal).ToString() : a_szValue); };
                case CAP.CAP_ALARMVOLUME: return (a_szValue);
                case CAP.CAP_AUTHOR: return (a_szValue);
                case CAP.CAP_AUTOFEED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOMATICCAPTURE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOMATICSENSEMEDIUM: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_AUTOSCAN: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_BATTERYMINUTES: return (a_szValue);
                case CAP.CAP_BATTERYPERCENTAGE: return (a_szValue);
                case CAP.CAP_CAMERAENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_CAMERAORDER: { TWPT twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPT>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case CAP.CAP_CAMERAPREVIEWUI: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_CAMERASIDE: { TWCS twcs; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWCS>(a_szValue), out twcs) ? ((int)twcs).ToString() : a_szValue); };
                case CAP.CAP_CAPTION: return (a_szValue);
                case CAP.CAP_CLEARPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_CUSTOMDSDATA: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_CUSTOMINTERFACEGUID: return (a_szValue);
                case CAP.CAP_DEVICEEVENT: { TWDE twde; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWDE>(a_szValue), out twde) ? ((int)twde).ToString() : a_szValue); };
                case CAP.CAP_DEVICEONLINE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_DEVICETIMEDATE: return (a_szValue);
                case CAP.CAP_DOUBLEFEEDDETECTION: { TWDF twdf; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWDF>(a_szValue), out twdf) ? ((int)twdf).ToString() : a_szValue); };
                case CAP.CAP_DOUBLEFEEDDETECTIONLENGTH: return (a_szValue);
                case CAP.CAP_DOUBLEFEEDDETECTIONRESPONSE: { TWDP twdp; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWDP>(a_szValue), out twdp) ? ((int)twdp).ToString() : a_szValue); };
                case CAP.CAP_DOUBLEFEEDDETECTIONSENSITIVITY: { TWUS twus; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWUS>(a_szValue), out twus) ? ((int)twus).ToString() : a_szValue); };
                case CAP.CAP_DUPLEX: { TWDX twdx; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWDX>(a_szValue), out twdx) ? ((int)twdx).ToString() : a_szValue); };
                case CAP.CAP_DUPLEXENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_ENABLEDSUIONLY: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_ENDORSER: return (a_szValue);
                case CAP.CAP_EXTENDEDCAPS: { CAP cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case CAP.CAP_FEEDERALIGNMENT: { TWFA twfa; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFA>(a_szValue), out twfa) ? ((int)twfa).ToString() : a_szValue); };
                case CAP.CAP_FEEDERENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDERLOADED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDERORDER: { TWFO twfo; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFO>(a_szValue), out twfo) ? ((int)twfo).ToString() : a_szValue); };
                case CAP.CAP_FEEDERPOCKET: { TWFP twfp; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFP>(a_szValue), out twfp) ? ((int)twfp).ToString() : a_szValue); };
                case CAP.CAP_FEEDERPREP: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_FEEDPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_INDICATORS: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_INDICATORSMODE: { TWCI twci; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWCI>(a_szValue), out twci) ? ((int)twci).ToString() : a_szValue); };
                case CAP.CAP_JOBCONTROL: { TWJC twjc; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWJC>(a_szValue), out twjc) ? ((int)twjc).ToString() : a_szValue); };
                case CAP.CAP_LANGUAGE: return (CvtCapValueFromTwlg(a_szValue));
                case CAP.CAP_MAXBATCHBUFFERS: return (a_szValue);
                case CAP.CAP_MICRENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_PAPERDETECTABLE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_PAPERHANDLING: { TWPH twph; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPH>(a_szValue), out twph) ? ((int)twph).ToString() : a_szValue); };
                case CAP.CAP_POWERSAVETIME: return (a_szValue);
                case CAP.CAP_POWERSUPPLY: { TWPS twps; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPS>(a_szValue), out twps) ? ((int)twps).ToString() : a_szValue); };
                case CAP.CAP_PRINTER: { TWPR twpr; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPR>(a_szValue), out twpr) ? ((int)twpr).ToString() : a_szValue); };
                case CAP.CAP_PRINTERCHARROTATION: return (a_szValue);
                case CAP.CAP_PRINTERENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_PRINTERFONTSTYLE: { TWPF twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPF>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case CAP.CAP_PRINTERINDEX: return (a_szValue);
                case CAP.CAP_PRINTERINDEXLEADCHAR: return (a_szValue);
                case CAP.CAP_PRINTERINDEXMAXVALUE: return (a_szValue);
                case CAP.CAP_PRINTERINDEXNUMDIGITS: return (a_szValue);
                case CAP.CAP_PRINTERINDEXSTEP: return (a_szValue);
                case CAP.CAP_PRINTERINDEXTRIGGER: { TWCT twct; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWCT>(a_szValue), out twct) ? ((int)twct).ToString() : a_szValue); };
                case CAP.CAP_PRINTERMODE: { TWPM twpm; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPM>(a_szValue), out twpm) ? ((int)twpm).ToString() : a_szValue); };
                case CAP.CAP_PRINTERSTRING: return (a_szValue);
                case CAP.CAP_PRINTERSTRINGPREVIEW: return (a_szValue);
                case CAP.CAP_PRINTERSUFFIX: return (a_szValue);
                case CAP.CAP_PRINTERVERTICALOFFSET: return (a_szValue);
                case CAP.CAP_REACQUIREALLOWED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_REWINDPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_SEGMENTED: { TWSG twsg; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWSG>(a_szValue), out twsg) ? ((int)twsg).ToString() : a_szValue); };
                case CAP.CAP_SERIALNUMBER: return (a_szValue);
                case CAP.CAP_SHEETCOUNT: return (a_szValue);
                case CAP.CAP_SUPPORTEDCAPS: { CAP cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case CAP.CAP_SUPPORTEDCAPSSEGMENTUNIQUE: { CAP cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case CAP.CAP_SUPPORTEDDATS: { DAT dat; return (Enum.TryParse(CvtCapValueFromEnumHelper<DAT>(a_szValue), out dat) ? ((int)dat).ToString() : a_szValue); };
                case CAP.CAP_THUMBNAILSENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_TIMEBEFOREFIRSTCAPTURE: return (a_szValue);
                case CAP.CAP_TIMEBETWEENCAPTURES: return (a_szValue);
                case CAP.CAP_TIMEDATE: return (a_szValue);
                case CAP.CAP_UICONTROLLABLE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.CAP_XFERCOUNT: return (a_szValue);
                case CAP.ICAP_AUTOBRIGHT: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTODISCARDBLANKPAGES: { TWBP twbp; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBP>(a_szValue), out twbp) ? ((int)twbp).ToString() : a_szValue); };
                case CAP.ICAP_AUTOMATICBORDERDETECTION: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICCOLORENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE: { TWPT twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPT>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case CAP.ICAP_AUTOMATICCROPUSESFRAME: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICDESKEW: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICLENGTHDETECTION: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOMATICROTATE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_AUTOSIZE: { TWAS twas; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWAS>(a_szValue), out twas) ? ((int)twas).ToString() : a_szValue); };
                case CAP.ICAP_BARCODEDETECTIONENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_BARCODEMAXRETRIES: return (a_szValue);
                case CAP.ICAP_BARCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case CAP.ICAP_BARCODESEARCHMODE: { TWBD twbd; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBD>(a_szValue), out twbd) ? ((int)twbd).ToString() : a_szValue); };
                case CAP.ICAP_BARCODESEARCHPRIORITIES: { TWBT twbt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBT>(a_szValue), out twbt) ? ((int)twbt).ToString() : a_szValue); };
                case CAP.ICAP_BARCODETIMEOUT: return (a_szValue);
                case CAP.ICAP_BITDEPTH: return (a_szValue);
                case CAP.ICAP_BITDEPTHREDUCTION: { TWBR twbr; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBR>(a_szValue), out twbr) ? ((int)twbr).ToString() : a_szValue); };
                case CAP.ICAP_BITORDER: { TWBO twbo; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBO>(a_szValue), out twbo) ? ((int)twbo).ToString() : a_szValue); };
                case CAP.ICAP_BITORDERCODES: { TWBO twbo; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBO>(a_szValue), out twbo) ? ((int)twbo).ToString() : a_szValue); };
                case CAP.ICAP_BRIGHTNESS: return (a_szValue);
                case CAP.ICAP_CCITTKFACTOR: return (a_szValue);
                case CAP.ICAP_COLORMANAGEMENTENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_COMPRESSION: { TWCP twcp; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWCP>(a_szValue), out twcp) ? ((int)twcp).ToString() : a_szValue); };
                case CAP.ICAP_CONTRAST: return (a_szValue);
                case CAP.ICAP_CUSTHALFTONE: return (a_szValue);
                case CAP.ICAP_EXPOSURETIME: return (a_szValue);
                case CAP.ICAP_EXTIMAGEINFO: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_FEEDERTYPE: { TWFE twfe; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFE>(a_szValue), out twfe) ? ((int)twfe).ToString() : a_szValue); };
                case CAP.ICAP_FILMTYPE: { TWFM twfm; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFM>(a_szValue), out twfm) ? ((int)twfm).ToString() : a_szValue); };
                case CAP.ICAP_FILTER: { TWFT twft; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFT>(a_szValue), out twft) ? ((int)twft).ToString() : a_szValue); };
                case CAP.ICAP_FLASHUSED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_FLASHUSED2: { TWFL twfl; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFL>(a_szValue), out twfl) ? ((int)twfl).ToString() : a_szValue); };
                case CAP.ICAP_FLIPROTATION: { TWFR twfr; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFR>(a_szValue), out twfr) ? ((int)twfr).ToString() : a_szValue); };
                case CAP.ICAP_FRAMES: return (a_szValue);
                case CAP.ICAP_GAMMA: return (a_szValue);
                case CAP.ICAP_HALFTONES: return (a_szValue);
                case CAP.ICAP_HIGHLIGHT: return (a_szValue);
                case CAP.ICAP_ICCPROFILE: { TWIC twic; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWIC>(a_szValue), out twic) ? ((int)twic).ToString() : a_szValue); };
                case CAP.ICAP_IMAGEDATASET: return (a_szValue);
                case CAP.ICAP_IMAGEFILEFORMAT: { TWFF twff; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWFF>(a_szValue), out twff) ? ((int)twff).ToString() : a_szValue); };
                case CAP.ICAP_IMAGEFILTER: { TWIF twif; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWIF>(a_szValue), out twif) ? ((int)twif).ToString() : a_szValue); };
                case CAP.ICAP_IMAGEMERGE: { TWIM twim; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWIM>(a_szValue), out twim) ? ((int)twim).ToString() : a_szValue); };
                case CAP.ICAP_IMAGEMERGEHEIGHTTHRESHOLD: return (a_szValue);
                case CAP.ICAP_JPEGPIXELTYPE: { TWPT twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPT>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case CAP.ICAP_JPEGQUALITY: { TWJQ twjq; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWJQ>(a_szValue), out twjq) ? ((int)twjq).ToString() : a_szValue); };
                case CAP.ICAP_JPEGSUBSAMPLING: { TWJS twjs; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWJS>(a_szValue), out twjs) ? ((int)twjs).ToString() : a_szValue); };
                case CAP.ICAP_LAMPSTATE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_LIGHTPATH: { TWLP twlp; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWLP>(a_szValue), out twlp) ? ((int)twlp).ToString() : a_szValue); };
                case CAP.ICAP_LIGHTSOURCE: { TWLS twls; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWLS>(a_szValue), out twls) ? ((int)twls).ToString() : a_szValue); };
                case CAP.ICAP_MAXFRAMES: return (a_szValue);
                case CAP.ICAP_MINIMUMHEIGHT: return (a_szValue);
                case CAP.ICAP_MINIMUMWIDTH: return (a_szValue);
                case CAP.ICAP_MIRROR: { TWMR twmr; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWMR>(a_szValue), out twmr) ? ((int)twmr).ToString() : a_szValue); };
                case CAP.ICAP_NOISEFILTER: { TWNF twnf; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWNF>(a_szValue), out twnf) ? ((int)twnf).ToString() : a_szValue); };
                case CAP.ICAP_ORIENTATION: { TWOR twor; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWOR>(a_szValue), out twor) ? ((int)twor).ToString() : a_szValue); };
                case CAP.ICAP_OVERSCAN: { TWOV twov; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWOV>(a_szValue), out twov) ? ((int)twov).ToString() : a_szValue); };
                case CAP.ICAP_PATCHCODEDETECTIONENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_PATCHCODEMAXRETRIES: return (a_szValue);
                case CAP.ICAP_PATCHCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case CAP.ICAP_PATCHCODESEARCHMODE: { TWBD twbd; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBD>(a_szValue), out twbd) ? ((int)twbd).ToString() : a_szValue); };
                case CAP.ICAP_PATCHCODESEARCHPRIORITIES: { TWPCH twpch; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPCH>(a_szValue), out twpch) ? ((int)twpch).ToString() : a_szValue); };
                case CAP.ICAP_PATCHCODETIMEOUT: return (a_szValue);
                case CAP.ICAP_PHYSICALHEIGHT: return (a_szValue);
                case CAP.ICAP_PHYSICALWIDTH: return (a_szValue);
                case CAP.ICAP_PIXELFLAVOR: { TWPF twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPF>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case CAP.ICAP_PIXELFLAVORCODES: { TWPF twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPF>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case CAP.ICAP_PIXELTYPE: { TWPT twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPT>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case CAP.ICAP_PLANARCHUNKY: { TWPC twpc; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPC>(a_szValue), out twpc) ? ((int)twpc).ToString() : a_szValue); };
                case CAP.ICAP_ROTATION: return (a_szValue);
                case CAP.ICAP_SHADOW: return (a_szValue);
                case CAP.ICAP_SUPPORTEDBARCODETYPES: { TWBT twbt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBT>(a_szValue), out twbt) ? ((int)twbt).ToString() : a_szValue); };
                case CAP.ICAP_SUPPORTEDEXTIMAGEINFO: { TWEI twei; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWEI>(a_szValue), out twei) ? ((int)twei).ToString() : a_szValue); };
                case CAP.ICAP_SUPPORTEDPATCHCODETYPES: { TWPCH twpch; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWPCH>(a_szValue), out twpch) ? ((int)twpch).ToString() : a_szValue); };
                case CAP.ICAP_SUPPORTEDSIZES: { TWSS twss; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWSS>(a_szValue), out twss) ? ((int)twss).ToString() : a_szValue); };
                case CAP.ICAP_THRESHOLD: return (a_szValue);
                case CAP.ICAP_TILES: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_TIMEFILL: return (a_szValue);
                case CAP.ICAP_UNDEFINEDIMAGESIZE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case CAP.ICAP_UNITS: { TWUN twun; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWUN>(a_szValue), out twun) ? ((int)twun).ToString() : a_szValue); };
                case CAP.ICAP_XFERMECH: { TWSX twsx; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWSX>(a_szValue), out twsx) ? ((int)twsx).ToString() : a_szValue); };
                case CAP.ICAP_XNATIVERESOLUTION: return (a_szValue);
                case CAP.ICAP_XRESOLUTION: return (a_szValue);
                case CAP.ICAP_XSCALING: return (a_szValue);
                case CAP.ICAP_YNATIVERESOLUTION: return (a_szValue);
                case CAP.ICAP_YRESOLUTION: return (a_szValue);
                case CAP.ICAP_YSCALING: return (a_szValue);
                case CAP.ICAP_ZOOMFACTOR: return (a_szValue);
            }
        }

    }
}
