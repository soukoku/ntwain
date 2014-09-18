using NTwain.Data;
using System;
using System.Collections.Generic;
namespace NTwain
{
    /// <summary>
    /// Interface for controlling caps.
    /// </summary>
    public interface ICapController
    {

        /// <summary>
        /// A general method that tries to get capability values from current <see cref="DataSource" />.
        /// </summary>
        /// <param name="capabilityId">The capability unique identifier.</param>
        /// <returns></returns>
        IList<object> CapGet(CapabilityId capabilityId);

        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        object CapGetCurrent(CapabilityId capId);

        /// <summary>
        /// Gets the default value for a capability.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        object CapGetDefault(CapabilityId capId);
        
        /// <summary>
        /// Gets the actual supported operations for a capability.
        /// </summary>
        /// <param name="capId">The cap identifier.</param>
        /// <returns></returns>
        QuerySupports CapQuerySupport(CapabilityId capId);

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        ReturnCode CapReset(CapabilityId capabilityId);

        /// <summary>
        /// Resets all values and constraint to power-on defaults.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        ReturnCode CapResetAll(CapabilityId capabilityId);


        //CapabilityControl<XferMech> CapAudioXferMech { get; }
        //CapabilityControl<BoolType> CapDuplexEnabled { get; }
        //CapabilityControl<BoolType> CapFeederEnabled { get; }
        //CapabilityControl<BoolType> CapImageAutoDeskew { get; }
        //CapabilityControl<BoolType> CapImageAutomaticBorderDetection { get; }
        //CapabilityControl<BoolType> CapImageAutoRotate { get; }
        //CapabilityControl<CompressionType> CapImageCompression { get; }
        //CapabilityControl<FileFormat> CapImageFileFormat { get; }
        //CapabilityControl<PixelType> CapImagePixelType { get; }
        //CapabilityControl<SupportedSize> CapImageSupportedSize { get; }
        //CapabilityControl<XferMech> CapImageXferMech { get; }
        //CapabilityControl<TWFix32> CapImageXResolution { get; }
        //CapabilityControl<TWFix32> CapImageYResolution { get; }
        //CapabilityControl<int> CapXferCount { get; }
    }
}
