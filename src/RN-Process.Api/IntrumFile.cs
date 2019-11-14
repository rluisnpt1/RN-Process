using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RN_Process.Api
{
    /// <summary>
    /// Configuration File for intrum
    /// </summary>
    public static class IntrumFile
    {
        /// <summary>
        /// Verify if server is in deve or production to set correct path directory
        /// Verify names in appsettings.json
        /// return a url path
        /// </summary>
        /// <returns></returns>
        //public static string GetBASE()
        //{
        //    var env = ConfigEnvironment.GetEnviroment();

        //    var path = "";

        //    if (env.Equals("DEV"))
        //    {
        //        path = AppDomain.CurrentDomain.BaseDirectory;
        //    }
        //    if (env.Equals("PROD"))
        //    {
        //        path = "F:\\";
        //    }
        //    return path;
        //}

        ///// <summary>
        ///// Create an directory if not exit 
        ///// Verify names in appsettings.json
        ///// return a base directory work
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static string CreateWorkDirectoryIfNotExistis(string name)
        //{
        //    var DIR = "";
        //    if (ConfigEnvironment.GetEnviroment().Equals("DEV"))
        //    {
        //        DIR = Path.GetFullPath(Path.Combine(GetBASE(), @"..\..\..\"));
        //    }
        //    if (ConfigEnvironment.GetEnviroment().Equals("PROD"))
        //    {
        //        //app config
        //        var pjn = ConfigEnvironment.GetProjectName();

        //        DIR = Path.GetFullPath(GetBASE() + "WORK\\" + pjn + "\\");
        //    }

        //    return CreateDirectory(DIR + name); // no need to check if it exists            
        //}

        ///// <summary>
        ///// create based on server or dev path
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static string CreateDirectoryBase(string name)
        //{
        //    var DIR = "";
        //    if (ConfigEnvironment.GetEnviroment().Equals("DEV"))
        //    {
        //        DIR = Path.GetFullPath(Path.Combine(GetBASE(), @"..\..\..\"));
        //    }
        //    if (ConfigEnvironment.GetEnviroment().Equals("PROD"))
        //    {
        //        DIR = Path.GetFullPath(GetBASE());
        //    }

        //    return CreateDirectory(DIR + name); // no need to check if it exists            
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path).FullName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathDir"></param>
        public static void DeleteZipFile(string pathDir)
        {
            string[] fileList1 = Directory.GetFiles(pathDir, @"*");
            foreach (string file in fileList1)
            {
                File.Delete(file);
            }
        }



        //public static string CreateDoubleZipFileContent(string folderSource, string fileDescription, string password)
        //{
        //    string contentName = HelperClass.RemoveSpecialCharacters(fileDescription) + "_Content_" + DateTime.Now.ToString("yyyyMMddHHmmss")+ ".zip";

        //    using (var zip = new ZipFile())
        //    {
        //        zip.AlternateEncoding = Encoding.UTF8;
        //        zip.AlternateEncodingUsage = ZipOption.Always;

        //        zip.Password = password;
        //        zip.Encryption = EncryptionAlgorithm.WinZipAes128;
        //        zip.AddDirectory(folderSource); //convet all itens inside of dictory
        //        zip.Save(Path.Combine(folderSource, contentName));
        //    }
        //    return Path.Combine(folderSource, contentName);
        //    //if (File.Exists(Path.Combine(newDateFolder, "content.zip")))
        //    //    File.Delete(Path.Combine(newDateFolder, "content.zip"));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDir"></param>
        /// <param name="toDir"></param>
        public static void MoveFileFromDirectory(string fromDir, string toDir)
        {
            if (File.Exists(toDir))
                File.Delete(toDir);

            File.Move(fromDir, toDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePathfromDir"></param>
        /// <param name="toDir"></param>
        public static void CopyFileFromDirectoryTo(string FilePathfromDir, string toDir)
        {
            File.Copy(FilePathfromDir, toDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFile(string path)
        {
            try
            {
                DeleteFileIfExit(path);

                CreateOnfileWithDefaultValueData(path);

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private static void CreateOnfileWithDefaultValueData(string path)
        {
            // Create the file.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFileIfExit(string path)
        {
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                File.Delete(path);
            }
        }

        public static string[] GetFilesInDirectory(string path)
        {
            return Directory.GetFiles(path);
        }

        public static string[] GetFilesInDirectory(string path, string typeOffile)
        {
            return Directory.GetFiles(path, typeOffile);
        }
    }
}
