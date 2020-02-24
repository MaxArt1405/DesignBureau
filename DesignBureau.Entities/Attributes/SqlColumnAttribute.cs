using System;
using System.Collections.Generic;
using System.Text;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Attributes
{
    public class SqlColumnAttribute : Attribute
    {
        public SqlColumnAttribute(string columnName)
        {
            ColumnName = columnName;
            IsPrimaryKey = false;
            SqlField = SqlFields.None;
            Type = SqlColumnTypes.Unknown;
        }
        public int DataLength { get; set; }
        public SqlFields SqlField { get; set; }
        public string ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsAutoIncrement { get; set; }
        public string SeqName { get; set; }
        public SqlColumnTypes Type { get; set; }
    }
}
