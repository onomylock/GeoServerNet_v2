using Microsoft.AspNetCore.SignalR.Client;

namespace ReverseProxy.Application.Services;

public interface ISignalRClientService
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    HubConnectionState ConnectionState { get; }
}