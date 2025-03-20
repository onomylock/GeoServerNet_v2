namespace Shared.Common.Models;

public static class SignalRKey
{
    public static string SignalRHubDisconnectFilterKey => "#GK-DisconnectFilter:UserId<{0}>";
}