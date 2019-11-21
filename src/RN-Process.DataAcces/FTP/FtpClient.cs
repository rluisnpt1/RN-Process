using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RN_Process.DataAccess.FTP
{
    public class FtpClient : IFtpClient
    {
        private readonly Queue<string> _downloadlist;
        private readonly NetworkCredential _networkCredential;
        private readonly object _sync = new object();
        private readonly int bufferSize = 2048;
        private FtpWebRequest ftpRequest;
        private FtpWebResponse ftpResponse;
        private Stream ftpStream;
        private string host;


        /// <summary>
        ///     Construct Object
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="hostIP"></param>
        public FtpClient(string userName, string password, string hostIP, string Port = null)
        {
            SetHostIP(hostIP, Port);
            _downloadlist = new Queue<string>();
            _networkCredential = new NetworkCredential(userName, password);
        }

        ///// <summary>
        /// Download File
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        public void DownloadFiles(string pathTofileToGetfile, string localTosaveData, string filename)
        {
            if (pathTofileToGetfile.StartsWith("/")) pathTofileToGetfile = pathTofileToGetfile.Remove(0, 1);
            //if (localTosaveData.EndsWith("\\"))
            //{
            //    localTosaveData = localTosaveData.Remove(localTosaveData.Length - 1);
            //}
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + pathTofileToGetfile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = _networkCredential;
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();

                /* Open a File Stream to Write the Downloaded File */
                //where to save data
                var localFileStream = new FileStream(localTosaveData + filename, FileMode.Create);
                /* Buffer for the Downloaded Data */
                var byteBuffer = new byte[bufferSize];
                var bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                while (bytesRead > 0)
                {
                    localFileStream.Write(byteBuffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                }

                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }

        /// <summary>
        ///     Upload File
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        public void UploadFile(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = _networkCredential;
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                //ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                var localFileStream = new FileStream(localFile, FileMode.OpenOrCreate);

                using (var ftpstream = ftpRequest.GetRequestStream())
                {
                    var buffer = new byte[localFileStream.Length];
                    localFileStream.Read(buffer, 0, buffer.Length);
                    localFileStream.Close();
                    ftpstream.Write(buffer, 0, buffer.Length);
                    ftpstream.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }


        /// <summary>
        ///     Delete File
        /// </summary>
        /// <param name="deleteFile"></param>
        public void Delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                /* Specify the Type of FTP Request */
                CreateFTPRequest(deleteFile, WebRequestMethods.Ftp.DeleteFile);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }


        /// <summary>
        ///     Delete Dir
        /// </summary>
        /// <param name="deleteFile"></param>
        public void DeleteDir(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                /* Specify the Type of FTP Request */
                CreateFTPRequest(deleteFile, WebRequestMethods.Ftp.RemoveDirectory);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }


        /// <summary>
        ///     Rename File
        /// </summary>
        /// <param name="currentFileNameAndPath"></param>
        /// <param name="newFileName"></param>
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Specify the Type of FTP Request */
                /* Create an FTP Request */
                CreateFTPRequest(currentFileNameAndPath, WebRequestMethods.Ftp.Rename);

                /* Rename the File */
                ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }

        /// <summary>
        ///     Create a New Directory on the FTP Server
        /// </summary>
        /// <param name="newDirectory"></param>
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(newDirectory, WebRequestMethods.Ftp.MakeDirectory);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }


        /// <summary>
        ///     Get the Date/Time a File was Created
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(fileName, WebRequestMethods.Ftp.GetDateTimestamp);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();

                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);

                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */

                fileInfo = ftpReader.ReadToEnd();

                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error FTP: {host} ," +
                                                  $" User: {_networkCredential.UserName}, Msg: ") + ex.Message);
            }
        }

        /// <summary>
        ///     Get the Size of a File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileSize(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(fileName, WebRequestMethods.Ftp.GetFileSize);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();

                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);

                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */

                while (ftpReader.Peek() != -1) fileInfo = ftpReader.ReadToEnd();

                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format($"Error FTP: {host} ," + $" User: {_networkCredential.UserName}, Msg: ") +
                    ex.Message);
            }
        }


        /// <summary>
        ///     List Directory Contents File/Folder Name Only
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(directory, WebRequestMethods.Ftp.ListDirectory);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                var directoryRaw = "";

                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */

                while (ftpReader.Peek() != -1) directoryRaw += ftpReader.ReadLine() + "|";

                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */

                var directoryList = directoryRaw.Split("|".ToCharArray());
                return directoryList;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format($"Error FTP: {host} ," + $" User: {_networkCredential.UserName}, Msg: ") +
                    ex.Message);
            }
        }


        /// List Directory Contents in Detail (Name, Size, Created, etc.)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(directory, WebRequestMethods.Ftp.ListDirectoryDetails);

                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                var ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try
                {
                    while (ftpReader.Peek() != -1) directoryRaw += ftpReader.ReadLine() + "|";
                }
                catch (Exception ex)
                {
                    ExceptionMessage(ex);
                }

                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try
                {
                    var directoryList = directoryRaw.Split("|".ToCharArray());
                    return directoryList;
                }
                catch (Exception ex)
                {
                    ExceptionMessage(ex);
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }

            /* Return an Empty string Array if an Exception Occurs */
            return new[] {""};
        }

        public FtpWebRequest CreateCredential(string directoryOrfileName)
        {
            ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + directoryOrfileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = _networkCredential;
            return ftpRequest;
        }

        public string[] GetFileList(string directory)
        {
            string[] downloadFiles;
            var result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                /* Create an FTP Request */
                CreateFTPRequest(directory, WebRequestMethods.Ftp.ListDirectory);

                response = ftpRequest.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                var line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error FTP: {host} ," +
                                                  $" User: {_networkCredential.UserName}, Msg: ") + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (response != null) response.Close();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="remoteFtpDirUrl">FTP:192.168.1.1/myfolder</param>
        /// <param name="targetDirt">c:temp</param>
        public void DownloadFtpDirectory(string remoteFtpDirUrl, string targetDirt)
        {
            try
            {
                if (string.IsNullOrEmpty(remoteFtpDirUrl)) remoteFtpDirUrl = $"{host}/";

                var listRequest = (FtpWebRequest) WebRequest.Create(remoteFtpDirUrl);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = _networkCredential;

                var lines = new List<string>();

                using (var listResponse = (FtpWebResponse) listRequest.GetResponse())
                using (var listStream = listResponse.GetResponseStream())
                using (var listReader = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream) lines.Add(listReader.ReadLine());
                }

                foreach (var line in lines)
                {
                    var tokens = line.Split(new[] {' '}, 9, StringSplitOptions.RemoveEmptyEntries);

                    var name = tokens[8]; //get folder
                    var permissions = tokens[0]; // get permition

                    var localFilePath = Path.Combine(targetDirt, name);
                    var fileUrl = remoteFtpDirUrl + name;

                    if (permissions[0] == 'd')
                    {
                        if (!Directory.Exists(localFilePath)) Directory.CreateDirectory(localFilePath);

                        DownloadFtpDirectory(fileUrl + "/", localFilePath);
                    }
                    else
                    {
                        var downloadRequest = (FtpWebRequest) WebRequest.Create(fileUrl);
                        downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                        downloadRequest.Credentials = _networkCredential;

                        using (var downloadResponse = (FtpWebResponse) downloadRequest.GetResponse())
                        using (var sourceStream = downloadResponse.GetResponseStream())
                        using (Stream targetStream = File.Create(localFilePath))
                        {
                            var buffer = new byte[10240];
                            int read;
                            while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                                targetStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage(ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="directory">DirName/ or File Name text.txt</param>
        /// <param name="method"></param>
        private void CreateFTPRequest(string directoryOrfileName, string method)
        {
            CreateCredential(directoryOrfileName);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = method;
        }


        /// <summary>
        /// </summary>
        /// <param name="hostIP"></param>
        private void SetHostIP(string hostIP, string port)
        {
            if (hostIP.Contains("FTP") || hostIP.Contains("SFTP"))
            {
                var hst = hostIP.Replace("//", "").Split(":");
                hostIP = hst[1];
                if (hst.Length > 2) port = hst[2];
            }


            IPAddress address = null;
            IPAddress.TryParse(hostIP, out address);

            if (address == null)
            {
                var hostEntry = Dns.GetHostEntry(hostIP);
                foreach (var item in hostEntry.AddressList)
                    //var endpoint = new IPEndPoint(IPAddress.Parse(item.MapToIPv4().ToString()),int.Parse(port));

                    //var client = new LookupClient(endpoint);

                    address = item.MapToIPv4();
            }

            host = "FTP://" + address;
        }


        public override string ToString()
        {
            return host.ToUpper();
        }

        private void ExceptionMessage(Exception ex)
        {
            throw new Exception(string.Format($"Error FTP: {host} ," + $" User: {_networkCredential.UserName}, Msg: ") +
                                ex.Message);
        }
    }
}