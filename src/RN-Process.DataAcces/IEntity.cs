

namespace RN_Process.DataAccess
{
    public interface IEntity<T>
    {
       new T Id { get; set; }
    }
}
