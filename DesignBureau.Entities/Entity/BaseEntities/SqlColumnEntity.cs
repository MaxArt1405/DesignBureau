using System;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Entity.BaseEntities
{
    public class SqlColumnEntity
    {
        public SqlColumnEntity()
        {
            IsPrimaryKey = false;
            SqlFieldId = (int)SqlFields.None;
        }
        public bool IsPrimaryKey { get; set; }
        public int SqlFieldId { get; set; }
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public string SeqName { get; set; }

        public string ColumnName4Select(bool withAs = true)
        {
            var column = ColumnName;
            if (ColumnType == typeof(double))
            {
                column = $"CAST({column} AS DECIMAL(38,18))";
            }

            var columnName = ColumnType == typeof(double) ? column : $"[{column}]";
            return withAs ? $"{columnName} as {PropertyName}" : $"{columnName}";
        }

        public string ColumnName4Update() => $"[{ColumnName}]";

        public string ColumnName4Insert() => $"[{ColumnName}]";
    }
}
