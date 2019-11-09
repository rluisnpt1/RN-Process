using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RN_Process.Shared.Enums
{
    public enum RequisitionType
    {
        [Description("New development")]
        NewDevelopment,
        
        [Description("Modification")]
        Modification,

        [Description("Subscription")]
        Subscription,


        [Description("Verification")]
        Verification,
    }
}
