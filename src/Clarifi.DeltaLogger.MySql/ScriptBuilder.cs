using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clarifi.DeltaLogger.Internal;

namespace Clarifi.DeltaLogger.MySql
{
    internal static class ScriptBuilder
    {
        public static string CreateDeleteScript(string table)
        {
            return $"DROP TABLE IF EXISTS `{table}`;";
        }

        public static string GetCreateTableScript(Table table)
        {
            var buffer = new StringBuilder();
            // Create table
            buffer.AppendLine($"CREATE TABLE `{table.Name}` (");
            // Add columns
            table.Columns.ForEach(c => buffer.AppendLine($"`{c.Name}` {_typeMap[c.ColumnType]},"));
            // Add primary key
            buffer.AppendLine($"PRIMARY KEY(`{table.PkColumn}`),");
            // All indexes
            for (int i = 0; i < table.IndexedColumns.Count; i++)
            {
                buffer.Append($"KEY `fk_{table.IndexedColumns[i]}` (`{table.IndexedColumns[i]}`)");
                var isLast = i == table.IndexedColumns.Count - 1;
                if (isLast == false)
                    buffer.AppendLine(",");
                else
                    buffer.AppendLine(" )");
            }
            buffer.AppendLine("DEFAULT CHARSET = utf8;");
            return buffer.ToString();
        }

        public static string GetInsertCommand(TypeInfo type, string id, string parentId, IDictionary<string, object> values)
        {
            var table = Table.CreateTable(type);
            var simpleFields = type.GetSimpleFields()
                                   .Where(f => values.ContainsKey(f.Name))
                                   .ToList();
            var buffer = new StringBuilder();
            buffer.Append($"INSERT INTO `{type.Name}` ( `id`, `parent_id`, `utc_create_date`");
            simpleFields.ForEach(f => buffer.Append($", `{f.Name}`"));
            var parentIdTxt = parentId == null ? "NULL" : $"\"{parentId}\"";
            buffer
               .AppendLine(" )")
               .AppendLine("VALUES")
               .Append($"( \"{id}\", {parentIdTxt}, \"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}\"");
            foreach( var field in simpleFields )
            {
                var col = table.Columns.Find(c => c.Name.Equals(field.Name));
                buffer.Append(", ").Append(col.FormatValue(values[field.Name]));
            }
            buffer.Append(");");
            return buffer.ToString();
        }

        private static readonly Dictionary<ColumnType, string> _typeMap = new Dictionary<ColumnType, string>
        {
            { ColumnType.Boolean, "BOOLEAN"},
            { ColumnType.DateTime, "DATETIME"},
            { ColumnType.Decimal, "DECIMAL(10,2)"},
            { ColumnType.Id, "VARCHAR(100)"},
            { ColumnType.Integer, "INT" },
            { ColumnType.String, "VARCHAR(1024)"},
            { ColumnType.Text, "TEXT"},
        };


    }
}
