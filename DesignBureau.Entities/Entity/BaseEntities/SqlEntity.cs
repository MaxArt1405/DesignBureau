using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignBureau.Entities.Attributes;
using DesignBureau.Entities.Enums;
using DesignBureau.Entities.Interfaces;
using Newtonsoft.Json;

namespace DesignBureau.Entities.Entity.BaseEntities
{
    public class SqlEntity : BaseEntity, ISqlEntity
    {
        public static string BindSymbol => "@";
        private static readonly ConcurrentDictionary<Type, List<SqlColumnEntity>> AllColumns = new ConcurrentDictionary<Type, List<SqlColumnEntity>>();

        [JsonIgnore]
        public virtual SqlFields ObjectField => SqlFields.None;

        [JsonIgnore]
        public virtual SqlFields CodeField => SqlFields.None;

        [JsonIgnore]
        public virtual string TableName4Query => $"[DbName].[dbo].{TableName}";//insert Db name

        [JsonIgnore]
        public virtual string TableName
        {
            get
            {
                var tableAttributes = GetType().GetCustomAttributes(typeof(TableAttribute), true);
                return tableAttributes.Length > 0 ? ((TableAttribute)tableAttributes[0]).Name : "";
            }
        }

        public List<SqlColumnEntity> CurrentColumns()
        {
            var type = GetType();
            return AllColumns.ContainsKey(type) ? AllColumns[type] : null;
        }
        public SqlEntity()
        {
            var type = GetType();

            if (AllColumns.ContainsKey(type))
                return;
            CreateEntityMap();
        }

        public int GetPrimaryKeyColumnId()
        {
            var col = CurrentColumns()?.FirstOrDefault(x => x.IsPrimaryKey);
            return col?.SqlFieldId ?? 0;
        }

        public virtual string SqlSelect(List<int> selectedColumns = null, bool distinct = false)
        {
            var stringBuilder = new StringBuilder();
            var fromSql = new StringBuilder();
            stringBuilder.Append("SELECT ");

            if (distinct)
            {
                stringBuilder.Append("distinct ");
            }

            fromSql.Append($" from {TableName4Query}");
            var allColumns = CurrentColumns().ToList();
            var columnList = allColumns.Select(x => x.ColumnName4Select()).ToList();

            if (selectedColumns != null && selectedColumns.Any())
            {
                columnList = (from col in allColumns where selectedColumns.Contains(col.SqlFieldId) select col.ColumnName4Select()).ToList();
            }

            var fieldsSql = string.Join(", ", columnList);
            stringBuilder.Append(fieldsSql);
            stringBuilder.Append(fromSql);
            return stringBuilder.ToString();
        }

        private void CreateEntityMap()
        {
            var columns = new List<SqlColumnEntity>();
            var type = GetType();
            foreach (var propertyInfo in type.GetProperties())
            {
                var columnAttributes = propertyInfo.GetCustomAttributes(typeof(SqlColumnAttribute), true);
                if (columnAttributes.Length != 1) continue;
                if (!(columnAttributes[0] is SqlColumnAttribute columnAttribute)) continue;
                var col = new SqlColumnEntity
                {
                    IsPrimaryKey = columnAttribute.IsPrimaryKey,
                    PropertyName = propertyInfo.Name,
                    ColumnType = propertyInfo.PropertyType,
                    ColumnName = columnAttribute.ColumnName.ToUpper(),
                    SqlFieldId = (int)columnAttribute.SqlField,
                    SeqName = columnAttribute.SeqName
                };
                columns.Add(col);
            }
            if (!AllColumns.ContainsKey(type))
                AllColumns.TryAdd(type, columns);
        }

        public virtual string SqlInsert()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO {TableName4Query} ");

            var fieldsList = new List<string>();
            var valuesList = new List<string>();
            var columns = CurrentColumns().ToList();
            foreach (var sqlColumn in columns)
            {
                fieldsList.Add(sqlColumn.ColumnName4Insert());
                valuesList.Add($"{BindSymbol}{sqlColumn.PropertyName}");
            }
            var fields = $"({string.Join(", ", fieldsList)})";
            var values = $" values ({string.Join(", ", valuesList)})";

            stringBuilder.Append(fields);
            stringBuilder.Append(values);
            return stringBuilder.ToString();
        }

        public Dictionary<int, string> GetSqlColumns()
        {
            var cols = CurrentColumns();
            if (cols == null)
                return null;
            var res = new Dictionary<int, string>();
            foreach (var sqlColumn in cols)
            {
                if (sqlColumn.SqlFieldId == (int)SqlFields.None) continue;
                if (!res.ContainsKey(sqlColumn.SqlFieldId))
                    res.Add(sqlColumn.SqlFieldId, sqlColumn.ColumnName4Update());
            }
            return res;
        }

        public virtual string SqlUpdate(List<int> columns)
        {
            if (columns == null || !columns.Any())
            {
                var cols = CurrentColumns().Where(x => !x.IsPrimaryKey)?.ToList();
                columns = cols.Select(x => x.SqlFieldId).ToList();
            }
            var stringBuilder = new StringBuilder();
            var fieldsList = new List<string>();
            var whereSql = new StringBuilder(" WHERE ");
            stringBuilder.Append($"UPDATE {TableName4Query} SET ");
            foreach (var sqlColumn in CurrentColumns().Where(x =>  columns.Contains(x.SqlFieldId)))
            {
                var columnName = sqlColumn.ColumnName4Update();
                if (string.IsNullOrEmpty(columnName))
                    continue;
                if (!sqlColumn.IsPrimaryKey)
                {
                    fieldsList.Add($"{columnName}={BindSymbol}{sqlColumn.PropertyName}");
                }
            }
            foreach (var sqlColumn in CurrentColumns().Where(x => x.IsPrimaryKey))
            {

                var columnName = sqlColumn.ColumnName4Insert();
                whereSql.Append($"{columnName}={BindSymbol}{sqlColumn.PropertyName}");
            }
            var fieldsSql = string.Join(", ", fieldsList);
            stringBuilder.Append(fieldsSql);
            stringBuilder.Append(whereSql);

            return stringBuilder.ToString();
        }

        public string SqlDelete(bool customWhere = false)
        {
            var stringBuilder = new StringBuilder();
            var fieldsSql = new StringBuilder("");
            var whereSql = new StringBuilder(" WHERE ");
            stringBuilder.Append($"DELETE FROM {TableName4Query} ");

            if (!customWhere)
            {
                var pkColumns = CurrentColumns().Where(x => x.IsPrimaryKey)?.ToList();
                if (pkColumns.Count == 1)
                {
                    var pkColumn = pkColumns.First();
                    whereSql.Append($"{pkColumn.ColumnName4Update()}={BindSymbol}{pkColumn.PropertyName}");
                }
                stringBuilder.Append(fieldsSql);
                stringBuilder.Append(whereSql);
            }

            return stringBuilder.ToString();
        }
    }
}
