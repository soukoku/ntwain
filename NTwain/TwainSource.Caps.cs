using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    partial class TwainSource
    {
        /// <summary>
        /// Gets the actual supported operations for a capability.
        /// </summary>
        /// <param name="capId">The cap identifier.</param>
        /// <returns></returns>
        public QuerySupport CapQuerySupport(CapabilityId capId)
        {
            QuerySupport retVal = QuerySupport.None;
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = Session.DGControl.Capability.QuerySupport(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapabilityReader.ReadValue(cap);

                    if (read.ContainerType == ContainerType.OneValue)
                    {
                        retVal = read.OneValue.ConvertToEnum<QuerySupport>();
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        public object CapGetCurrent(CapabilityId capId)
        {
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = Session.DGControl.Capability.GetCurrent(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapabilityReader.ReadValue(cap);

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
        /// A general method that tries to get capability values from current <see cref="TwainSource" />.
        /// </summary>
        /// <param name="capabilityId">The capability unique identifier.</param>
        /// <returns></returns>
        public IList<object> CapGetValues(CapabilityId capabilityId)
        {
            var list = new List<object>();
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = Session.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    cap.ReadMultiCapValues(list);
                }
            }
            return list;
        }

        #region xfer mech

        /// <summary>
        /// Gets the supported image <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<XferMech> CapGetImageXferMech()
        {
            return CapGetValues(CapabilityId.ICapXferMech).CastToEnum<XferMech>(true);
        }

        #endregion

        #region compression

        /// <summary>
        /// Gets the supported <see cref="CompressionType"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<CompressionType> CapGetCompression()
        {
            return CapGetValues(CapabilityId.ICapCompression).CastToEnum<CompressionType>(true);
        }

        /// <summary>
        /// Change the image compression for the current source.
        /// </summary>
        /// <param name="compression">The compression.</param>
        /// <returns></returns>
        public ReturnCode CapSetImageCompression(CompressionType compression)
        {
            using (TWCapability compressCap = new TWCapability(CapabilityId.ICapCompression, new TWOneValue { Item = (uint)compression, ItemType = ItemType.UInt16 }))
            {
                return Session.DGControl.Capability.Set(compressCap);
            }
        }

        #endregion

        #region image format

        /// <summary>
        /// Gets the supported <see cref="FileFormat"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<FileFormat> CapGetImageFileFormat()
        {
            return CapGetValues(CapabilityId.ICapImageFileFormat).CastToEnum<FileFormat>(true);
        }

        /// <summary>
        /// Change the image format for the current source.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public ReturnCode CapSetImageFormat(FileFormat format)
        {
            using (TWCapability formatCap = new TWCapability(CapabilityId.ICapImageFileFormat, new TWOneValue { Item = (uint)format, ItemType = ItemType.UInt16 }))
            {
                return Session.DGControl.Capability.Set(formatCap);
            }
        }

        #endregion

        #region pixel type

        /// <summary>
        /// Gets the supported <see cref="PixelType"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<PixelType> CapGetPixelTypes()
        {
            return CapGetValues(CapabilityId.ICapPixelType).CastToEnum<PixelType>(true);
        }

        /// <summary>
        /// Change the pixel type for the current source.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ReturnCode CapSetPixelType(PixelType type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ICapPixelType, one))
            {
                return Session.DGControl.Capability.Set(dx);
            }
        }

        #endregion

        #region xfer mech

        /// <summary>
        /// Gets the supported image <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<XferMech> CapGetImageXferMechs()
        {
            return CapGetValues(CapabilityId.ICapXferMech).CastToEnum<XferMech>(true);
        }

        /// <summary>
        /// Gets the supported audio <see cref="XferMech"/> for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<XferMech> CapGetAudioXferMechs()
        {
            return CapGetValues(CapabilityId.ACapXferMech).CastToEnum<XferMech>(true);
        }

        /// <summary>
        /// Change the image xfer type for the current source.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ReturnCode CapSetImageXferMech(XferMech type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ICapXferMech, one))
            {
                return Session.DGControl.Capability.Set(dx);
            }
        }

        /// <summary>
        /// Change the audio xfer type for the current source.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ReturnCode CapSetAudioXferMech(XferMech type)
        {
            var one = new TWOneValue();
            one.Item = (uint)type;
            one.ItemType = ItemType.UInt16;
            using (TWCapability dx = new TWCapability(CapabilityId.ACapXferMech, one))
            {
                return Session.DGControl.Capability.Set(dx);
            }
        }

        #endregion

        #region dpi

        /// <summary>
        /// Gets the supported DPI values for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<TWFix32> CapGetDPIs()
        {
            var list = CapGetValues(CapabilityId.ICapXResolution);
            return list.Select(o => o.ConvertToFix32()).ToList();
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="dpi">The DPI.</param>
        /// <returns></returns>
        public ReturnCode CapSetDPI(TWFix32 dpi)
        {
            return CapSetDPI(dpi, dpi);
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="xDPI">The x DPI.</param>
        /// <param name="yDPI">The y DPI.</param>
        /// <returns></returns>
        public ReturnCode CapSetDPI(TWFix32 xDPI, TWFix32 yDPI)
        {
            TWOneValue one = new TWOneValue();
            one.Item = (uint)xDPI;// ((uint)dpi) << 16;
            one.ItemType = ItemType.Fix32;

            using (TWCapability xres = new TWCapability(CapabilityId.ICapXResolution, one))
            {
                var rc = Session.DGControl.Capability.Set(xres);
                if (rc == ReturnCode.Success)
                {
                    one.Item = (uint)yDPI;
                    using (TWCapability yres = new TWCapability(CapabilityId.ICapYResolution, one))
                    {
                        rc = Session.DGControl.Capability.Set(yres);
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
        /// <returns></returns>
        public IList<SupportedSize> CapGetSupportedSizes()
        {
            return CapGetValues(CapabilityId.ICapSupportedSizes).CastToEnum<SupportedSize>(true);
        }

        /// <summary>
        /// Change the supported paper size for the current source.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public ReturnCode CapSetSupportedSize(SupportedSize size)
        {
            var one = new TWOneValue();
            one.Item = (uint)size;
            one.ItemType = ItemType.UInt16;

            using (TWCapability xres = new TWCapability(CapabilityId.ICapSupportedSizes, one))
            {
                var rc = Session.DGControl.Capability.Set(xres);
                return rc;
            }
        }

        #endregion

        #region onesie flags

        /// <summary>
        /// Change the auto deskew flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetAutoDeskew(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticDeskew))
            {

                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticDeskew, en))
                    {
                        rc = Session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticDeskew, one))
                    {
                        rc = Session.DGControl.Capability.Set(capValue);
                    }
                }
            }

            return rc;
        }

        /// <summary>
        /// Change the auto rotate flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetAutoRotate(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticRotate))
            {
                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticRotate, en))
                    {
                        rc = Session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticRotate, one))
                    {
                        rc = Session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// Change the auto border detection flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetBorderDetection(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticBorderDetection))
            {
                // this goes along with undefinedimagesize so that also
                // needs to be set
                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapUndefinedImageSize, en))
                    {
                        rc = Session.DGControl.Capability.Set(dx);
                    }
                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, en))
                    {
                        rc = Session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapUndefinedImageSize, one))
                    {
                        rc = Session.DGControl.Capability.Set(capValue);
                    }
                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                    {
                        rc = Session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// Change the duplex flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> to use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetDuplex(bool useIt)
        {
            if (Identity.ProtocolMajor >= 2)
            {
                // twain 2 likes to use enum :(

                TWEnumeration en = new TWEnumeration();
                en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                en.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, en))
                {
                    return Session.DGControl.Capability.Set(dx);
                }
            }
            else
            {
                TWOneValue one = new TWOneValue();
                one.Item = (uint)(useIt ? 1 : 0);
                one.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, one))
                {
                    return Session.DGControl.Capability.Set(dx);
                }
            }
        }

        /// <summary>
        /// Change the use feeder flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetFeeder(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.CapFeederEnabled))
            {
                if (Identity.ProtocolMajor >= 2)
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
                            rc = Session.DGControl.Capability.Set(dx);
                        }
                    }

                    // to really use feeder we must also set autofeed or autoscan, but only
                    // for one of them since setting autoscan also sets autofeed
                    if (SupportedCaps.Contains(CapabilityId.CapAutoScan))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoScan, en))
                        {
                            rc = Session.DGControl.Capability.Set(dx);
                        }
                    }
                    else if (SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoFeed, en))
                        {
                            rc = Session.DGControl.Capability.Set(dx);
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
                            rc = Session.DGControl.Capability.Set(enabled);
                        }
                    }
                    // to really use feeder we must also set autofeed or autoscan, but only
                    // for one of them since setting autoscan also sets autofeed
                    if (SupportedCaps.Contains(CapabilityId.CapAutoScan))
                    {
                        using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoScan, one))
                        {
                            rc = Session.DGControl.Capability.Set(autoScan);
                        }
                    }
                    else if (SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                    {
                        using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoFeed, one))
                        {
                            rc = Session.DGControl.Capability.Set(autoScan);
                        }
                    }
                }
            }
            return rc;
        }

        #endregion
    }
}
