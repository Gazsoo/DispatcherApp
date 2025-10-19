using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Constants;
public static class Roles
{
    public const string Administrator = nameof(Administrator);
    public const string Dispatcher = nameof(Dispatcher);
    public const string User = nameof(User);

    public static readonly string[] All = { Administrator, Dispatcher, User };
}
