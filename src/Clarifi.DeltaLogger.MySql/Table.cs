using System.Collections.Generic;
using Clarifi.RoomMappingLogger.Internal;

namespace Clarifi.RoomMappingLogger.MySql
{
    internal class Table
    {
        public static Table CreateTable(TypeInfo type)
        {
            var table = new Table { Name = type.Name };
            table.Columns.Add(new Column { Name = "id", ColumnType = ColumnType.Id });
            table.Columns.Add(new Column { Name = "parent_id", ColumnType = ColumnType.Id});
            table.Columns.Add(new Column { Name = "utc_create_date", ColumnType = ColumnType.DateTime });
            var fields = type.GetSimpleFields();
            foreach (var field in fields)
            {
                var col = new Column { Name = field.Name, ColumnType = Column.ResolveColumnType(field.Type) };
                table.Columns.Add(col);
            }
            table.PkColumn = "id";
            table.IndexedColumns.Add("parent_id");
            return table;
        }

        public string Name { get; set; }

        public List<Column> Columns { get; } = new List<Column>();

        public string PkColumn { get; set; }

        public List<string> IndexedColumns { get; } = new List<string>();
    }
}
