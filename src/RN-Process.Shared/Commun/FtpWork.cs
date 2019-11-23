using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinSCP;
using EnumerationOptions = WinSCP.EnumerationOptions;

namespace RN_Process.Shared.Commun
{
    public class FtpWork
    {
        private static string _lastFileName;


        public SessionOptions SessionOptions(string hostName, string userName, string password, string fingerprint)
        {
            Guard.Against.NullOrWhiteSpace(hostName, nameof(hostName));
            Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
            Guard.Against.NullOrWhiteSpace(password, nameof(password));
            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = hostName,
                UserName = userName,
                Password = password,
                SshHostKeyFingerprint = fingerprint
            };

            return sessionOptions;
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        public void DowloadFilesAndDirectories(SessionOptions options, string remotePath, string localPath)
        {

            using var session = new Session();
            // Connect
            session.Open(options);

            var fileInfos = GetFilesAndDirectoriesRemoteDir(options, remotePath);
            foreach (var fileInfo in fileInfos)
            {
                var localFilePath =
                    RemotePath.TranslateRemotePathToLocal(
                        fileInfo.FullName, remotePath, localPath);

                if (fileInfo.IsDirectory)
                {
                    // Create local subdirectory, if it does not exist yet
                    if (!Directory.Exists(localFilePath)) Directory.CreateDirectory(localFilePath);
                }
                else
                {
                    Console.WriteLine("Downloading file {0}...", fileInfo.FullName);
                    // Download file
                    var remoteFilePath = RemotePath.EscapeFileMask(fileInfo.FullName);

                    var transferResult =
                        session.GetFiles(remoteFilePath, localFilePath);

                    // Did the download succeeded?
                    if (!transferResult.IsSuccess)
                        // Print error (but continue with other files)
                        Console.WriteLine(
                            "Error downloading file {0}: {1}",
                            fileInfo.FullName, transferResult.Failures[0].Message);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="remotePath">"/home/user"</param>
        /// <returns></returns>
        public IEnumerable<RemoteFileInfo> GetFilesAndDirectoriesRemoteDir(SessionOptions options, string remotePath)
        {

            using var session = new Session();
            // Connect
            session.Open(options);

            // Enumerate files and directories to download
            var fileInfos =
                session.EnumerateRemoteFiles(
                    remotePath, null,
                    EnumerationOptions.EnumerateDirectories |
                    EnumerationOptions.AllDirectories);
            return fileInfos;
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="remotePath">"/home/user/"</param>
        /// <param name="localPathDir">@"d:\download\"</param>
        public void GetAllFilesRemoteDir(SessionOptions options, string remotePath, string localPathDir)
        {

            using var session = new Session();
            // Connect
            session.Open(options);

            // Download files
            var transferOptions = new TransferOptions { TransferMode = TransferMode.Binary };
            try
            {
                var transferResult = session.GetFiles(remotePath + "*", localPathDir, false, transferOptions);

                // Throw on any error
                transferResult.Check();
            }
            finally
            {
                // Terminate line after the last file (if any)
                if (_lastFileName != null) Console.WriteLine();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="localPath">@"d:\www"</param>
        /// <param name="remoteDir">"/home/martin/public_html"</param>
        /// <returns></returns>

        public void SynchronizationLocalAndRemoteDir(SessionOptions options, string localPath, string remoteDir)
        {
            Synchronization(options, localPath, remoteDir);
        }

        private SynchronizationResult Synchronization(SessionOptions options, string localPath, string remoteDir)
        {
            using var session = new Session();
            session.FileTransferred += FileTransferred;

            // Connect
            session.Open(options);

            // Synchronize files
            var synchronizationResult = session.SynchronizeDirectories(
                SynchronizationMode.Remote, localPath, remoteDir, false);

            // Throw on any error
            synchronizationResult.Check();

            return synchronizationResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="fromLocalDir">@"d:\toupload\*"</param>
        /// <param name="toRemoteDir">/home/user/</param>
        public void UploadFileToRemoteDir(SessionOptions options, string fromLocalDir, string toRemoteDir)
        {
            using var session = new Session();
            // Connect
            session.Open(options);

            // Upload files
            var transferOptions = new TransferOptions { TransferMode = TransferMode.Binary };

            var transferResult = session.PutFiles(fromLocalDir, toRemoteDir, false, transferOptions);
            // Throw on any error
            transferResult.Check();
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="fileName">"/home/user/file.name.txt"</param>
        /// <param name="localPath"></param>
        public void DownloadFileRemoteDir(SessionOptions options, string fileName, string localPath)
        {
            using var session = new Session();
            // Connect
            session.Open(options);

            // Download the selected file
            //check // Throw on any error
            session.GetFiles(RemotePath.EscapeFileMask(fileName), localPath).Check();
        }

        public RemoteFileInfo GetLastFileRemoteFileInfo(RemoteDirectoryInfo directoryInfo)
        {

            // Select the most recent file
            var latest =
                directoryInfo.Files
                    .Where(file => !file.IsDirectory)
                    .OrderByDescending(file => file.LastWriteTime)
                    .FirstOrDefault();

            // Any file at all?
            if (latest == null) throw new Exception("No file found");

            return latest;
        }

        /// <summary>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="remotePath">/home/martin/public_html</param>
        /// <returns></returns>
        public RemoteDirectoryInfo GetRemoteDirectoryInfo(SessionOptions options, string remotePath)
        {
            using var session = new Session();
            // Connect
            session.Open(options);

            // Get list of files in the directory
            var directoryInfo = session.ListDirectory(remotePath);
            return directoryInfo;
        }

        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            // New line for every new file
            if (_lastFileName != null && _lastFileName != e.FileName) Console.WriteLine();

            // Print transfer progress
            Console.Write("\r{0} ({1:P0})", e.FileName, e.FileProgress);

            // Remember a name of the last file reported
            _lastFileName = e.FileName;
        }

        /// <summary>
        /// status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FileTransferred(object sender, TransferEventArgs e)
        {
            if (e.Error == null)
            {
                Console.WriteLine("Upload of {0} succeeded", e.FileName);
            }
            else
            {
                Console.WriteLine("Upload of {0} failed: {1}", e.FileName, e.Error);
            }

            if (e.Chmod != null)
            {
                if (e.Chmod.Error == null)
                {
                    Console.WriteLine(
                        "Permissions of {0} set to {1}", e.Chmod.FileName, e.Chmod.FilePermissions);
                }
                else
                {
                    Console.WriteLine(
                        "Setting permissions of {0} failed: {1}", e.Chmod.FileName, e.Chmod.Error);
                }
            }
            else
            {
                Console.WriteLine("Permissions of {0} kept with their defaults", e.Destination);
            }

            if (e.Touch != null)
            {
                if (e.Touch.Error == null)
                {
                    Console.WriteLine(
                        "Timestamp of {0} set to {1}", e.Touch.FileName, e.Touch.LastWriteTime);
                }
                else
                {
                    Console.WriteLine(
                        "Setting timestamp of {0} failed: {1}", e.Touch.FileName, e.Touch.Error);
                }
            }
            else
            {
                // This should never happen during "local to remote" synchronization
                Console.WriteLine(
                    "Timestamp of {0} kept with its default (current time)", e.Destination);
            }
        }

    }
}