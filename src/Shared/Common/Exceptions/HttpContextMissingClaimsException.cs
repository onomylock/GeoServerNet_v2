namespace Shared.Common.Exceptions;

public class HttpContextMissingClaimsException : LocalizedException
{
    public HttpContextMissingClaimsException(string message) : base(message)
    {
    }
}