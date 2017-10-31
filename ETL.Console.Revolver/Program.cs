using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMG.Core;
using NMG.Core.Domain;
using NMG.Core.Reader;

namespace ETL.Console.Revolver
{
    class Program
    {
        static void Main(string[] args)
        {
            IMetadataReader metadataReader;

            Connection currentconnection = new Connection();
            currentconnection.Id = new Guid();
            currentconnection.ConnectionString = @"Data Source=localhost;Initial Catalog=eOS;User Id=sa;Password=05276903;";
            currentconnection.Type = ServerType.SqlServer;

            metadataReader = MetadataFactory.GetReader(currentconnection.Type, currentconnection.ConnectionString);

            var owners = metadataReader.GetOwners();
            var tables = metadataReader.GetTables(owners[2]);

            var tableuser = metadataReader.GetTableDetails(tables[1], owners[2]);

            foreach(var col in tableuser)
            {
                System.Console.WriteLine("Col Name:    {0}",col.Name);
                //Console.
            }

            System.Console.ReadKey();

        }
    }
}
