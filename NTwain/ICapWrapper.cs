using NTwain.Data;
using System;
namespace NTwain
{

    /// <summary>
    /// Interface for reading a TWAIN capability associated with a <see cref="DataSource"/>.
    /// </summary>
    /// <typeparam name="TValue">The TWAIN type of the value.</typeparam>    
    public interface IReadOnlyCapWrapper<TValue>
    {
        /// <summary>
        /// Gets a value indicating whether <see cref="Get"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get values; otherwise, <c>false</c>.
        /// </value>
        bool CanGet { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetDefault"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get default value; otherwise, <c>false</c>.
        /// </value>
        bool CanGetCurrent { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetCurrent"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get current value; otherwise, <c>false</c>.
        /// </value>
        bool CanGetDefault { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetHelp"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get help; otherwise, <c>false</c>.
        /// </value>
        bool CanGetHelp { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetLabel"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get label; otherwise, <c>false</c>.
        /// </value>
        bool CanGetLabel { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetLabelEnum"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get label enum; otherwise, <c>false</c>.
        /// </value>
        bool CanGetLabelEnum { get; }

        /// <summary>
        /// Gets the capability.
        /// </summary>
        /// <value>
        /// The capability.
        /// </value>
        NTwain.Data.CapabilityId Capability { get; }

        /// <summary>
        /// Gets all the possible values of this capability.
        /// </summary>
        /// <returns></returns>
        System.Collections.Generic.IList<TValue> Get();

        /// <summary>
        /// Gets the current value of this capability.
        /// </summary>
        /// <returns></returns>
        TValue GetCurrent();

        /// <summary>
        /// Gets the default value of this capability.
        /// </summary>
        /// <returns></returns>
        TValue GetDefault();

        /// <summary>
        /// [Experimental] Gets the help value of this capability.
        /// </summary>
        /// <returns></returns>
        string GetHelp();

        /// <summary>
        /// [Experimental] Gets the label value of this capability.
        /// </summary>
        /// <returns></returns>
        string GetLabel();

        /// <summary>
        /// [Experimental] Gets the display names for possible values of this capability.
        /// </summary>
        /// <returns></returns>
        System.Collections.Generic.IList<string> GetLabelEnum();

        /// <summary>
        /// Gets a value indicating whether this capability is supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this capability is supported; otherwise, <c>false</c>.
        /// </value>
        bool IsSupported { get; }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>
        /// The supported actions.
        /// </value>
        NTwain.Data.QuerySupports SupportedActions { get; }
    }

    /// <summary>
    /// Interface for reading/writing a TWAIN capability associated with a <see cref="DataSource"/>.
    /// </summary>
    /// <typeparam name="TValue">The TWAIN type of the value.</typeparam>
    public interface ICapWrapper<TValue> : IReadOnlyCapWrapper<TValue>
    {
        /// <summary>
        /// Gets a value indicating whether <see cref="Reset"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can reset; otherwise, <c>false</c>.
        /// </value>
        bool CanReset { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="Set"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can set; otherwise, <c>false</c>.
        /// </value>
        bool CanSet { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="SetConstraint"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can set constraint; otherwise, <c>false</c>.
        /// </value>
        bool CanSetConstraint { get; }

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <returns></returns>
        NTwain.Data.ReturnCode Reset();

        /// <summary>
        /// Simplified version that sets the current value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        NTwain.Data.ReturnCode Set(TValue value);

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ReturnCode SetConstraint(TWOneValue value);

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ReturnCode SetConstraint(TWEnumeration value);

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ReturnCode SetConstraint(TWRange value);
    }
}
