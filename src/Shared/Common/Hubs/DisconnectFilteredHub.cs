using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Common.Enums;
using Shared.Common.Exceptions;
using Shared.Common.Hubs.Base;
using Shared.Common.Models;
using Shared.Common.Models.Dto;
using Shared.Common.Models.DTO.Base;

namespace Shared.Common.Hubs;

/// <summary>
///     Hub that disconnects users. Use to disconnect users which expired/revoked JWT.
/// </summary>
public interface IDisconnectFilteredHub
{
    /// <summary>
    ///     Invoke to end connections by connection with filter added
    /// </summary>
    /// <remarks>Can ONLY be invoked from client call!</remarks>
    public void InvokeDisconnectFilter();

    public Task AddDisconnectFilter(string connectionId, Func<(bool result, string reason)> filter);
    public void DeleteDisconnectFilter(Func<KeyValuePair<string, HubConnectionInfo>, bool> predicate);
}

public interface IDisconnectFilteredHubActions : IHubBaseAction;

public record HubConnectionInfo(Func<(bool result, string reason)> DisconnectFilter, HubCallerContext HubCallerContext);

/// <summary>
///     Hub that disconnects users. Use to disconnect users which expired/revoked JWT.
/// </summary>
public abstract class DisconnectFilteredHub<T>(
    IHostEnvironment hostEnvironment,
    ILogger<HubBase<IDisconnectFilteredHubActions>> loggerDisconnectFilteredHub,
    ILogger<HubBase<T>> loggerHubBase,
    IHttpContextAccessor httpContextAccessor
)
    : HubBase<T>(loggerHubBase, hostEnvironment), IDisconnectFilteredHub where T : class, IDisconnectFilteredHubActions
{
    private static readonly ConcurrentDictionary<string, HubConnectionInfo> HubConnectionInfos = new();
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;

    /// <summary>
    ///     Invoke to end connections by connection with filter added
    /// </summary>
    /// <remarks>Can ONLY be invoked from client call!</remarks>
    public void InvokeDisconnectFilter()
    {
        var httpContextFeature = Context.Features.Get<IHttpContextFeature>()!;

        foreach (var _ in HubConnectionInfos.Select(_ => new
                 {
                     ConnectionId = _.Key, DisconnectFilterResult = _.Value.DisconnectFilter.Invoke(),
                     _.Value.HubCallerContext
                 }).Where(_ => _.DisconnectFilterResult.result))
        {
            Clients.Client(_.ConnectionId).ReceiveErrorModelResult(new ErrorModelResult
            {
                Errors =
                [
                    new ErrorModelResultEntry(ErrorType.Generic,
                        _.DisconnectFilterResult.reason ?? Localize.Keys.Error.SignalRDisconnectFiltered,
                        ErrorEntryType.Message)
                ]
            });
            _.HubCallerContext.Abort();
        }
    }

    public async Task AddDisconnectFilter(string connectionId, Func<(bool result, string reason)> filter)
    {
        if (!HubConnectionInfos.TryAdd(Context.ConnectionId, new HubConnectionInfo(filter, Context)))
            await ThrowException(new AddDisconnectFilterFailedException(), abortConnection: true);
    }

    public void DeleteDisconnectFilter(Func<KeyValuePair<string, HubConnectionInfo>, bool> predicate)
    {
        if (!HubConnectionInfos.TryRemove(HubConnectionInfos.SingleOrDefault(predicate)))
            loggerDisconnectFilteredHub.LogWarning(
                "[{typeNameOf}:{functionNameOf}] Failed to remove disconnect filter for {ConnectionId} connection",
                nameof(DisconnectFilteredHub<T>), nameof(DeleteDisconnectFilter),
                Context.ConnectionId);
    }

    public override async Task OnConnectedAsync()
    {
        if (!Guid.TryParse(_httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimKey.UserId)?.Value,
                out var userId) || userId == default)
            await ThrowException(new HttpContextMissingClaimsException(ClaimKey.UserId), abortConnection: true);

        if (!int.TryParse(_httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimKey.ExpiresAt)?.Value,
                out var expiresAt))
            await ThrowException(new HttpContextMissingClaimsException(ClaimKey.ExpiresAt), abortConnection: true);

        var key = string.Format(SignalRKey.SignalRHubDisconnectFilterKey, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, key);
        await AddDisconnectFilter(Context.ConnectionId,
            () => (DateTimeOffset.FromUnixTimeSeconds(expiresAt) < DateTimeOffset.UtcNow,
                Localize.Keys.Error.SignalRDisconnectFiltered));

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (!Guid.TryParse(_httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimKey.UserId)?.Value,
                out var userId) && userId != default)
            if (userId != default)
            {
                var key = string.Format(SignalRKey.SignalRHubDisconnectFilterKey, userId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, key);
                DeleteDisconnectFilter(_ => _.Key == Context.ConnectionId);
            }

        await base.OnDisconnectedAsync(exception);
    }

    private class AddDisconnectFilterFailedException : LocalizedException;
}