using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.DAL.Configurations;
public class EmailSettings
{
    public const string SectionName = "EmailSettings";
    public string SenderAddress { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}
