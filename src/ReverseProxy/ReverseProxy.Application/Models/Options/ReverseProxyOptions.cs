using System.ComponentModel.DataAnnotations;
using Shared.Common.Models;

namespace ReverseProxy.Application.Models.Options;

public class ReverseProxyOptions
{
    [Required] public UriData BaseUri { get; set; }
    
    [Required] public string MethodCaller { get; set; }
    [Required] public int ErrorCooldown { get; set; }
}