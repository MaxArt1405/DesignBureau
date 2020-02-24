using System.Collections.Generic;
using DesignBureau.Entities.Entity.BaseEntities;

namespace DesignBureau.Entities.Interfaces
{
    public interface IRepository<T> where T : new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(SelectionInfo selection);
        T GetItem(int code);
        T Add(T value);
        T Update(T value);
        T Update(T value, List<int> fields2Update);
        bool Delete(T value);
        bool IsValid();
        int GetSeqValue(string seqName);
    }
}
