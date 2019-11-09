

namespace RN_Process.DataAccess
{
    public interface IEntity<out TV> : IEntityBase
    {
        new TV Id { get; }
    }
}
