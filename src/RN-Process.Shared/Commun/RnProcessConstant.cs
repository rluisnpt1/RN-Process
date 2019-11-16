using System;
using System.Collections.Generic;
using System.Text;

namespace RN_Process.Shared.Commun
{
    public static class RnProcessConstant
    {
        public const string Open = "Open";
        public const string Complete = "Complete";

        public const string ColumnsBaseIntrum = "intrum_Column1";
        public const string ColumnsBaseClient = "client_Column1";
        public const string MsgCanNotSaveDuplicateTask = "Cannot save duplicate Tasks.";
        

        public static readonly string BaseTestWorkFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string BaseWorkFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}