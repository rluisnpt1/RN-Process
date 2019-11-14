namespace RN_Process.DataAccess.FTP
{

    public class FtpClientCredencial
    {
        public FtpClientCredencial(string folder, string name, string host, string port, string user, string pass, string comments, string remoteDir, string localDir)
        {
            Folder = folder;
            Name = name;
            Port = port;
            Host = host;
            User = user;
            Pass = pass;
            Comments = comments;
            RemoteDir = remoteDir;
            LocalDir = localDir;
        }

        public string Folder { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Comments { get; set; }
        public string RemoteDir { get; set; }
        public string LocalDir { get; set; }
    }
}
