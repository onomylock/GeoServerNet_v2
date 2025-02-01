using System.Net;
using Shared.Domain.Entity;
using Shared.Domain.Entity.Base;

namespace MasterServer.Domain.Entities;

public record Node: EntityBase
{
    IPAddress Address { get; set; }
}