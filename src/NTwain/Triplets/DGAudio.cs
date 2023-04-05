using NTwain.Data;
using NTwain.Triplets.AudioDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains triplet calls starting with <see cref="DG.AUDIO"/>.
  /// </summary>
  public static class DGAudio
  {
    public static readonly AudioFileXfer AudioFileXfer = new();

    public static readonly AudioInfo AudioInfo = new();

    public static readonly AudioNativeXfer AudioNativeXfer = new();

  }
}