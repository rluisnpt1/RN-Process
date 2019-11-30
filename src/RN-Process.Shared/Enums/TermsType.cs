using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RN_Process.Shared.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TermsType
    {
        [Description("Credit Card")] CreditCards,
        [Description("Leasing")] Leasing,
        [Description("Loan")] Loan
    }
}