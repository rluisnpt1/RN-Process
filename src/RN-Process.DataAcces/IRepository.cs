using System.Collections.Generic;

namespace RN_Process.DataAccess
{
    public interface IRepository<T, in TV> where T : IEntity<TV>
    {
        IList<T> GetAll();
        T GetById(TV id);
        void Save(T saveThis);
        void Delete(T deleteThis);
    }
}
