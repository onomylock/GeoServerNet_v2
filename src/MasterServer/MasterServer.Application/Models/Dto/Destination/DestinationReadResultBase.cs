using System.Net;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.Destination;

public class DestinationReadResultBase : EntityResponseBase
{
    public string Alias { get; set; }
    public string Host { get; set; }
    public string Address { get; set; }
}