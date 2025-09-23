using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.Exceptions;
public class NotFoundException: Exception
{
    public string EntityName { get; }
    public object Key { get; }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }

}
