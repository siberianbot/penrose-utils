namespace Penrose.Utils.Tests.AssetPacker;

public static class Paths
{
    public static readonly string BasePath = Path.Combine(Path.GetTempPath(), "Penrose.Utils.Tests.AssetPacker");

    static Paths()
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }
}