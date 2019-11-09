using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RN_Process.Shared.Enums
{
    public enum StatusType
    {
        [Description("Success")]
        Success,

        [Description("Error On processing")]
        Error,

        [Description("Need Verification")]
        VerificationRequired,
    }
}
