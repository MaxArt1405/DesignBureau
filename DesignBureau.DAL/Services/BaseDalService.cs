using System;
using System.Collections.Generic;
using DesignBureau.DAL.Repositories;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Interfaces;

namespace DesignBureau.DAL.Services
{
    public class BaseDalService<T> : IDisposable where T : SqlEntity, ISqlEntity, IBaseEntity, new()
    {
        protected readonly IRepository<T> Repository;

        public BaseDalService()
        {
            Repository = new BaseDbRepository<T>();
        }

        public IEnumerable<T> GetAll()
        {
            var data = Repository.GetAll();
            return data ?? new List<T>();
        }

        public IEnumerable<T> GetAll(SelectionInfo selection)
        {
            var data = Repository.GetAll(selection);
            return data ?? new List<T>();
        }

        public T GetItem(int code)
        {
            var item = Repository.GetItem(code);
            return item;
        }

        public T Add(T entity)
        {
            var result = Repository.Add(entity);
            return result;
        }

        public T Update(T entity)
        {
            var result = Repository.Update(entity, null);
            return result;
        }

        public T Update(T entity, List<int> columns = null)
        {
            var result = Repository.Update(entity, columns);
            return result;
        }

        public bool Delete(T entity)
        {
            var result = Repository.Delete(entity);
            return result;
        }

        public T GetEmptyObject()
        {
            var emptyObj = new T();
            return emptyObj;
        }

        public bool ValidTable()
        {
            var result = Repository.IsValid();
            return result;
        }

        public string GetTableName()
        {
            var tableName = new T().TableName;
            return tableName;
        }

        public int GetSeqValue(string seqName)
        {
            var seq = Repository.GetSeqValue(seqName);
            return seq;
        }

        public void Dispose() { }
    }
}
