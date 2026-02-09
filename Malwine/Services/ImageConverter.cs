using System.IO;
using System.Threading.Tasks;

using SixLabors.ImageSharp;

namespace Malwine.Services;

public class ImageConverter
{
  public async Task<byte[]> ConvertAsync(Stream image)
  {
    using var avatarImage = await Image.LoadAsync(image);
    using var avatarStream = new MemoryStream();
    await avatarImage.SaveAsWebpAsync(avatarStream);

    return avatarStream.ToArray();
  }
}