using NTwain.Data;
using NTwain.Values;
using NTwain.Values.Cap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Defines common methods on <see cref="TwainSession"/> using the raw
    /// TWAIN triplet api.
    /// </summary>
    public static class TwainSessionExtensions
    {
        /// <summary>
        /// Gets the manager status. Only call this at state 2 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static TWStatus GetManagerStatus(this TwainSession session)
        {
            TWStatus stat;
            session.DGControl.Status.GetManager(out stat);
            return stat;
        }
        /// <summary>
        /// Gets the source status. Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static TWStatus GetSourceStatus(this TwainSession session)
        {
            TWStatus stat;
            session.DGControl.Status.GetSource(out stat);
            return stat;
        }

        /// <summary>
        /// Gets list of sources available in the system.
        /// Only call this at state 2 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<TWIdentity> GetSources(this TwainSession session)
        {
            List<TWIdentity> list = new List<TWIdentity>();

            // now enumerate
            TWIdentity srcId;
            var rc = session.DGControl.Identity.GetFirst(out srcId);
            if (rc == ReturnCode.Success) { list.Add(srcId); }
            do
            {
                rc = session.DGControl.Identity.GetNext(out srcId);
                if (rc == ReturnCode.Success)
                {
                    list.Add(srcId);
                }
            } while (rc == ReturnCode.Success);

            return list;
        }

        #region common caps

        /// <summary>
        /// Gets the current value for a general capability. This only works for types that are under 32bit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        public static T GetCurrentCap<T>(this TwainSession session, CapabilityId capId) where T : struct,IConvertible
        {
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = session.DGControl.Capability.GetCurrent(cap);
                if (rc == ReturnCode.Success)
                {
                    switch (cap.ContainerType)
                    {
                        case ContainerType.Enum:
                            var enu = cap.GetEnumValue();
                            if (enu.ItemType < ItemType.Frame)
                            {
                                // does this work?
                                return ConvertValueToType<T>(enu.ItemList[enu.CurrentIndex].ToString(), true);
                            }
                            break;
                        case ContainerType.OneValue:
                            var one = cap.GetOneValue();
                            if (one.ItemType < ItemType.Frame)
                            {
                                return ConvertValueToType<T>(one.Item, true);
                            }
                            break;
                        case ContainerType.Range:
                            var range = cap.GetRangeValue();
                            if (range.ItemType < ItemType.Frame)
                            {
                                return ConvertValueToType<T>(range.CurrentValue, true);
                            }
                            break;
                    }
                }
            }
            return default(T);
        }

        static ushort GetLowerWord(uint value)
        {
            return (ushort)(value & 0xffff);
        }
        static uint GetUpperWord(uint value)
        {
            return (ushort)(value >> 16);
        }

        static T ConvertValueToType<T>(object value, bool tryUpperWord) where T : struct,IConvertible
        {
            var returnType = typeof(T);
            if (returnType.IsEnum)
            {
                if (tryUpperWord)
                {
                    // small routine to see if works with bad sources that put
                    // 16bit value in the upper word instead of lower word.
                    var rawType = Enum.GetUnderlyingType(returnType);
                    if (typeof(ushort).IsAssignableFrom(rawType))
                    {
                        var intVal = Convert.ToUInt32(value);
                        var enumVal = GetLowerWord(intVal);
                        if (!Enum.IsDefined(returnType, enumVal))
                        {
                            return (T)Enum.ToObject(returnType, GetUpperWord(intVal));
                        }
                    }
                }
                // this may work better?
                return (T)Enum.ToObject(returnType, value);
                //// cast to underlying type first then to the enum
                //return (T)Convert.ChangeType(value, rawType);
            }
            return (T)Convert.ChangeType(value, returnType);
        }

        /// <summary>
        /// A generic method that returns the data in a <see cref="TWCapability" />.
        /// </summary>
        /// <typeparam name="TCapVal">The expected capability value type.</typeparam>
        /// <param name="capability">The capability returned from the source.</param>
        /// <param name="toPopulate">The list to populate if necessary.</param>
        /// <returns></returns>
        public static IList<TCapVal> ReadMultiCapValues<TCapVal>(this TWCapability capability, IList<TCapVal> toPopulate) where TCapVal : struct,IConvertible
        {
            return ReadMultiCapValues<TCapVal>(capability, toPopulate, true);
        }
        static IList<TCapVal> ReadMultiCapValues<TCapVal>(this TWCapability capability, IList<TCapVal> toPopulate, bool tryUpperWord) where TCapVal : struct,IConvertible
        {
            if (toPopulate == null) { toPopulate = new List<TCapVal>(); }

            switch (capability.ContainerType)
            {
                case ContainerType.OneValue:
                    var value = capability.GetOneValue();
                    if (value != null)
                    {
                        var val = ConvertValueToType<TCapVal>(value.Item, tryUpperWord);// (T)Convert.ToUInt16(value.Item);
                        toPopulate.Add(val);
                    }
                    break;
                case ContainerType.Array:
                    var arr = capability.GetArrayValue();
                    if (arr != null && arr.ItemList != null)
                    {
                        for (int i = 0; i < arr.ItemList.Length; i++)
                        {
                            var val = ConvertValueToType<TCapVal>(arr.ItemList[i], tryUpperWord);// (T)Convert.ToUInt16(enumr.ItemList[i]);
                            toPopulate.Add(val);
                        }
                    }
                    break;
                case ContainerType.Enum:
                    var enumr = capability.GetEnumValue();
                    if (enumr != null && enumr.ItemList != null)
                    {
                        for (int i = 0; i < enumr.ItemList.Length; i++)
                        {
                            var val = ConvertValueToType<TCapVal>(enumr.ItemList[i], tryUpperWord);// (T)Convert.ToUInt16(enumr.ItemList[i]);
                            toPopulate.Add(val);
                        }
                    }
                    break;
                case ContainerType.Range:
                    var range = capability.GetRangeValue();
                    if (range != null)
                    {
                        for (uint i = range.MinValue; i < range.MaxValue; i += range.StepSize)
                        {
                            var val = ConvertValueToType<TCapVal>(i, tryUpperWord);
                            toPopulate.Add(val);
                        }
                    }
                    break;
            }
            return toPopulate;
        }

        /// <summary>
        /// A generic method that tries to get capability values from current <see cref="TwainSession" />.
        /// </summary>
        /// <typeparam name="TCapVal">The expected capability value type.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="capabilityId">The capability unique identifier.</param>
        /// <param name="tryUpperWord">if set to <c>true</c> then apply to workaround for certain bad sources.</param>
        /// <returns></returns>
        public static IList<TCapVal> GetCapabilityValues<TCapVal>(this TwainSession session, CapabilityId capabilityId, bool tryUpperWord) where TCapVal : struct,IConvertible
        {
            var list = new List<TCapVal>();
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = session.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    cap.ReadMultiCapValues<TCapVal>(list, tryUpperWord);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets list of capabilities supported by current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        internal static IList<CapabilityId> GetCapabilities(this TwainSession session)
        {
            return session.GetCapabilityValues<CapabilityId>(CapabilityId.CapSupportedCaps, false);
        }

        #region xfer mech

        /// <summary>
        /// Gets the supported image <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<XferMech> CapGetImageXferMech(this TwainSession session)
        {
            return session.GetCapabilityValues<XferMech>(CapabilityId.ICapXferMech, true);
        }

        #endregion

        #region compression

        /// <summary>
        /// Gets the supported <see cref="Compression"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<Compression> CapGetCompression(this TwainSession session)
        {
            return session.GetCapabilityValues<Compression>(CapabilityId.ICapCompression, true);
        }

        /// <summary>
        /// Change the image compression for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="compression">The compression.</param>
        /// <returns></returns>
        public static ReturnCode CapSetImageCompression(this TwainSession session, Compression compression)
        {
            using (TWCapability compressCap = new TWCapability(CapabilityId.ICapCompression, new TWOneValue { Item = (uint)compression, ItemType = Values.ItemType.UInt16 }))
            {
                return session.DGControl.Capability.Set(compressCap);
            }
        }

        #endregion

        #region image format

        /// <summary>
        /// Gets the supported <see cref="ImageFileFormat"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<ImageFileFormat> CapGetImageFileFormat(this TwainSession session)
        {
            return session.GetCapabilityValues<ImageFileFormat>(CapabilityId.ICapImageFileFormat, true);
        }

        /// <summary>
        /// Change the image format for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static ReturnCode CapSetImageFormat(this TwainSession session, ImageFileFormat format)
        {
            using (TWCapability formatCap = new TWCapability(CapabilityId.ICapImageFileFormat, new TWOneValue { Item = (uint)format, ItemType = Values.ItemType.UInt16 }))
            {
                return session.DGControl.Capability.Set(formatCap);
            }
        }

        #endregion

        #region pixel type

        /// <summary>
        /// Gets the supported <see cref="PixelType"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<PixelType> CapGetPixelTypes(this TwainSession session)
        {
            return session.GetCapabilityValues<PixelType>(CapabilityId.ICapPixelType, true);
        }

        /// <summary>
        /// Change the pixel type for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ReturnCode CapSetPixelType(this TwainSession session, PixelType type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ICapPixelType, one))
            {
                return session.DGControl.Capability.Set(dx);
            }
        }

        #endregion

        #region dpi

        /// <summary>
        /// Gets the supported DPI values for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<int> CapGetDPIs(this TwainSession session)
        {
            return session.GetCapabilityValues<int>(CapabilityId.ICapXResolution, true);
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="dpi">The DPI.</param>
        /// <returns></returns>
        public static ReturnCode CapSetDPI(this TwainSession session, int dpi)
        {
            return CapSetDPI(session, dpi, dpi);
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="xDPI">The x DPI.</param>
        /// <param name="yDPI">The y DPI.</param>
        /// <returns></returns>
        public static ReturnCode CapSetDPI(this TwainSession session, int xDPI, int yDPI)
        {
            TWOneValue one = new TWOneValue();
            one.Item = (uint)xDPI;// ((uint)dpi) << 16;
            one.ItemType = ItemType.Fix32;

            using (TWCapability xres = new TWCapability(CapabilityId.ICapXResolution, one))
            {
                var rc = session.DGControl.Capability.Set(xres);
                if (rc == ReturnCode.Success)
                {
                    one.Item = (uint)yDPI;
                    using (TWCapability yres = new TWCapability(CapabilityId.ICapYResolution, one))
                    {
                        rc = session.DGControl.Capability.Set(yres);
                    }
                }
                return rc;
            }
        }

        #endregion

        #region supported paper size

        /// <summary>
        /// Gets the supported <see cref="SupportedSize"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<SupportedSize> CapGetSupportedSizes(this TwainSession session)
        {
            return session.GetCapabilityValues<SupportedSize>(CapabilityId.ICapSupportedSizes, true);
        }

        /// <summary>
        /// Change the supported paper size for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static ReturnCode CapSetSupportedSize(this TwainSession session, SupportedSize size)
        {
            var one = new TWOneValue();
            one.Item = (uint)size;
            one.ItemType = ItemType.UInt16;

            using (TWCapability xres = new TWCapability(CapabilityId.ICapSupportedSizes, one))
            {
                var rc = session.DGControl.Capability.Set(xres);
                return rc;
            }
        }

        #endregion

        #region onesie flags

        /// <summary>
        /// Change the auto deskew flag for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="useIt">if set to <c>true</c> [use it].</param>
        /// <returns></returns>
        public static ReturnCode CapSetAutoDeskew(this TwainSession session, bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (session.SupportedCaps.Contains(CapabilityId.ICapAutomaticDeskew))
            {

                if (session.SourceId.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticDeskew, en))
                    {
                        rc = session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticDeskew, one))
                    {
                        rc = session.DGControl.Capability.Set(capValue);
                    }
                }
            }

            return rc;
        }

        /// <summary>
        /// Change the auto rotate flag for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="useIt">if set to <c>true</c> [use it].</param>
        /// <returns></returns>
        public static ReturnCode CapSetAutoRotate(this TwainSession session, bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (session.SupportedCaps.Contains(CapabilityId.ICapAutomaticRotate))
            {
                if (session.SourceId.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticRotate, en))
                    {
                        rc = session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticRotate, one))
                    {
                        rc = session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// Change the auto border detection flag for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="useIt">if set to <c>true</c> [use it].</param>
        /// <returns></returns>
        public static ReturnCode CapSetBorderDetection(this TwainSession session, bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (session.SupportedCaps.Contains(CapabilityId.ICapAutomaticBorderDetection))
            {
                // this goes along with undefinedimagesize so that also
                // needs to be set
                if (session.SourceId.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapUndefinedImageSize, en))
                    {
                        rc = session.DGControl.Capability.Set(dx);
                    }
                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, en))
                    {
                        rc = session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapUndefinedImageSize, one))
                    {
                        rc = session.DGControl.Capability.Set(capValue);
                    }
                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                    {
                        rc = session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// Change the duplex flag for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="useIt">if set to <c>true</c> [use it].</param>
        /// <returns></returns>
        public static ReturnCode CapSetDuplex(this TwainSession session, bool useIt)
        {
            if (session.SourceId.ProtocolMajor >= 2)
            {
                // twain 2 likes to use enum :(

                TWEnumeration en = new TWEnumeration();
                en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                en.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, en))
                {
                    return session.DGControl.Capability.Set(dx);
                }
            }
            else
            {
                TWOneValue one = new TWOneValue();
                one.Item = (uint)(useIt ? 1 : 0);
                one.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, one))
                {
                    return session.DGControl.Capability.Set(dx);
                }
            }
        }

        /// <summary>
        /// Change the use feeder flag for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="useIt">if set to <c>true</c> [use it].</param>
        /// <returns></returns>
        public static ReturnCode CapSetFeeder(this TwainSession session, bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (session.SupportedCaps.Contains(CapabilityId.CapFeederEnabled))
            {
                if (session.SourceId.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (ushort)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;



                    // we will never set feeder off, only autofeed and autoscan
                    // but if it is to SET then enable feeder needs to be set first
                    if (useIt)
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapFeederEnabled, en))
                        {
                            rc = session.DGControl.Capability.Set(dx);
                        }
                    }

                    // to really use feeder we must also set autofeed or autoscan, but only
                    // for one of them since setting autoscan also sets autofeed
                    if (session.SupportedCaps.Contains(CapabilityId.CapAutoScan))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoScan, en))
                        {
                            rc = session.DGControl.Capability.Set(dx);
                        }
                    }
                    else if (session.SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoFeed, en))
                        {
                            rc = session.DGControl.Capability.Set(dx);
                        }
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    if (useIt)
                    {
                        using (TWCapability enabled = new TWCapability(CapabilityId.CapFeederEnabled, one))
                        {
                            rc = session.DGControl.Capability.Set(enabled);
                        }
                    }
                    // to really use feeder we must also set autofeed or autoscan, but only
                    // for one of them since setting autoscan also sets autofeed
                    if (session.SupportedCaps.Contains(CapabilityId.CapAutoScan))
                    {
                        using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoScan, one))
                        {
                            rc = session.DGControl.Capability.Set(autoScan);
                        }
                    }
                    else if (session.SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                    {
                        using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoFeed, one))
                        {
                            rc = session.DGControl.Capability.Set(autoScan);
                        }
                    }
                }
            }
            return rc;
        }

        #endregion

        #endregion
    }
}
