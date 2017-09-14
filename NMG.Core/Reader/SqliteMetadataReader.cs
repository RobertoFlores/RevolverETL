﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using NMG.Core.Domain;

namespace NMG.Core.Reader
{
    public class SqliteMetadataReader : IMetadataReader
    {
        private readonly string _connectionStr;

        public SqliteMetadataReader(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        public IList<Column> GetTableDetails(Table table, string owner)
        {
            var columns = new List<Column>();

            using (var sqlCon = new SQLiteConnection(_connectionStr))
            {
                try
                {
                    using (var tableDetailsCommand = sqlCon.CreateCommand())
                    {
                        tableDetailsCommand.CommandText = string.Format("SELECT * FROM {0}", table.Name);
                        sqlCon.Open();

                        var dr = tableDetailsCommand.ExecuteReader(CommandBehavior.SchemaOnly);
                        var dt = dr.GetSchemaTable();
                        var m = new DataTypeMapper();

                        foreach (DataRow row in dt.Rows)
                        {
                            columns.Add(
                                    new Column
                                    {
                                        Name = row["ColumnName"].ToString(),
                                        IsNullable = (bool)row["AllowDBNull"],
                                        IsPrimaryKey = (bool)row["IsKey"],
                                        MappedDataType = m.MapFromDBType(ServerType.SQLite, row["DataTypeName"].ToString(), (int)row["ColumnSize"], null, null).ToString(),
                                        DataLength = (int)row["ColumnSize"],
                                        DataType = row["DataTypeName"].ToString(),
                                        IsUnique = (bool)row["IsUnique"]
                                    });
                            dr.Close();
                        }
                    }

                    table.Owner = owner;
                    table.Columns = columns;
                    table.PrimaryKey = DeterminePrimaryKeys(table);
                    table.ForeignKeys = new List<ForeignKey>();// DetermineForeignKeyReferences(table);
                    table.HasManyRelationships = new List<HasMany>();// DetermineHasManyRelationships(table);
                }
                finally
                {
                    sqlCon.Close();
                }
            }

            return columns;
        }

        public List<Table> GetTables(string owner)
        {
            var tables = new List<Table>();

            using (var sqlCon = new SQLiteConnection(_connectionStr))
            {
                sqlCon.Open();
                try
                {
                    using (var tableDetailsCommand = sqlCon.CreateCommand())
                    {
                        tableDetailsCommand.CommandText =
                            "SELECT name FROM sqlite_master WHERE type in ('table', 'view') AND name not like 'sqlite?_%' escape '?'";
                        using (var sqlDataReader = tableDetailsCommand.ExecuteReader(CommandBehavior.Default))
                        {
                            while (sqlDataReader.Read())
                            {
                                tables.Add(new Table { Name = sqlDataReader.GetString(0) });
                            }
                        }
                    }
                }
                finally
                {
                    sqlCon.Close();
                }
            }

            return tables;
        }

        public IList<string> GetOwners()
        {
            return new List<string>{"master"};
        }

        public List<string> GetSequences(string owner)
        {
            return new List<string>();
        }

        public PrimaryKey DeterminePrimaryKeys(Table table)
        {
            var primaryKeys = table.Columns.Where(x => x.IsPrimaryKey.Equals(true)).ToList();

            if (primaryKeys.Count() == 1)
            {
                var c = primaryKeys.First();
                var key = new PrimaryKey
                {
                    Type = PrimaryKeyType.PrimaryKey,
                    Columns = { c }
                };
                return key;
            }
            
            if (primaryKeys.Count() > 1)
            {
                var key = new PrimaryKey
                {
                    Type = PrimaryKeyType.CompositeKey,
                    Columns = primaryKeys
                };
                return key;
            }
            
            return null;
        }

        public IList<ForeignKey> DetermineForeignKeyReferences(Table table)
        {
            var foreignKeys = table.Columns.Where(x => x.IsForeignKey).Distinct()
                                   .Select(c => new ForeignKey
                                   {
                                       Name = c.Name,
                                       References = c.ForeignKeyTableName,
                                       Columns = DetermineColumnsForForeignKey(table.Columns, c.ConstraintName)
                                   }).ToList();

            Table.SetUniqueNamesForForeignKeyProperties(foreignKeys);

            return foreignKeys;
        }

        /// <summary>
        /// Search for one or more columns that make up the foreign key.
        /// </summary>
        /// <param name="columns">All columns that could be used for the foreign key</param>
        /// <param name="foreignKeyName">Name of the foreign key constraint</param>
        /// <returns>List of columns associated with the foreign key</returns>
        /// <remarks>Composite foreign key will return multiple columns</remarks>
        private IList<Column> DetermineColumnsForForeignKey(IList<Column> columns, string foreignKeyName)
        {
            return (from c in columns
                    where c.IsForeignKey && c.ConstraintName == foreignKeyName
                    select c).ToList();
        }

    }

    public class SqliteDataType
    {
        public SqliteDataType(string sqliteType)
        {
            if (sqliteType.Contains("("))
            {
                var typeSplit = sqliteType.Replace(")", string.Empty).Split('(');
                DataType = typeSplit[0];
                DataLength = int.Parse(typeSplit[1]);
            }
        }
        public string DataType { get; set; }
        public int? DataLength { get; set; }
    }
}