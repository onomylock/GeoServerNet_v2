using Microsoft.Extensions.Hosting;

namespace Shared.Common.BackgroundServices;

public abstract class RestartableBackgroundService : BackgroundService
{
    private volatile CancellationTokenSource _cts;
    private volatile Task _task;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var ct = MakeCancellationToken(stoppingToken);
                
                await Run(ct);
            }
            catch (OperationCanceledException)
            {
                //ignored
            }
    }
    
    private CancellationToken MakeCancellationToken(CancellationToken stoppingToken = default)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        _cts = cts;
        
        return cts.Token;
    }
    
    private async Task Run(CancellationToken cancellationToken = default)
    {
        var task = await Task.Factory.StartNew(() => DoWorkAsync(cancellationToken), TaskCreationOptions.LongRunning | TaskCreationOptions.RunContinuationsAsynchronously);
        _task = task;
        
        await task;
    }
    
    protected abstract Task DoWorkAsync(CancellationToken cancellationToken);
    
    protected void Restart()
    {
        var ctsPrev = _cts;
        
        ctsPrev?.Cancel();
        
        _task?.Wait(CancellationToken.None);
        
        ctsPrev?.Dispose();
    }
}