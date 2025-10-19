using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace DispatcherApp.Common.Exceptions;
public class ValidationException: Exception
{
    public ValidationException(string message) : base(message) {
        Errors = new Dictionary<string, string[]>();
            }
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this("Errors")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
