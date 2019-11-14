using System.Net;

namespace RN_Process.DataAccess.FTP
{
    public interface IFtpClient
    {
        FtpWebRequest CreateCredential(string directoryOrfileName);
        void CreateDirectory(string newDirectory);
        void Delete(string deleteFile);
        void DeleteDir(string deleteFile);
        string[] DirectoryListDetailed(string directory);
        string[] DirectoryListSimple(string directory);
        string[] GetFileList(string directory);
        void UploadFile(string remoteFile, string localFile);

        void DownloadFiles(string remoteFile, string localFile);
        void DownloadFtpDirectory(string remoteFtpDirUrl, string targetDirt);
        string GetFileCreatedDateTime(string fileName);
        string GetFileSize(string fileName);
        void Rename(string currentFileNameAndPath, string newFileName);
    }
}