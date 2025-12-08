using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.Session;
public record ParticipantDto(
    string UserId, 
    string Name);
