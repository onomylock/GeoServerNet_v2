using System.Net;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Node : EntityBase
{
    public IPAddress Address { get; set; }
}