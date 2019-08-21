using System;
using System.Collections.Generic;
using System.Globalization;

namespace Clarifi.DeltaLogger.MySql
{ 
    internal class Column
    {
        private static readonly Dictionary<Type, ColumnType> _typeColMap = new Dictionary<Type, ColumnType>
        {
            { typeof(int), ColumnType.Integer },
            { typeof(long), ColumnType.Integer },
            { typeof(decimal), ColumnType.Decimal },
            { typeof(float), ColumnType.Decimal },
            { typeof(double), ColumnType.Decimal },
            { typeof(string), ColumnType.String },
            { typeof(bool), ColumnType.Boolean },
            { typeof(DateTime), ColumnType.DateTime }
        };

        public static ColumnType ResolveColumnType(Type type )
        {
            return _typeColMap[type];
        }

        public ColumnType ColumnType { get; set; }

        public string Name { get; set; }

        public string FormatValue(object value)
        {
            if (value == null)
                return "NULL";
            switch (ColumnType)
            {
                case ColumnType.Id:
                case ColumnType.String:
                case ColumnType.Text:
                    return $"\"{value.ToString()}\"";
                case ColumnType.DateTime:
                    return $"\"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}\"";
                case ColumnType.Boolean:
                case ColumnType.Integer:
                case ColumnType.Decimal:
                default:
                    return value.ToString();
            }
        }
    }
}