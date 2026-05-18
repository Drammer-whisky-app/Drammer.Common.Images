using System.Reflection;

namespace Drammer.Common.Images.Tests;

internal static class TestHelpers
{
    public const string FileName1 = "DALL·E 2025-03-10 16.25.30.webp";
    public const string ResourceName1 = $"Drammer.Common.Images.Tests.Resources.{FileName1}";

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