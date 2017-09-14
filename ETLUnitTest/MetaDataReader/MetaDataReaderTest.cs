using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMG.Core;
using NMG.Core.Domain;
using NMG.Core.Reader;


namespace ETLUnitTest.MetaDataReader
{
    [TestClass]
    public class MetaDataReaderTest
    {
        [TestMethod]
        public void GetTables()
        {
            IMetadataReader metadataReader;

            Connection currentconnection = new Connection();
            currentconnection.Id = new Guid();
            currentconnection.ConnectionString = @"Data Source=LOCALHOST\MSSQLSERVER2014;Initial Catalog=revolverETL;User Id=sa;Password=password;";
            currentconnection.Type = ServerType.SqlServer;

            metadataReader = MetadataFactory.GetReader(currentconnection.Type, currentconnection.ConnectionString);

            var owners = metadataReader.GetOwners();

            var tables = metadataReader.GetTables(owners[9]);

            var tableuser = metadataReader.GetTableDetails(tables[1], owners[9]);

            Assert.AreEqual(13, owners.Count);
        }
    }
}
