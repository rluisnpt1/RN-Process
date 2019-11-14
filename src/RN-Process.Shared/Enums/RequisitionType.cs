using System.ComponentModel;

namespace RN_Process.Shared.Enums
{
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