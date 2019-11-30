using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RN_Process.Shared.Enums
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusType
    {
        [Description("Success")] Success,

        [Description("Error On processing")] Error,

        [Description("Need Verification")] VerificationRequired,

        [Description("Processed")] Processed
    }
}