using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
namespace ETLUnitTest.DataModel
{
    [TestClass]
    public class DbConnectUnitTest
    {
        #region MysqlReader
        [TestMethod]
        public void MysqlGetTablesTest()
        {
            const string connectionString = "Server=localhost;Database=adrepdbhuella;Uid=admin;Pwd=password";
            var database = new MysqlDataReader(connectionString);
            database.Name = "MySQL Test";
            var tables = database.GetTables("adrepdbhuella");
 
            Assert.AreEqual(98, tables.Rows.Count);
        }

        [TestMethod]
        public void MysqlTableGetCountTest()
        {
            const string connectionString = "Server=localhost;Database=adrepdbhuella;Uid=admin;Pwd=password";
            var database = new MysqlDataReader(connectionString);
            database.Name = "MySQL Test";
            var tables = database.GetTables("adrepdbhuella");
            var tablecount = database.GetCount("asistencia", "adrepdbhuella");
            Assert.AreEqual(293, tablecount);
        }

        [TestMethod]
        public void GetAllFromTableTest()
        {
            const string connectionString = "Server=localhost;Database=adrepdbhuella;Uid=admin;Pwd=password";
            var reader = new MysqlDataReader();
            reader.SetConnection(connectionString);

            reader.Name = "MySQL Test";
            var tables = reader.GetTables("adrepdbhuella");
            var tableRows = reader.GetAllFromTable("asistencia", "adrepdbhuella").Rows;
            Assert.AreEqual(293, tableRows.Count);
        }

        [TestMethod]
        public void GetSelectedColsFromTable()
        {
            const string connectionString = "Server=localhost;Database=adrepdbhuella;Uid=admin;Pwd=password";
            var reader = new MysqlDataReader();
            reader.SetConnection(connectionString);

            reader.Name = "MySQL Test";
            var tables = reader.GetTables("adrepdbhuella");
            List<string> columns = new List<string>();
            columns.Add("idpersona"); //nombre, apaterno, amaterno
            columns.Add("nombre");
            columns.Add("apaterno");
            columns.Add("amaterno");
            var tableRows = reader.GetSelectedColsFromTable("persona", "adrepdbhuella", columns).Rows;
            Assert.AreEqual(104, tableRows.Count);
        }

        [TestMethod]
        public void GetTableDetailsTest()
        {
            const string connectionString = "Server=localhost;Database=adrepdbhuella;Uid=admin;Pwd=password";
            var reader = new MysqlDataReader();
            reader.SetConnection(connectionString);

            reader.Name = "MySQL Test";
            var tables = reader.GetTables("adrepdbhuella");
            //var tableusuario = tables.FirstOrDefault(x => x.Name == "usuario");
            var tableDetails = reader.GetTableDetails("persona", "adrepdbhuella");
            //var tableRows = reader.GetAllFromTable("turno", "devbostons").Rows;
            Assert.AreEqual(10, tableDetails.Count);
        }
        #endregion

        #region SQL Server DataReader

        #endregion
    }
}
