using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Data
{
  /// <summary>
  /// Contains methods for writing vairous things to pointers.
  /// </summary>
  static class ValueWriter
  {
    /// <summary>
    /// Allocates and copies the string value into a pointer in UTF8 that's null-terminated.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="memMgr"></param>
    /// <param name="finalLength">Final length to use with the pointer (includes the null).</param>
    /// <returns></returns>
    public static unsafe IntPtr StringToPtrUTF8(this string? value, IMemoryManager memMgr, out uint finalLength)
    {
      finalLength = 0;
      if (value == null) return IntPtr.Zero;

      fixed (char* pInput = value)
      {
        var len = Encoding.UTF8.GetByteCount(pInput, value.Length);
        finalLength = (uint)len + 1;
        var pResult = (byte*)memMgr.Alloc(finalLength);
        var bytesWritten = Encoding.UTF8.GetBytes(pInput, value.Length, pResult, len);
        Trace.Assert(len == bytesWritten);
        pResult[len] = 0;
        return (IntPtr)pResult;
      }
    }



    // most of these are modified from the original TWAIN.CsvToCapability()

    //public static void WriteOneValueContainer<TValue>(IMemoryManager memMgr, ref TW_CAPABILITY twCap, TValue value) where TValue : struct
    //{
    //    IntPtr lockedPtr = IntPtr.Zero;
    //    try
    //    {
    //        if (twCap.hContainer != IntPtr.Zero) memMgr.Free(ref twCap.hContainer);

    //        TWTY itemType = GetItemType<TValue>();

    //        // Allocate the container (go for worst case, which is TW_STR255)...
    //        if (TwainPlatform.IsMacOSX)
    //        {
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE_MACOSX)) + Marshal.SizeOf(default(TW_STR255))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            TW_ONEVALUE_MACOSX twonevaluemacosx = default;
    //            twonevaluemacosx.ItemType = (uint)itemType;
    //            Marshal.StructureToPtr(twonevaluemacosx, lockedPtr, false);

    //            lockedPtr += Marshal.SizeOf(twonevaluemacosx);
    //        }
    //        else
    //        {
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE)) + Marshal.SizeOf(default(TW_STR255))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            TW_ONEVALUE twonevalue = default;
    //            twonevalue.ItemType = itemType;
    //            Marshal.StructureToPtr(twonevalue, lockedPtr, false);

    //            lockedPtr += Marshal.SizeOf(twonevalue);
    //        }

    //        WriteContainerData(lockedPtr, itemType, value, 0);
    //    }
    //    finally
    //    {
    //        if (lockedPtr != IntPtr.Zero) memMgr.Unlock(twCap.hContainer);
    //    }
    //}

    //public static void WriteArrayContainer<TValue>(IMemoryManager memMgr, ref TW_CAPABILITY twCap, TValue[] values) where TValue : struct
    //{
    //    IntPtr lockedPtr = IntPtr.Zero;
    //    try
    //    {
    //        if (twCap.hContainer != IntPtr.Zero) memMgr.Free(ref twCap.hContainer);

    //        TWTY itemType = GetItemType<TValue>();

    //        // Allocate the container (go for worst case, which is TW_STR255)...
    //        if (TwainPlatform.IsMacOSX)
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ARRAY_MACOSX)) + ((values.Length + 1) * Marshal.SizeOf(default(TW_STR255)))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            // Set the meta data...
    //            TW_ARRAY_MACOSX twarraymacosx = default;
    //            twarraymacosx.ItemType = (uint)itemType;
    //            twarraymacosx.NumItems = (uint)values.Length;
    //            Marshal.StructureToPtr(twarraymacosx, lockedPtr, false);

    //            // Get the pointer to the ItemList...
    //            lockedPtr += Marshal.SizeOf(twarraymacosx);
    //        }
    //        else
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ARRAY)) + ((values.Length + 1) * Marshal.SizeOf(default(TW_STR255)))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            // Set the meta data...
    //            TW_ARRAY twarray = default;
    //            twarray.ItemType = itemType;
    //            twarray.NumItems = (uint)values.Length;
    //            Marshal.StructureToPtr(twarray, lockedPtr, false);

    //            // Get the pointer to the ItemList...
    //            lockedPtr += Marshal.SizeOf(twarray);
    //        }

    //        // Set the ItemList...
    //        for (var i = 0; i < values.Length; i++)
    //        {
    //            WriteContainerData(lockedPtr, itemType, values[i], i);
    //        }
    //    }
    //    finally
    //    {
    //        if (lockedPtr != IntPtr.Zero) memMgr.Unlock(twCap.hContainer);
    //    }
    //}

    //public static void WriteEnumContainer<TValue>(IMemoryManager memMgr, ref TW_CAPABILITY twCap, Enumeration<TValue> value) where TValue : struct
    //{
    //    IntPtr lockedPtr = IntPtr.Zero;
    //    try
    //    {
    //        if (twCap.hContainer != IntPtr.Zero) memMgr.Free(ref twCap.hContainer);

    //        TWTY itemType = GetItemType<TValue>();

    //        // Allocate the container (go for worst case, which is TW_STR255)...
    //        if (TwainPlatform.IsMacOSX)
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_MACOSX)) + ((value.Items.Length + 1) * Marshal.SizeOf(default(TW_STR255)))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            // Set the meta data...
    //            TW_ENUMERATION_MACOSX twenumerationmacosx = default;
    //            twenumerationmacosx.ItemType = (uint)itemType;
    //            twenumerationmacosx.NumItems = (uint)value.Items.Length;
    //            twenumerationmacosx.CurrentIndex = (uint)value.CurrentIndex;
    //            twenumerationmacosx.DefaultIndex = (uint)value.DefaultIndex;
    //            Marshal.StructureToPtr(twenumerationmacosx, lockedPtr, false);

    //            // Get the pointer to the ItemList...
    //            lockedPtr += Marshal.SizeOf(twenumerationmacosx);
    //        }
    //        // Windows or the 2.4+ Linux DSM...
    //        else if (TWAIN.GetPlatform() == Platform.WINDOWS ||
    //            (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm) ||
    //            ((twain.m_blFoundLatestDsm || twain.m_blFoundLatestDsm64) && (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm)))
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION)) + ((value.Items.Length + 1) * Marshal.SizeOf(default(TW_STR255)))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            // Set the meta data...
    //            TW_ENUMERATION twenumeration = default;
    //            twenumeration.ItemType = itemType;
    //            twenumeration.NumItems = (uint)value.Items.Length;
    //            twenumeration.CurrentIndex = (uint)value.CurrentIndex;
    //            twenumeration.DefaultIndex = (uint)value.CurrentIndex;
    //            Marshal.StructureToPtr(twenumeration, lockedPtr, false);

    //            // Get the pointer to the ItemList...
    //            lockedPtr += Marshal.SizeOf(twenumeration);
    //        }
    //        // The -2.3 Linux DSM...
    //        else
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_LINUX64)) + ((value.Items.Length + 1) * Marshal.SizeOf(default(TW_STR255)))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);

    //            // Set the meta data...
    //            TW_ENUMERATION_LINUX64 twenumerationlinux64 = default;
    //            twenumerationlinux64.ItemType = itemType;
    //            twenumerationlinux64.NumItems = (ulong)value.Items.Length;
    //            twenumerationlinux64.CurrentIndex = (ulong)value.CurrentIndex;
    //            twenumerationlinux64.DefaultIndex = (ulong)value.CurrentIndex;
    //            Marshal.StructureToPtr(twenumerationlinux64, lockedPtr, false);

    //            // Get the pointer to the ItemList...
    //            lockedPtr += Marshal.SizeOf(twenumerationlinux64);
    //        }

    //        // Set the ItemList...
    //        for (var i = 0; i < value.Items.Length; i++)
    //        {
    //            WriteContainerData(lockedPtr, itemType, value.Items[i], i);
    //        }
    //    }
    //    finally
    //    {
    //        if (lockedPtr != IntPtr.Zero) memMgr.Unlock(twCap.hContainer);
    //    }
    //}

    //public static void WriteRangeContainer<TValue>(IMemoryManager memMgr, ref TW_CAPABILITY twCap, Range<TValue> value) where TValue : struct
    //{
    //    IntPtr lockedPtr = IntPtr.Zero;
    //    try
    //    {
    //        if (twCap.hContainer != IntPtr.Zero) memMgr.Free(ref twCap.hContainer);

    //        TWTY itemType = GetItemType<TValue>();

    //        // Allocate the container (go for worst case, which is TW_STR255)...
    //        if (TwainPlatform.IsMacOSX)
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_RANGE_MACOSX))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);
    //        }
    //        // Windows or the 2.4+ Linux DSM...
    //        else if (TWAIN.GetPlatform() == Platform.WINDOWS ||
    //            (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm) ||
    //            ((twain.m_blFoundLatestDsm || twain.m_blFoundLatestDsm64) && (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm)))
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_RANGE))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);
    //        }
    //        // The -2.3 Linux DSM...
    //        else
    //        {
    //            // Allocate...
    //            twCap.hContainer = memMgr.Alloc((uint)(Marshal.SizeOf(default(TW_RANGE_LINUX64))));
    //            lockedPtr = memMgr.Lock(twCap.hContainer);
    //        }

    //        // Set the Item...
    //        WriteRangeValues(twain, lockedPtr, itemType, value);
    //    }
    //    finally
    //    {
    //        if (lockedPtr != IntPtr.Zero) memMgr.Unlock(twCap.hContainer);
    //    }
    //}

    //static void WriteRangeValues<TValue>(IMemoryManager memMgr, IntPtr lockedPtr, TWTY itemType, Range<TValue> value) where TValue : struct
    //{
    //    // TODO: reduce this later

    //    TW_RANGE twrange = default;
    //    TW_RANGE_MACOSX twrangemacosx = default;
    //    TW_RANGE_LINUX64 twrangelinux64 = default;

    //    switch (itemType)
    //    {
    //        default:
    //            throw new NotSupportedException($"{itemType} is not supported for range.");
    //        case TWTY.INT8:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = (uint)Convert.ToSByte(value.MinValue);
    //                twrangemacosx.MaxValue = (uint)Convert.ToSByte(value.MaxValue);
    //                twrangemacosx.StepSize = (uint)Convert.ToSByte(value.StepSize);
    //                twrangemacosx.DefaultValue = (uint)Convert.ToSByte(value.DefaultValue);
    //                twrangemacosx.CurrentValue = (uint)Convert.ToSByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = (uint)Convert.ToSByte(value.MinValue);
    //                twrange.MaxValue = (uint)Convert.ToSByte(value.MaxValue);
    //                twrange.StepSize = (uint)Convert.ToSByte(value.StepSize);
    //                twrange.DefaultValue = (uint)Convert.ToSByte(value.DefaultValue);
    //                twrange.CurrentValue = (uint)Convert.ToSByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = (uint)Convert.ToSByte(value.MinValue);
    //                twrangelinux64.MaxValue = (uint)Convert.ToSByte(value.MaxValue);
    //                twrangelinux64.StepSize = (uint)Convert.ToSByte(value.StepSize);
    //                twrangelinux64.DefaultValue = (uint)Convert.ToSByte(value.DefaultValue);
    //                twrangelinux64.CurrentValue = (uint)Convert.ToSByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.UINT8:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = Convert.ToByte(value.MinValue);
    //                twrangemacosx.MaxValue = Convert.ToByte(value.MaxValue);
    //                twrangemacosx.StepSize = Convert.ToByte(value.StepSize);
    //                twrangemacosx.DefaultValue = Convert.ToByte(value.DefaultValue);
    //                twrangemacosx.CurrentValue = Convert.ToByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = Convert.ToByte(value.MinValue);
    //                twrange.MaxValue = Convert.ToByte(value.MaxValue);
    //                twrange.StepSize = Convert.ToByte(value.StepSize);
    //                twrange.DefaultValue = Convert.ToByte(value.DefaultValue);
    //                twrange.CurrentValue = Convert.ToByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = Convert.ToByte(value.MinValue);
    //                twrangelinux64.MaxValue = Convert.ToByte(value.MaxValue);
    //                twrangelinux64.StepSize = Convert.ToByte(value.StepSize);
    //                twrangelinux64.DefaultValue = Convert.ToByte(value.DefaultValue);
    //                twrangelinux64.CurrentValue = Convert.ToByte(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.INT16:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = (uint)Convert.ToInt16(value.MinValue);
    //                twrangemacosx.MaxValue = (uint)Convert.ToInt16(value.MaxValue);
    //                twrangemacosx.StepSize = (uint)Convert.ToInt16(value.StepSize);
    //                twrangemacosx.DefaultValue = (uint)Convert.ToInt16(value.DefaultValue);
    //                twrangemacosx.CurrentValue = (uint)Convert.ToInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = (uint)Convert.ToInt16(value.MinValue);
    //                twrange.MaxValue = (uint)Convert.ToInt16(value.MaxValue);
    //                twrange.StepSize = (uint)Convert.ToInt16(value.StepSize);
    //                twrange.DefaultValue = (uint)Convert.ToInt16(value.DefaultValue);
    //                twrange.CurrentValue = (uint)Convert.ToInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = (uint)Convert.ToInt16(value.MinValue);
    //                twrangelinux64.MaxValue = (uint)Convert.ToInt16(value.MaxValue);
    //                twrangelinux64.StepSize = (uint)Convert.ToInt16(value.StepSize);
    //                twrangelinux64.DefaultValue = (uint)Convert.ToInt16(value.DefaultValue);
    //                twrangelinux64.CurrentValue = (uint)Convert.ToInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.BOOL:
    //        case TWTY.UINT16:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = Convert.ToUInt16(value.MinValue);
    //                twrangemacosx.MaxValue = Convert.ToUInt16(value.MaxValue);
    //                twrangemacosx.StepSize = Convert.ToUInt16(value.StepSize);
    //                twrangemacosx.DefaultValue = Convert.ToUInt16(value.DefaultValue);
    //                twrangemacosx.CurrentValue = Convert.ToUInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = Convert.ToUInt16(value.MinValue);
    //                twrange.MaxValue = Convert.ToUInt16(value.MaxValue);
    //                twrange.StepSize = Convert.ToUInt16(value.StepSize);
    //                twrange.DefaultValue = Convert.ToUInt16(value.DefaultValue);
    //                twrange.CurrentValue = Convert.ToUInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = Convert.ToUInt16(value.MinValue);
    //                twrangelinux64.MaxValue = Convert.ToUInt16(value.MaxValue);
    //                twrangelinux64.StepSize = Convert.ToUInt16(value.StepSize);
    //                twrangelinux64.DefaultValue = Convert.ToUInt16(value.DefaultValue);
    //                twrangelinux64.CurrentValue = Convert.ToUInt16(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.INT32:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = (uint)Convert.ToInt32(value.MinValue);
    //                twrangemacosx.MaxValue = (uint)Convert.ToInt32(value.MaxValue);
    //                twrangemacosx.StepSize = (uint)Convert.ToInt32(value.StepSize);
    //                twrangemacosx.DefaultValue = (uint)Convert.ToInt32(value.DefaultValue);
    //                twrangemacosx.CurrentValue = (uint)Convert.ToInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = (uint)Convert.ToInt32(value.MinValue);
    //                twrange.MaxValue = (uint)Convert.ToInt32(value.MaxValue);
    //                twrange.StepSize = (uint)Convert.ToInt32(value.StepSize);
    //                twrange.DefaultValue = (uint)Convert.ToInt32(value.DefaultValue);
    //                twrange.CurrentValue = (uint)Convert.ToInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = (uint)Convert.ToInt32(value.MinValue);
    //                twrangelinux64.MaxValue = (uint)Convert.ToInt32(value.MaxValue);
    //                twrangelinux64.StepSize = (uint)Convert.ToInt32(value.StepSize);
    //                twrangelinux64.DefaultValue = (uint)Convert.ToInt32(value.DefaultValue);
    //                twrangelinux64.CurrentValue = (uint)Convert.ToInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.UINT32:
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                twrangemacosx.ItemType = (uint)itemType;
    //                twrangemacosx.MinValue = Convert.ToUInt32(value.MinValue);
    //                twrangemacosx.MaxValue = Convert.ToUInt32(value.MaxValue);
    //                twrangemacosx.StepSize = Convert.ToUInt32(value.StepSize);
    //                twrangemacosx.DefaultValue = Convert.ToUInt32(value.DefaultValue);
    //                twrangemacosx.CurrentValue = Convert.ToUInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangemacosx, lockedPtr, false);
    //            }
    //            else if ((twain.m_linuxdsm == TWAIN.LinuxDsm.Unknown) || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm))
    //            {
    //                twrange.ItemType = itemType;
    //                twrange.MinValue = Convert.ToUInt32(value.MinValue);
    //                twrange.MaxValue = Convert.ToUInt32(value.MaxValue);
    //                twrange.StepSize = Convert.ToUInt32(value.StepSize);
    //                twrange.DefaultValue = Convert.ToUInt32(value.DefaultValue);
    //                twrange.CurrentValue = Convert.ToUInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrange, lockedPtr, false);
    //            }
    //            else
    //            {
    //                twrangelinux64.ItemType = itemType;
    //                twrangelinux64.MinValue = Convert.ToUInt32(value.MinValue);
    //                twrangelinux64.MaxValue = Convert.ToUInt32(value.MaxValue);
    //                twrangelinux64.StepSize = Convert.ToUInt32(value.StepSize);
    //                twrangelinux64.DefaultValue = Convert.ToUInt32(value.DefaultValue);
    //                twrangelinux64.CurrentValue = Convert.ToUInt32(value.CurrentValue);
    //                Marshal.StructureToPtr(twrangelinux64, lockedPtr, false);
    //            }
    //            break;
    //        case TWTY.FIX32:
    //            double min = Convert.ToDouble(value.MinValue);
    //            double max = Convert.ToDouble(value.MaxValue);
    //            double step = Convert.ToDouble(value.StepSize);
    //            double def = Convert.ToDouble(value.DefaultValue);
    //            double current = Convert.ToDouble(value.CurrentValue);
    //            if (TwainPlatform.IsMacOSX)
    //            {
    //                TW_RANGE_FIX32_MACOSX twrangefix32macosx = default;
    //                twrangefix32macosx.ItemType = (uint)itemType;
    //                twrangefix32macosx.MinValue = new TW_FIX32(min);
    //                twrangefix32macosx.MaxValue = new TW_FIX32(max);
    //                twrangefix32macosx.StepSize = new TW_FIX32(step);
    //                twrangefix32macosx.DefaultValue = new TW_FIX32(def);
    //                twrangefix32macosx.CurrentValue = new TW_FIX32(current);
    //                Marshal.StructureToPtr(twrangefix32macosx, lockedPtr, false);
    //            }
    //            else
    //            {
    //                TW_RANGE_FIX32 twrangefix32 = default;
    //                twrangefix32.ItemType = itemType;
    //                twrangefix32.MinValue = new TW_FIX32(min);
    //                twrangefix32.MaxValue = new TW_FIX32(max);
    //                twrangefix32.StepSize = new TW_FIX32(step);
    //                twrangefix32.DefaultValue = new TW_FIX32(def);
    //                twrangefix32.CurrentValue = new TW_FIX32(current);
    //                Marshal.StructureToPtr(twrangefix32, lockedPtr, false);
    //            }
    //            break;
    //    }
    //}

    //static TWTY GetItemType<TValue>() where TValue : struct
    //{
    //    var type = typeof(TValue);
    //    if (type == typeof(BoolType)) return TWTY.BOOL;
    //    if (type == typeof(TW_FIX32)) return TWTY.FIX32;
    //    if (type == typeof(TW_STR32)) return TWTY.STR32;
    //    if (type == typeof(TW_STR64)) return TWTY.STR64;
    //    if (type == typeof(TW_STR128)) return TWTY.STR128;
    //    if (type == typeof(TW_STR255)) return TWTY.STR255;
    //    if (type == typeof(TW_FRAME)) return TWTY.FRAME;

    //    if (type.IsEnum)
    //    {
    //        type = type.GetEnumUnderlyingType();
    //    }

    //    if (type == typeof(ushort)) return TWTY.UINT16;
    //    if (type == typeof(short)) return TWTY.INT16;
    //    if (type == typeof(uint)) return TWTY.UINT32;
    //    if (type == typeof(int)) return TWTY.INT32;
    //    if (type == typeof(byte)) return TWTY.UINT8;
    //    if (type == typeof(sbyte)) return TWTY.INT8;

    //    throw new NotSupportedException($"{type.Name} is not supported for writing.");
    //}

    ///// <summary>
    ///// Writes single piece of value to the container pointer.
    ///// </summary>
    ///// <typeparam name="TValue"></typeparam>
    ///// <param name="intptr">A locked pointer to the container's data pointer. If data is array this is the 0th item.</param>
    ///// <param name="type">The twain type.</param>
    ///// <param name="value"></param>
    ///// <param name="itemIndex">Index of the item if pointer is array.</param>
    //static void WriteContainerData<TValue>(IntPtr intptr, TWTY type, TValue value, int itemIndex) where TValue : struct
    //{
    //    switch (type)
    //    {
    //        default:
    //            throw new NotSupportedException($"Unsupported item type {type} for writing.");
    //        // TODO: for small types needs to fill whole int32 before writing?
    //        case TWTY.INT8:
    //            intptr += 1 * itemIndex;
    //            //int intval = Convert.ToSByte(value);
    //            //Marshal.StructureToPtr(intval, intptr, false);
    //            Marshal.StructureToPtr(Convert.ToSByte(value), intptr, false);
    //            break;
    //        case TWTY.UINT8:
    //            intptr += 1 * itemIndex;
    //            //uint uintval = Convert.ToByte(value);
    //            //Marshal.StructureToPtr(uintval, intptr, false);
    //            Marshal.StructureToPtr(Convert.ToByte(value), intptr, false);
    //            break;
    //        case TWTY.INT16:
    //            intptr += 2 * itemIndex;
    //            //intval = Convert.ToInt16(value);
    //            //Marshal.StructureToPtr(intval, intptr, false);
    //            Marshal.StructureToPtr(Convert.ToInt16(value), intptr, false);
    //            break;
    //        case TWTY.BOOL:
    //        case TWTY.UINT16:
    //            intptr += 2 * itemIndex;
    //            //uintval = Convert.ToUInt16(value);
    //            //Marshal.StructureToPtr(uintval, intptr, false);
    //            Marshal.StructureToPtr(Convert.ToUInt16(value), intptr, false);
    //            break;
    //        case TWTY.INT32:
    //            intptr += 4 * itemIndex;
    //            Marshal.StructureToPtr(Convert.ToInt32(value), intptr, false);
    //            break;
    //        case TWTY.UINT32:
    //            intptr += 4 * itemIndex;
    //            Marshal.StructureToPtr(Convert.ToUInt32(value), intptr, false);
    //            break;
    //        case TWTY.FIX32:
    //            intptr += 4 * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //        case TWTY.FRAME:
    //            intptr += 16 * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //        case TWTY.STR32:
    //            intptr += TW_STR32.Size * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //        case TWTY.STR64:
    //            intptr += TW_STR64.Size * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //        case TWTY.STR128:
    //            intptr += TW_STR128.Size * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //        case TWTY.STR255:
    //            intptr += TW_STR255.Size * itemIndex;
    //            Marshal.StructureToPtr(value, intptr, false);
    //            break;
    //    }
    //}
  }
}
