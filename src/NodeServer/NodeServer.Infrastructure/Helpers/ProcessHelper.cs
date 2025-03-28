using System.Diagnostics;
using Shared.Domain.View;

namespace NodeServer.Infrastructure.Helpers;

public static class ProcessHelper
{
    public static ProcessStartInfo ConfigureProcessStartInfo(List<KeyValueEntry> keyValueEntries,
        string workingDirectory)
    {
        return new ProcessStartInfo
        {
            Arguments = keyValueEntries.Find(_ => _.Key == nameof(ProcessStartInfo.Arguments)).Value ?? string.Empty,
            ErrorDialogParentHandle = 0,
            FileName = keyValueEntries.Find(_ => _.Key == nameof(ProcessStartInfo.FileName)).Value ?? string.Empty,
            UseShellExecute = false,
            WorkingDirectory = workingDirectory
        };
    }
}