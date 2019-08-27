using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Clarifi.RoomMappingLogger.Internal;
using global::MySql.Data.MySqlClient;

namespace Clarifi.RoomMappingLogger.MySql
{
    public class LogDb : ILogDb, IDisposable
    {
        MySqlConnection _conn = null;
        string _connectionString { get; }

        public LogDb() : this(Settings.GetConnectionString()) { }

        public LogDb(string connStr)
        {
            _connectionString = connStr;
            Task.Run(() => Initialize()).Wait();
        }

        #region Public Methods
        public void Dispose()
        {
            Task.Run(() => DeInitialize()).Wait();
        }

        public async Task LogAsync(TypeInfo type, string id, string parentId, IDictionary<string, object> values)
        {
            using (var cmd = new MySqlCommand(ScriptBuilder.GetInsertCommand(type, id, parentId, values), _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 10;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task ProvisionAsync(TypeInfo[] types)
        {
            // 1. Drop existing tables
            // 2. Create new tables

            var tables = Array.ConvertAll(types, Table.CreateTable);

            var tx = await _conn.BeginTransactionAsync();
            foreach (var table in tables)
            {
                using (var delCmd = new MySqlCommand(ScriptBuilder.CreateDeleteScript(table.Name), _conn, tx))
                {
                    delCmd.CommandType = CommandType.Text;
                    delCmd.CommandTimeout = 10;
                    await delCmd.ExecuteNonQueryAsync();
                }
                using (var createCmd = new MySqlCommand(ScriptBuilder.GetCreateTableScript(table), _conn, tx))
                {
                    createCmd.CommandType = CommandType.Text;
                    createCmd.CommandTimeout = 10;
                    await createCmd.ExecuteNonQueryAsync();
                }
            }
            tx.Commit();
        }

        #endregion

        #region Private Methods
        private async Task Initialize()
        {
            _conn = new MySqlConnection(_connectionString);
            await _conn.OpenAsync();
        }

        private async Task DeInitialize()
        {
            await _conn.CloseAsync();
            _conn.Dispose();
        }
        #endregion
    }
}

