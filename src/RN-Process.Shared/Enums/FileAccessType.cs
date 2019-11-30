using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RN_Process.Shared.Enums
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequisitionType
    {
        [Description("New development")] NewDevelopment,

        [Description("Modification")] Modification,

        [Description("Subscription")] Subscription,


        [Description("Verification")] Verification
    }
}