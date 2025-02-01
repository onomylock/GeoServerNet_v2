using System.Net;
using Shared.Domain.Entity;

namespace MasterServer.Domain.Entities;

public record Node: EntityBase
{
    IPAddress Address { get; set; }
}