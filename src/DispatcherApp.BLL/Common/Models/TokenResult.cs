using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.BLL.Common.Models;
public class TokenResult
{
    public required string  AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public TimeProvider TimeProvider { get; init; } = TimeProvider.System;
    public int ExpiresIn => (int)(ExpiresAt - TimeProvider.GetUtcNow().UtcDateTime).TotalSeconds;
}
