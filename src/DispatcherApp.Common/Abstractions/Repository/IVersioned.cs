using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Abstractions.Repository;
public interface IVersionedEntity
{
    long Version { get; set; }
}
