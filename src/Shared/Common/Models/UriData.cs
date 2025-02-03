using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models;

public record UriData
{
    public string Scheme { get; set; }
    public string Host { get; set; }

    [Range(-1, ushort.MaxValue)] public int Port { get; set; }

    public string Path { get; set; }
}