using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.Common.Abstractions.Repository;

namespace DispatcherApp.Common.Exceptions;
public static class GuardExtention
{

    public static IVersionedEntity Concurrent(this IGuardClause guardClause,
        IVersionedEntity input,
        long previous,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input.Version != previous)
        {
            Exception? exception = exceptionCreator?.Invoke();

            if (string.IsNullOrEmpty(message))
            {
                throw exception ?? new ConcurrencyException("Parameter: " + parameterName);
            }
            throw exception ?? new ConcurrencyException("Parameter: " + parameterName + " message: " + message);
        }

        return input;
    }
}
