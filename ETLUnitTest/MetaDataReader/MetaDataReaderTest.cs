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
            currentconnection.ConnectionString = @"Data Source=localhost;Initial Catalog=eOS;User Id=sa;Password=05276903;";
            currentconnection.Type = ServerType.SqlServer;

            metadataReader = MetadataFactory.GetReader(currentconnection.Type, currentconnection.ConnectionString);

            var owners = metadataReader.GetOwners();
            //var owner = owners.Contains("db_owner");
            var tables = metadataReader.GetTables(owners[2]);

            var tableuser = metadataReader.GetTableDetails(tables[1], owners[2]);

            Assert.AreEqual(13, 13);
        }
    }
}
