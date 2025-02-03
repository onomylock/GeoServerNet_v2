using System.ComponentModel.DataAnnotations.Schema;
using MemoryPack;
using Shared.Domain.Entity.Base;
using Shared.Domain.View;

namespace Shared.Domain.Entity;

/// <summary>
///     DO NOT delete in transaction scope via DeleteAsync, but rather Update IsInUse = false.
///     This way, if transaction fails, remote file will not be deleted, otherwise later freed by FileJobs
/// </summary>
public abstract record File : EntityBase
{
    public string FileName { get; set; }

    public string BucketName { get; set; }

    [MemoryPackIgnore] [NotMapped] public long Size { get; set; }

    /// <summary>
    ///     Used to store keys, which are not generic to file
    /// </summary>
    //TODO: in .NET 8 version of EFCore, ExecuteUpdate, ExecuteDelete fails with owned entities
    //TODO: https://github.com/dotnet/efcore/issues/32823
    public List<KeyValueEntry> Metadata { get; set; } = [];

    [MemoryPackIgnore] [NotMapped] public Stream Stream { get; set; }

    /// <summary>
    ///     Indicates whether file is in use, if not, entity deleted by FileJobs service after some time
    /// </summary>
    public bool IsInUse { get; set; }

    // public static class FileMetadataKey
    // {
    //     public const string FileType = "FileType";
    // }
}