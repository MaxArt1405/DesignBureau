using System;
using System.Data;
using System.Data.SqlClient;
using DesignBureau.Entities.Interfaces;

namespace DesignBureau.Entities.Entity.UnitOfWork
{
    public class UnitOfWorkFactory
    {
        [ThreadStatic]
        private static IUnitOfWork _current;

        public static bool IsActive => _current != null;

        public static IUnitOfWork Current
        {
            get
            {
                var unitOfWork = _current;
                if (unitOfWork == null)
                    throw new InvalidOperationException("You are not in a unit of work");
                return unitOfWork;
            }
        }

        public static IUnitOfWork Start()
        {
            if (_current != null)
                throw new InvalidOperationException("You cannot start more than one unit of work at the same time.");
            var connectionString = $"";
            var connection = GetConnection(connectionString);
            connection.Open();
            _current = new UnitOfWork(connection);
            return _current;
        }

        private static IDbConnection GetConnection(string connectionString) => new SqlConnection(connectionString);

        public static void Dispose()
        {
            _current?.Dispose();
            _current = null;
        }
    }
}
