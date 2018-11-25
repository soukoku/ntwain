using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Data
{
    // use custom containers for twain container types to not have to worry about memory mgmt
    // after giving it to consumers

    ///// <summary>
    ///// Container for one value.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public struct OneValue<T>
    //{
    //    /// <summary>
    //    /// The type of the item.
    //    /// </summary>
    //    public ItemType Type;

    //    /// <summary>
    //    /// The value.
    //    /// </summary>
    //    public T Value;
    //}

    /// <summary>
    /// Stores a group of associated individual values for a capability.
    /// The values need have no relationship to one another aside from 
    /// being used to describe the same "value" of the capability
    /// </summary>
    public struct ArrayValue<T>
    {
        /// <summary>
        /// The type of items in the array.
        /// </summary>
        public ItemType Type;

        /// <summary>
        /// Array of values.
        /// </summary>
        public T[] ItemList;
    }

    /// <summary>
    /// An enumeration stores a list of individual values, with one of the items designated as the current
    /// value. There is no required order to the values in the list.
    /// </summary>
    public struct EnumValue<T>
    {
        /// <summary>
        /// The type of items in the enumerated list.
        /// </summary>
        public ItemType Type;

        /// <summary>
        /// The item number, or index (zero-based) into 
        /// <see cref="ItemList"/>, of the "current"
        /// value for the capability.
        /// </summary>
        public int CurrentIndex;

        /// <summary>
        /// The item number, or index (zero-based) into 
        /// <see cref="ItemList"/>, of the "power-on"
        /// value for the capability.
        /// </summary>
        public int DefaultIndex;

        /// <summary>
        /// The enumerated value list.
        /// </summary>
        public T[] ItemList;
    }
    
}
