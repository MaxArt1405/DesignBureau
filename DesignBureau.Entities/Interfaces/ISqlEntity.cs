using System.Collections.Generic;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Interfaces
{
    public interface ISqlEntity : IBaseEntity
    {
        SqlFields ObjectField { get; }
        SqlFields CodeField { get; }
        string TableName { get; }
        string TableName4Query { get; }
        string SqlInsert();
        string SqlSelect(List<int> selectedColumns = null, bool distinct = false);
        List<SqlColumnEntity> CurrentColumns();
        Dictionary<int, string> GetSqlColumns();
        int GetPrimaryKeyColumnId();
        string SqlUpdate(List<int> columns);
        string SqlDelete(bool customWhere = false);
    }
}
