using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Interfaces;

namespace DataModel.Entities
{
    public class DataBaseReaderFactory : IDataBaseReaderFactory
    {
        public IDataBaseReader CreateDataBaseReader(DatabaseDriver DatabaseDriver)
        {
            switch (DatabaseDriver)
            {
                case DatabaseDriver.MySQL:
                    return new MysqlDataReader();
                //case DatabaseDriver.SQLServer:
                //    return new SQLServerDataReader();
                //case DatabaseDriver.SQLite:
                //    return new SQLiteDataReader();
                default:
                    return new MysqlDataReader();
            }
        }
    }
}
