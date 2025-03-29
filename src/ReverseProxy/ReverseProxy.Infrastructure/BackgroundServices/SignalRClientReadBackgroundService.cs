using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ReverseProxy.Application.Models.Options;
using ReverseProxy.Application.Services;
using Shared.Common.BackgroundServices;

namespace ReverseProxy.Infrastructure.BackgroundServices;

public class SignalRClientReadBackgroundService : RestartableBackgroundService
{
    private readonly ISignalRClientService _signalRClientService;
    private readonly ILogger _logger;
    private readonly ReverseProxyOptions _reverseProxyOptions;
    
    public SignalRClientReadBackgroundService(ILogger logger, ISignalRClientService signalRClientService, ReverseProxyOptions reverseProxyOptions)
    {
        _logger = logger;
        _signalRClientService = signalRClientService;
        _reverseProxyOptions = reverseProxyOptions;
    }
    
    protected override async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
                try
                {
                    if (_signalRClientService.ConnectionState != HubConnectionState.Connected)
                    {
                        await _signalRClientService.StartAsync(cancellationToken);    
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error occured while starting {nameof(_signalRClientService)}");
                    
                    await Task.Delay(TimeSpan.FromMilliseconds(_reverseProxyOptions.ErrorCooldown), cancellationToken);
                }
        }
        catch (OperationCanceledException)
        {
            //ignored
        }
        finally
        {
            await _signalRClientService.StopAsync(CancellationToken.None);
        }
    }
}