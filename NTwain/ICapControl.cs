using NTwain.Data;
using System;
using System.Collections.Generic;
namespace NTwain
{
    /// <summary>
    /// Interface for providing basic functions at controlling caps.
    /// </summary>
    public interface ICapControl : ITripletControl
    {

        /// <summary>
        /// Gets all the possible values for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        IList<object> CapGet(CapabilityId capabilityId);

        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        object CapGetCurrent(CapabilityId capabilityId);

        /// <summary>
        /// Gets the default value for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        object CapGetDefault(CapabilityId capabilityId);

        /// <summary>
        /// Gets the supported operations for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        QuerySupports? CapQuerySupport(CapabilityId capabilityId);

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        ReturnCode CapReset(CapabilityId capabilityId);

        /// <summary>
        /// Resets all values and constraints to power-on defaults.
        /// </summary>
        /// <returns></returns>
        ReturnCode CapResetAll();

    }
}
