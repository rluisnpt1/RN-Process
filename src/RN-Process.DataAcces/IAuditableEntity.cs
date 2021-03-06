﻿using System;

namespace RN_Process.DataAccess
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime? UpdatedDate { get; set; }

        string ModifiedBy { get; set; }
    }
}