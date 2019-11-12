using System.ComponentModel;

namespace RN_Process.Shared.Enums
{
    public enum RequisitionType
    {
        [Description("New development")] NewDevelopment,

        [Description("Modification")] Modification,

        [Description("Subscription")] Subscription,


        [Description("Verification")] Verification
    }
}