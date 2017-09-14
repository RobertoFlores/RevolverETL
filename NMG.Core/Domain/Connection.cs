using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMG.Core.Domain
{
    public class Connection
    {
        public Guid Id { get; set; }
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public ServerType Type { get; set; }
    }
}
