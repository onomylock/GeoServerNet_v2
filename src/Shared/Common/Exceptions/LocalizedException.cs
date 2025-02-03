namespace Shared.Common.Exceptions;

public abstract class LocalizedException : Exception
{
    protected LocalizedException()
    {
    }

    protected LocalizedException(string message) : base(message)
    {
    }

    protected LocalizedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Transforms exception type name, to a localizable string
    /// </summary>
    public string AsUi => $"#UI_{GetType().FullName?.ToUpper()}";
}