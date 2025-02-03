using System.Diagnostics;
using Shared.Domain.View;

namespace Shared.Common.Helpers;

public static class ProcessHelper
{
    public static Task<ProcessStartInfo> GetProcessStartInfoAsync(List<KeyValueEntry> metadata)
    {
        var processStartInfo = new ProcessStartInfo
        {
            Arguments = metadata.First(x => x.Key == "Arguments").Value ?? string.Empty,
            FileName = metadata.First(x => x.Key == "FileName").Value ?? throw new InvalidOperationException(),
            UseShellExecute = true,
            WorkingDirectory = metadata.First(x => x.Key == "WorkingDirectory").Value ?? string.Empty
        };

        return Task.FromResult(processStartInfo);
    }
}