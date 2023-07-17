using System.Security.Cryptography;

namespace ScratchScript.Extensions;

public static class ArrayExtensions
{
    public static string Md5Checksum(this byte[] s)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(s);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}