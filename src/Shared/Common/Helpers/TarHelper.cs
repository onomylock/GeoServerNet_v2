using ICSharpCode.SharpZipLib.Tar;

namespace Shared.Common.Helpers;

public static class TarHelper
{
    public static void AddToTarManually(TarArchive tarArchive, string sourceDirectory)
    {
        if (!Directory.Exists(sourceDirectory))
            return;

        var sourceDirectoryInfo = new DirectoryInfo(sourceDirectory);

        foreach (var filePath in Directory.GetFiles(sourceDirectory))
        {
            var fileInfo = new FileInfo(filePath);
            var tarEntry = TarEntry.CreateEntryFromFile(filePath);

            tarEntry.Name = Path.Join(sourceDirectoryInfo.Name, fileInfo.Name);

            tarArchive.WriteEntry(tarEntry, true);
        }
    }
}