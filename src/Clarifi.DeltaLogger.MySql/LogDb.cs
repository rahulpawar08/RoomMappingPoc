using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Clarifi.DeltaLogger.Internal;
using global::MySql.Data.MySqlClient;

namespace Clarifi.DeltaLogger.MySql
{
    public class LogDb : ILogDb
    {
        public LogDb() : this(Settings.GetConnectionString()) { }

        public LogDb(string connStr)
        {
            ConnectionString = connStr;
        }

        public string ConnectionString { get; }

        public async Task LogAsync(TypeInfo type, string id, string parentId, IDictionary<string, object> values)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new MySqlCommand(ScriptBuilder.GetInsertCommand(type, id, parentId, values), conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 10;
                    await cmd.ExecuteNonQueryAsync();
                }
              await conn.CloseAsync();
            }
        }

        public async Task ProvisionAsync(TypeInfo[] types)
        {
            // 1. Drop existing tables
            // 2. Create new tables

            var tables = Array.ConvertAll(types, Table.CreateTable);
            using (var conn = new MySqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                var tx = await conn.BeginTransactionAsync();
                foreach (var table in tables)
                {

                    using (var delCmd = new MySqlCommand(ScriptBuilder.CreateDeleteScript(table.Name), conn, tx))
                    {
                        delCmd.CommandType = CommandType.Text;
                        delCmd.CommandTimeout = 10;
                        await delCmd.ExecuteNonQueryAsync();
                    }
                    using (var createCmd = new MySqlCommand(ScriptBuilder.GetCreateTableScript(table), conn, tx))
                    {
                        createCmd.CommandType = CommandType.Text;
                        createCmd.CommandTimeout = 10;
                        await createCmd.ExecuteNonQueryAsync();
                    }
                }
                tx.Commit();
                await conn.CloseAsync();
            }
        }




    }
}
