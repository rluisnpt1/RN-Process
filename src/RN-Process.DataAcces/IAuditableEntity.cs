using System;
using System.Collections.Generic;
using System.Text;

namespace RN_Process.DataAccess
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime? ModifiedDate { get; set; }

        string ModifiedBy { get; set; }
    }
}
