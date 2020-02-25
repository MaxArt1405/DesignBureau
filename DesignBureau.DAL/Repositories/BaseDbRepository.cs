using DesignBureau.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Entity.UnitOfWork;
using Dapper;
using Newtonsoft.Json;

namespace DesignBureau.DAL.Repositories
{
    internal class BaseDbRepository<T> : IRepository<T>, IDisposable where T : ISqlEntity, new()
    {
        public BaseDbRepository() { }

        public IEnumerable<T> GetAll()
        {
            var data = Select();
            return data;
        }

        public IEnumerable<T> GetAll(SelectionInfo selection)
        {
            var where = GetWhereString(selection);
            return Select(where);
        }

        public T GetItem(int code)
        {
            var selection = new SelectionInfo()
            {
                Codes = new List<int>() { code }
            };
            var where = GetWhereString(selection);
            var result = Select(where).FirstOrDefault();
            return result;
        }

        public T Add(T value)
        {
            var cols = value.CurrentColumns();
            var pkCol = cols.FirstOrDefault(x => x.IsPrimaryKey && !string.IsNullOrEmpty(x.SeqName));
            if (pkCol != null)
            {
                var seq = GetSeqValue(pkCol.SeqName);
                value.Id = seq;
            }

            IUnitOfWork localUnitOfWork = null;
            try
            {
                if (!UnitOfWorkFactory.IsActive)
                    localUnitOfWork = UnitOfWorkFactory.Start();
                var query = value.SqlInsert();
                UnitOfWorkFactory.Current.Connection.Execute(query, value);
            }
            finally
            {
                if (localUnitOfWork != null)
                {
                    UnitOfWorkFactory.Dispose();
                }
            }

            return value;
        }

        public T Update(T value)
        {
            var result = Update(value, new List<int>());
            return result;
        }

        public T Update(T value, List<int> fieldsForUpdate)
        {
            IUnitOfWork localUnitOfWork = null;
            try
            {
                if (!UnitOfWorkFactory.IsActive)
                    localUnitOfWork = UnitOfWorkFactory.Start();
                var query = value.SqlUpdate(fieldsForUpdate);
                UnitOfWorkFactory.Current.Connection.Execute(query, value);
            }
            finally
            {
                if (localUnitOfWork != null)
                {
                    UnitOfWorkFactory.Dispose();
                }
            }
            return value;
        }

        public bool Delete(T value)
        {
            IUnitOfWork localUnitOfWork = null;
            try
            {
                if (!UnitOfWorkFactory.IsActive)
                    localUnitOfWork = UnitOfWorkFactory.Start();
                var query = value.SqlDelete();
                UnitOfWorkFactory.Current.Connection.Execute(query, value);

                if (GetItem(value.Id) == null)
                {
                    return true;
                }
            }
            finally
            {
                if (localUnitOfWork != null)
                {
                    UnitOfWorkFactory.Dispose();
                }
            }

            return false;
        }

        public bool IsValid()
        {
            var entity = new T();
            Add(entity);
            var result = Delete(entity);
            return result;
        }
        public string GetWhereString(SelectionInfo selection)
        {
            var whereList = WhereAndBySelection(selection);
            var where = string.Empty;
            if (whereList != null && whereList.Any())
            {
                where = string.Join(" AND", whereList.Select(x => $"({x})"));
            }
            return where;
        }

        private static IEnumerable<T> Select(string where = "", bool distinct = false)
        {
            var entity = new T();
            var query = entity.SqlSelect(null, distinct);
            if (!string.IsNullOrEmpty(where))
                query += " where " + where;
            IUnitOfWork localUnitOfWork = null;
            List<T> result;
            try
            {
                if (!UnitOfWorkFactory.IsActive)
                    localUnitOfWork = UnitOfWorkFactory.Start();
                result = UnitOfWorkFactory.Current.Connection.Query<T>(query).ToList();
            }
            finally
            {
                if (localUnitOfWork != null)
                {
                    UnitOfWorkFactory.Dispose();
                }
            }
            return result;
        }

        private List<string> WhereAndBySelection(SelectionInfo selection)
        {
            var result = new List<string>();

            if (selection == null)
                return result;

            if (selection.IntOptions == null)
                selection.IntOptions = new List<KeyValuePair<int, int>>();

            if (selection.StringOptions == null)
                selection.StringOptions = new List<KeyValuePair<int, string>>();

            var obj = new T();
            var columns = obj.GetSqlColumns();
            var codeColumnId = obj.GetPrimaryKeyColumnId();

            if (selection.Codes != null && selection.Codes.Any() && codeColumnId != 0)
            {
                var where = string.Empty;

                if (columns.TryGetValue(codeColumnId, out var colName))
                {
                    if (!string.IsNullOrEmpty(colName))
                    {
                        var codesList = string.Join(", ", selection.Codes);
                        where = selection.Codes.Count > 1
                            ? $"{colName} IN ({codesList})"
                            : $"{colName} = {selection.Codes[0]}";
                    }
                }

                if (!string.IsNullOrEmpty(where))
                    result.Add(where);
            }

            if (selection.IntOptions != null)
            {
                foreach (var group in selection.IntOptions.GroupBy(x => x.Key))
                {
                    var where = string.Empty;
                    var intValues = group.Select(it => it.Value).ToList();

                    if (columns.TryGetValue(group.Key, out var colName))
                    {
                        var codesList = string.Join(", ", intValues);
                        where = intValues.Count > 1
                            ? $"{colName} IN ({codesList})"
                            : $"{colName} = {intValues[0]}";
                    }

                    if (!string.IsNullOrEmpty(where))
                        result.Add(where);
                }
            }

            if (selection.StringOptions != null)
            {
                foreach (var group in selection.StringOptions.GroupBy(x => x.Key))
                {
                    var where = string.Empty;
                    var strValues = group.Select(it => it.Value).ToList();

                    if (columns.TryGetValue(group.Key, out var colName))
                    {
                        colName = selection.CaseIgnore ? $"UPPER({colName})" : colName;
                        strValues = selection.CaseIgnore ? strValues.Select(x => x.ToUpper()).ToList() : strValues;
                        var codesList = string.Join(", ", strValues.Select(x => $"'{x}'"));
                        where = strValues.Count > 1
                            ? $"{colName} IN ({codesList})"
                            : $"{colName} = '{strValues[0]}'";
                    }

                    if (!string.IsNullOrEmpty(where))
                        result.Add(where);
                }
            }

            var list = result.Select(x => $"({x})").ToList();
            return list;
        }

        public int GetSeqValue(string seqName)
        {
            var result = 0;
            if (string.IsNullOrEmpty(seqName))
                return result;

            var query = $" SELECT NEXT VALUE FOR [Department].[dbo].{seqName} as 'Id'";
            IUnitOfWork localUnitOfWork = null;
            try
            {
                if (!UnitOfWorkFactory.IsActive)
                    localUnitOfWork = UnitOfWorkFactory.Start();
                var r = UnitOfWorkFactory.Current.Connection.Query<IdName>(query).FirstOrDefault();
                if (r != null)
                    result = r.Id;
            }
            finally
            {
                if (localUnitOfWork != null)
                {
                    UnitOfWorkFactory.Dispose();
                }
            }
            return result;
        }

        public void Dispose() { }
    }
    public class IdName
    {
        public IdName() { }

        public IdName(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

}
