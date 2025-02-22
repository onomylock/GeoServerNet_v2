using Shared.Common.Enums;

namespace Shared.Common.Models.DTO.Base;

public sealed class ErrorModelResultEntry(
    ErrorType errorType,
    string message,
    ErrorEntryType errorEntryType = ErrorEntryType.None) : ErrorBase(message)
{
    public ErrorType ErrorType { get; } = errorType;
    
    public ErrorEntryType ErrorEntryType { get; } = errorEntryType;
}