using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Entities;

namespace DataModel.Interfaces
{
    interface IDataBaseReaderFactory
    {
        IDataBaseReader CreateDataBaseReader(DatabaseDriver DatabaseDriver);
    }
}
