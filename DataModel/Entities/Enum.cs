using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    public enum DatabaseDriver
    {
        MySQL = 1,
        SQLServer,
        SQLite,
        Oracle
    }

    public enum FileType
    {
        CSV =1,
        Excel,
        XML
    }

    public enum RemoteConnection
    {
        FTP = 1,
        RestFull,
        SOA,
        OData,
        WCF
    }

   
}
