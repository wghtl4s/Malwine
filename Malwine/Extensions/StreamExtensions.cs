using System.Collections.Generic;
using System.IO;

namespace Malwine.Extensions;

static class StreamExtensions
{
  public static IEnumerable<byte> AsEnumerable(this Stream @this)
  {
    for (var @byte = @this.ReadByte(); @byte != -1; @byte = @this.ReadByte())
    {
      yield return (byte)@byte;
    }
  }
}