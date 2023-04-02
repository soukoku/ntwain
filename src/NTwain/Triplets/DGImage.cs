using NTwain.Data;
using NTwain.Triplets.ImageDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains triplet calls starting with <see cref="DG.IMAGE"/>.
  /// </summary>
  public static class DGImage
  {
    public static readonly CieColor CieColor = new();

    public static readonly ExtImageInfo ExtImageInfo = new();

    public static readonly Filter Filter = new();

    public static readonly GrayResponse GrayResponse = new();

    public static readonly IccProfile IccProfile = new();

    public static readonly ImageFileXfer ImageFileXfer = new();

    public static readonly ImageInfo ImageInfo = new();

    public static readonly ImageLayout ImageLayout = new();

    public static readonly ImageMemFileXfer ImageMemFileXfer = new();

    public static readonly ImageMemXfer ImageMemXfer = new();

    public static readonly ImageNativeXfer ImageNativeXfer = new();

    public static readonly JpegCompression JpegCompression = new();

    public static readonly Palette8 Palette8 = new();

    public static readonly RgbResponse RgbResponse = new();
  }
}