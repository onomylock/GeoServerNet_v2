using System.ComponentModel.DataAnnotations;
using Shared.Common.Models;

namespace ReverseProxy.Application.Models.Options;

public class ReverseProxyOptions
{
    [Required] public UriData MasterServerJsonWebTokenExpiredChannel { get; set; }
    [Required] public UriData MasterServerJsonWebTokenAuthenticationChannel { get; set; }
}