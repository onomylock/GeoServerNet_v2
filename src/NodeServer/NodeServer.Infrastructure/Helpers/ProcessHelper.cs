using System.Diagnostics;
using Shared.Domain.View;

namespace NodeServer.Infrastructure.Helpers;

public static class ProcessHelper
{
    public static ProcessStartInfo ConfigureProcessStartInfo(List<KeyValueEntry> keyValueEntries, string workingDirectory)
    {
        return new ProcessStartInfo
        {
            Arguments = keyValueEntries.Find(_ => _.Key == nameof(ProcessStartInfo.Arguments)).Value ?? string.Empty,
            // CreateNoWindow = false,
            // ErrorDialog = false,
            ErrorDialogParentHandle = 0,
            FileName = keyValueEntries.Find(_ => _.Key == nameof(ProcessStartInfo.FileName)).Value ?? string.Empty,
            // PasswordInClearText = null,
            // RedirectStandardError = false,
            // RedirectStandardInput = false,
            // RedirectStandardOutput = false,
            // StandardErrorEncoding = null,
            // StandardInputEncoding = null,
            // StandardOutputEncoding = null,
            // UserName = null,
            UseShellExecute = false,
            WorkingDirectory = workingDirectory
        };
    }
}