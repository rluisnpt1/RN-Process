using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RN_Process.Shared.Enums
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileAccessType
    {
        [Description("FTP")] FTP,
        [Description("EMAIL")] Email,
        [Description("WEBSERVICE")] WebServer,
        [Description("API")] API,
        [Description("WEB SITE")] WebSite,
        [Description("DATA BASE")] DataBase,
        [Description("REMOTE DESKTOP")] RemoteDesktop,
        [Description("ACTIVE DIRECTORY")] ActiveDirectory,
        [Description("LOCAL MACHINE")] LocalMachine
    }
}