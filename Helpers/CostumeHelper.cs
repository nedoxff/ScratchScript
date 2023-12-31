using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace ScratchScript.Helpers;

public class CostumeHelper
{
    public static byte[] GetEmptyImage()
    {
        var image = new Image<Rgba32>(1, 1);
        var stream = new MemoryStream();
        image.Save(stream, new PngEncoder());
        return stream.ToArray();
    }

    public static Costume GetEmptyCostume()
    {
        var emptyAsset = GetEmptyImage();
        var checksum = emptyAsset.Md5Checksum();
        return new Costume
        {
            Name = "empty",
            DataFormat = "png",
            AssetId = checksum,
            Md5Extension = checksum + ".png",
            Data = emptyAsset
        };   
    }
}