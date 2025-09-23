using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.Exceptions;
public class ValidationException: Exception
{
    public ValidationException(string message) : base(message) {
        Errors = new Dictionary<string, string[]>();
            }
    public ValidationException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
