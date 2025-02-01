using System.Net;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Node : EntityBase
{
    private IPAddress Address { get; set; }
}