using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DispatcherApp.BLL.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        using (_logger.BeginScope("MediatR Request: {RequestName}", requestName))
        {
            _logger.LogInformation("Handling {RequestName} {@Request}", requestName, request);

            try
            {
                var response = await next();
                _logger.LogInformation("Handled {RequestName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                var logLevel = ex switch
                {
                    IAlreadyLoggedException => (LogLevel?)null,
                    ValidationException => LogLevel.Information,
                    ForbiddenAccessException => LogLevel.Warning,
                    //EmailNotConfirmedException => LogLevel.Warning,
                    //InvalidCredentialsException => LogLevel.Warning,
                    ConcurrencyException => LogLevel.Warning,
                    Ardalis.GuardClauses.NotFoundException => LogLevel.Information,
                    _ => LogLevel.Error
                };

                if (logLevel.HasValue)
                {
                    _logger.Log(logLevel.Value, ex, "Unhandled exception for {RequestName}", requestName);
                }

                throw;
            }
        }
    }
}
