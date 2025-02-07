using System.ComponentModel.DataAnnotations.Schema;
using Shared.Domain.Entity.Base;

namespace NodeServer.Domain.Entities;

public record NodeServerSolution : EntityBase
{
    public Guid MasterServerSolutionId { get; set; }
    public string DirectoryPath { get; set; }
    public string FileExePath { get; set; }
    public string DirectoryResultsPath { get; set; }
    [Column(TypeName = "jsonb")] public string ArgumentsMask { get; set; }
}