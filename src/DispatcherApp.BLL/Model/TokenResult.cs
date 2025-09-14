using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.BLL.Model;
public class TokenResult
{
    public required string  AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int ExpiresIn => (int)(ExpiresAt - DateTime.UtcNow).TotalSeconds;
}
