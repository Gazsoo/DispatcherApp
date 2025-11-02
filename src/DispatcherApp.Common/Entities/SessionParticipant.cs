using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Entities;
public class SessionParticipant
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public DispatcherSession Session { get; set; } = null!;

    public string UserId { get; set; } = null!;


}
