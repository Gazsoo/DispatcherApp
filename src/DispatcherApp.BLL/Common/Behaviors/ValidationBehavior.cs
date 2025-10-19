using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace DispatcherApp.BLL.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationTasks = _validators.Select(validator => validator.ValidateAsync(context, cancellationToken));
            var results = await Task.WhenAll(validationTasks);

            var failures = results
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new DispatcherApp.Common.Exceptions.ValidationException(failures);
            }
        }

        return await next();
    }
}
