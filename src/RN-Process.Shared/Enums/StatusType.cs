using System.ComponentModel;

namespace RN_Process.Shared.Enums
{
    public enum StatusType
    {
        [Description("Success")] Success,

        [Description("Error On processing")] Error,

        [Description("Need Verification")] VerificationRequired,

        [Description("Processed")] Processed
    }
}