namespace RN_Process.DataAccess
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}