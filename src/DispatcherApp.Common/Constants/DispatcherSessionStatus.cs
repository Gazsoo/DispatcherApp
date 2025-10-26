using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Constants;
public enum DispatcherSessionStatus
{
    Scheduled,
    Started,
    InProgress,
    Postponed,
    Canceled,
    Finished
}
