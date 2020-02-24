using System;
using System.Data;
using DesignBureau.Entities.Interfaces;

namespace DesignBureau.Entities.Entity.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        IDbTransaction _transaction;
        private readonly Guid _id;

        IDbConnection IUnitOfWork.Connection => _connection;

        IDbTransaction IUnitOfWork.Transaction => _transaction;

        Guid IUnitOfWork.Id => _id;

        internal UnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            _connection = connection;
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            if (_connection == null) return;
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Dispose();
        }
    }
}
