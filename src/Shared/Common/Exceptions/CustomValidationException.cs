using Shared.Common.Models.DTO.Base;

namespace Shared.Common.Exceptions;

public class CustomValidationException() : Exception("One or more validation failures have occured.")
{
    public IEnumerable<ErrorBase> Errors { get; } = new List<ErrorBase>();

    public CustomValidationException(IEnumerable<ErrorBase> errors)
        : this() => Errors = errors;
}