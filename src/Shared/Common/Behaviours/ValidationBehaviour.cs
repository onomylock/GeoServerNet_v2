using FluentValidation;
using MediatR;
using Shared.Common.Exceptions;
using Shared.Common.Models.DTO.Base;

namespace Shared.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(r => new ErrorBase { PropertyMessage = r.PropertyName, ErrorMessage = r.ErrorMessage })
            .ToList();

        if (errors.Any()) throw new CustomValidationException(errors);

        return await next();
    }
}