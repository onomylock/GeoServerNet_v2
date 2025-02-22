namespace Shared.Common.Models.DTO.Base;

public class ErrorBase(string message)
{
    public string Message { get; } = message;
}