using System.Reflection;

namespace Drammer.Common.Images.Tests;

internal static class TestHelpers
{
    public const string FileName1 = "DALL·E 2025-03-10 16.25.30.webp";
    public const string ResourceName1 = $"Drammer.Common.Images.Tests.Resources.{FileName1}";
    public const int WidthFile1 = 1024;
    public const int HeightFile1 = 1024;

    public const string FileName2 = "DALL·E 2025-03-11 08.49.42.webp";
    public const string ResourceName2 = $"Drammer.Common.Images.Tests.Resources.{FileName2}";
    public const int WidthFile2 = 1792;
    public const int HeightFile2 = 1024;

    public static byte[]? ReadResource(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var resFilestream = assembly.GetManifestResourceStream(fileName);
        if (resFilestream == null)
        {
            return null;
        }

        var ba = new byte[resFilestream.Length];
        resFilestream.ReadExactly(ba, 0, ba.Length);
        return ba;
    }
}