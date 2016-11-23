using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Interfaces
{
    public interface IDataBaseReader
    {
        string Name { get; set; }
        void Enable();
        void Disable();

    }
}
