using NTwain.Data;
using NTwain.Values;
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

        /// <summary>
        /// Verifies the session is within the specified state range (inclusive). Throws
        /// <see cref="TwainStateException" /> if violated.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="allowedMinimum">The allowed minimum.</param>
        /// <param name="allowedMaximum">The allowed maximum.</param>
        /// <param name="group">The triplet data group.</param>
        /// <param name="dataArgumentType">The triplet data argument type.</param>
        /// <param name="message">The triplet message.</param>
        /// <exception cref="TwainStateException"></exception>
        internal static void VerifyState(this ITwainStateInternal session, int allowedMinimum, int allowedMaximum, DataGroups group, DataArgumentType dataArgumentType, NTwain.Values.Message message)
        {
            if (session.EnforceState && (session.State < allowedMinimum || session.State > allowedMaximum))
            {
                throw new TwainStateException(session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message,
                    string.Format("TWAIN state {0} does not match required range {1}-{2} for operation {3}-{4}-{5}.",
                    session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message));
            }
        }


        #region common caps

        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        public static object GetCurrentCap(this TwainSession session, CapabilityId capId)
        {
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = session.DGControl.Capability.GetCurrent(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapReadOut.ReadValue(cap);

                    switch (read.ContainerType)
                    {
                        case ContainerType.Enum:
                            if (read.CollectionValues != null)
                            {
                                return read.CollectionValues[read.EnumCurrentIndex];
                            }
                            break;
                        case ContainerType.OneValue:
                            return read.OneValue;
                        case ContainerType.Range:
                            return read.RangeCurrentValue;
                        case ContainerType.Array:
                            // no source should ever return an array but anyway
                            if (read.CollectionValues != null)
                            {
                                return read.CollectionValues.FirstOrDefault();
                            }
                            break;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// A general method that returns the data in a <see cref="TWCapability" />.
        /// </summary>
        /// <param name="capability">The capability returned from the source.</param>
        /// <param name="toPopulate">The list to populate if necessary.</param>
        /// <returns></returns>
        public static IList<object> ReadMultiCapValues(this TWCapability capability, IList<object> toPopulate)
        {
            if (toPopulate == null) { toPopulate = new List<object>(); }

            var read = CapReadOut.ReadValue(capability);

            switch (read.ContainerType)
            {
                case ContainerType.OneValue:
                    if (read.OneValue != null)
                    {
                        toPopulate.Add(read.OneValue);
                    }
                    break;
                case ContainerType.Array:
                case ContainerType.Enum:
                    if (read.CollectionValues != null)
                    {
                        foreach (var o in read.CollectionValues)
                        {
                            toPopulate.Add(o);
                        }
                    }
                    break;
                case ContainerType.Range:
                    for (var i = read.RangeMinValue; i <= read.RangeMaxValue; i += read.RangeStepSize)
                    {
                        toPopulate.Add(i);
                    }
                    break;
            }
            return toPopulate;
        }

        /// <summary>
        /// A general method that tries to get capability values from current <see cref="TwainSession" />.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="capabilityId">The capability unique identifier.</param>
        /// <returns></returns>
        public static IList<object> GetCapabilityValues(this TwainSession session, CapabilityId capabilityId)
        {
            var list = new List<object>();
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = session.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    cap.ReadMultiCapValues(list);
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
            return session.GetCapabilityValues(CapabilityId.CapSupportedCaps).CastToEnum<CapabilityId>(false);
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
            return session.GetCapabilityValues(CapabilityId.ICapXferMech).CastToEnum<XferMech>(true);
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
            return session.GetCapabilityValues(CapabilityId.ICapCompression).CastToEnum<Compression>(true);
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
        /// Gets the supported <see cref="FileFormat"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<FileFormat> CapGetImageFileFormat(this TwainSession session)
        {
            return session.GetCapabilityValues(CapabilityId.ICapImageFileFormat).CastToEnum<FileFormat>(true);
        }

        /// <summary>
        /// Change the image format for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static ReturnCode CapSetImageFormat(this TwainSession session, FileFormat format)
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
            return session.GetCapabilityValues(CapabilityId.ICapPixelType).CastToEnum<PixelType>(true);
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

        #region xfer mech

        /// <summary>
        /// Gets the supported image <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<XferMech> CapGetImageXferMechs(this TwainSession session)
        {
            return session.GetCapabilityValues(CapabilityId.ICapXferMech).CastToEnum<XferMech>(true);
        }

        /// <summary>
        /// Gets the supported audio <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public static IList<XferMech> CapGetAudioXferMechs(this TwainSession session)
        {
            return session.GetCapabilityValues(CapabilityId.ACapXferMech).CastToEnum<XferMech>(true);
        }

        /// <summary>
        /// Change the image xfer type for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ReturnCode CapSetImageXferMech(this TwainSession session, XferMech type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ICapXferMech, one))
            {
                return session.DGControl.Capability.Set(dx);
            }
        }

        /// <summary>
        /// Change the audio xfer type for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ReturnCode CapSetAudioXferMech(this TwainSession session, XferMech type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ACapXferMech, one))
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
        public static IList<TWFix32> CapGetDPIs(this TwainSession session)
        {
            var list = session.GetCapabilityValues(CapabilityId.ICapXResolution);
            return list.Select(o => o.ConvertToFix32()).ToList();
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="dpi">The DPI.</param>
        /// <returns></returns>
        public static ReturnCode CapSetDPI(this TwainSession session, TWFix32 dpi)
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
        public static ReturnCode CapSetDPI(this TwainSession session, TWFix32 xDPI, TWFix32 yDPI)
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
            return session.GetCapabilityValues(CapabilityId.ICapSupportedSizes).CastToEnum<SupportedSize>(true);
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
