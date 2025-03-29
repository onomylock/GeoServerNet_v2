using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ReverseProxy.Application.Models.Options;
using ReverseProxy.Application.Services;

namespace ReverseProxy.Infrastructure.Services;

public class SignalRClientService : ISignalRClientService
{
    private readonly ILogger<SignalRClientService> _logger;
    private HubConnection _hubConnection;
    private IDisposable _hubConnectionHandler;
    private ReverseProxyOptions _reverseProxyOptions;
    private IMediator _mediator;
    
    public HubConnectionState ConnectionState => _hubConnection.State;

    public SignalRClientService(ILogger<SignalRClientService> logger, ReverseProxyOptions reverseProxyOptions, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
        _reverseProxyOptions = reverseProxyOptions;
        
        var uri = UriHelper.BuildAbsolute(scheme: _reverseProxyOptions.BaseUri.Scheme,
            path: PathString.FromUriComponent(_reverseProxyOptions.BaseUri.Path),
            host: HostString.FromUriComponent(_reverseProxyOptions.BaseUri.Host));

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(uri)
            .Build();
    }

    private async Task OnNotification(INotification notification)
    {
        await _mediator.Publish(notification);
    }
    
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _hubConnectionHandler = _hubConnection.On<INotification>(_reverseProxyOptions.MethodCaller, OnNotification);
        await _hubConnection.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        _hubConnectionHandler.Dispose();
        await _hubConnection.DisposeAsync();
    }
}