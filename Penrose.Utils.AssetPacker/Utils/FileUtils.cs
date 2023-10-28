namespace Penrose.Utils.AssetPacker.Utils;

public static class FileUtils
{
    public static string? GetExtension(string filename)
    {
        int indexOfPeriod = filename.LastIndexOf('.');

        if (indexOfPeriod < 0)
        {
            return null;
        }

        string extension = filename[indexOfPeriod..].ToLower();

        if (extension != ".xml")
        {
            return extension;
        }

        indexOfPeriod = filename.LastIndexOf('.', indexOfPeriod - 1);

        return filename[indexOfPeriod..].ToLower();
    }
}