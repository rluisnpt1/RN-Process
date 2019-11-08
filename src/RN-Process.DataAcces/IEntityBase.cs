using System;
using RN_Process.DataAcces;

namespace RN_Process.DataAccess
{
    public interface IEntityBase : IModifiableEntity
    {
        object Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }

        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        byte[] RowVersion { get; set; }
    }
}
