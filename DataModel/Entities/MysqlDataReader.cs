using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using DataModel.Interfaces;
using DataModel.Utils;
using NLog;

namespace DataModel.Entities
{
    [Serializable()]
    public class MysqlDataReader : IDataBaseReader
    {
        public MysqlDataReader(string raddress, string dbport, string dbname, string dbuser, string dbpassword)
        {
            RemoteAdress = raddress;
            DataBasePort = dbport; 
            DataBaseName = dbname;
            User = dbuser;
            Password = dbpassword;
            DatabaseDriver = DatabaseDriver.MySQL;
        }
        public MysqlDataReader(string connectionstring)
        {
            connectionStr = connectionstring;
            DatabaseDriver = DatabaseDriver.MySQL;
        }
        public MysqlDataReader()
        {
            //TBA
            DatabaseDriver = DatabaseDriver.MySQL;
        }
        private string connectionStr = "";
        private string _Name { get; set; }
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        private Logger logger = LogManager.GetCurrentClassLogger();
        public string RemoteAdress { get; set; }
        public string DataBasePort { get; set; }
        public string DataBaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public DatabaseDriver DatabaseDriver { get; set; }

        public void Enable()
        {
            //TBA
        }

        public void Disable()
        {
            //TBA
        }
        public void SetConnection(string connectionstring)
        {
            this.connectionStr = connectionstring;
        }
        public int GetCount(string table, string dbname)
        {
            var conn = new MySqlConnection(connectionStr);
            conn.Open();
            int count = 0;
            string stringcount = "";
            try
            {
                using (conn)
                {
                    var tableCommand = conn.CreateCommand();
                    tableCommand.CommandText = string.Format("select count(*) from {0}", table);

                    var sqlDataReader = tableCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    while (sqlDataReader.Read())
                    {
                        stringcount = sqlDataReader.GetString(0);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            bool resultParsing = int.TryParse(stringcount, out count);

            if (resultParsing)
            {
                return count;
            }
            else
            {
                return 0;
            }
        }
        
        public DataTable GetAllFromTable(string table, string dbname)
        {
            DataTable result = new DataTable();
            var conn = new MySqlConnection(connectionStr);
            conn.Open();
            
            try
            {
                using (conn)
                {
                    var tableCommand = conn.CreateCommand();

                    tableCommand.CommandText = String.Format("select * from {0}", table);

                    MySqlDataAdapter da = new MySqlDataAdapter();

                    da.SelectCommand = tableCommand;
                    
                    da.Fill(result);
                   
                }
            }
            catch(Exception ex)
            {
                logger.Error("Error in GetAllFromTable", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            if (result.Rows.Count > 0)
            {
                return result;
            }
            else
            {
                return new DataTable();
            }
      
        }

        public DataTable GetSelectedColsFromTable(string table, string dbname, List<string> columns)
        {
            DataTable result = new DataTable();
            var conn = new MySqlConnection(connectionStr);
            conn.Open();
           
            try
            {
                using (conn)
                {
                    var tableCommand = conn.CreateCommand();

                    tableCommand.CommandText = CreateSqlCommand(table, columns);

                    MySqlDataAdapter da = new MySqlDataAdapter();

                    da.SelectCommand = tableCommand;

                    da.Fill(result);

                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetAllFromTable", ex.Message);
            }
            finally
            {
                conn.Close();
            }


            if (result.Rows.Count > 0)
            {
                return result;
            }
            else
            {
                return new DataTable();
            }
        }

        private string CreateSqlCommand(string table, List<string> columns)
        {
            string result = "";
            if(columns.Count > 0)
            {
                string fields = " ";
                var lastitem = columns.Last();
                foreach(string col in columns)
                {
                    if(col == lastitem)
                    {
                        fields = fields + col;
                    }
                    else
                    {
                        fields = fields + col + ", ";
                    }
                    

                }

                return string.Format("select {0} from {1}",fields, table);
            }
            else
            {
                return result;
            }
        }
        public void CreateConnectionStr(string raddress, string dbport, string dbname, string dbuser, string dbpassword)
        {
            connectionStr = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", raddress, dbport, dbname, dbuser, dbpassword);
        }

        public DataTable GetTables(string dbname)
        {
            DataTable result = new DataTable();
            var conn = new MySqlConnection(connectionStr);
            conn.Open();
            int count = 0;
            string stringcount = "";
            try
            {
                using (conn)
                {
                    var tableCommand = conn.CreateCommand();

                    tableCommand.CommandText = String.Format("select table_name from information_schema.tables where table_type like 'BASE TABLE' and TABLE_SCHEMA = '{0}'", dbname);

                    MySqlDataAdapter da = new MySqlDataAdapter();

                    da.SelectCommand = tableCommand;

                    da.Fill(result);

                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetTables", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            bool resultParsing = int.TryParse(stringcount, out count);

            if (result.Rows.Count > 0)
            {
                return result;
            }
            else
            {
                return new DataTable();
            }
        }

        public DataColumnCollection GetTableDetails(string table, string dbname)
        {
            //select * from usuario LIMIT 1
            DataTable result = new DataTable();
            var conn = new MySqlConnection(connectionStr);
            conn.Open();
            
            try
            {
                using (conn)
                {
                    var tableCommand = conn.CreateCommand();

                    tableCommand.CommandText = String.Format("select * from {0} limit 1", table);

                    MySqlDataAdapter da = new MySqlDataAdapter();

                    da.SelectCommand = tableCommand;

                    da.Fill(result);

                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetTableDetails", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return result.Columns;
        
        }
    }
}
