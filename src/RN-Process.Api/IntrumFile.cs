using System;
using System.IO;
using System.Text;

namespace RN_Process.Api
{
    /// <summary>
    ///     Configuration File for intrum
    /// </summary>
    public static class IntrumFile
    {
        
        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateDirectory(string path)
        {
            if (!Directory.Exists(path)) return "";
            Console.WriteLine($"Begin process of {path}");
            return Directory.CreateDirectory(path).FullName;
        }

        /// <summary>
        /// </summary>
        /// <param name="pathDir"></param>
        public static void DeleteZipFile(string pathDir)
        {
            var fileList1 = Directory.GetFiles(pathDir, @"*");
            foreach (var file in fileList1) File.Delete(file);
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
        /// </summary>
        /// <param name="FilePathfromDir"></param>
        /// <param name="toDir"></param>
        public static void CopyFileFromDirectoryTo(string FilePathfromDir, string toDir)
        {
            File.Copy(FilePathfromDir, toDir);
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFile(string path)
        {
            try
            {
                DeleteFileIfExit(path);

                CreateOnfileWithDefaultValueData(path);

                // Open the stream and read it back.
                using (var sr = File.OpenText(path))
                {
                    var s = "";
                    while ((s = sr.ReadLine()) != null) Console.WriteLine(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        private static void CreateOnfileWithDefaultValueData(string path)
        {
            // Create the file.
            using (var fs = File.Create(path))
            {
                var info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFileIfExit(string path)
        {
            // Delete the file if it exists.
            if (File.Exists(path))
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                File.Delete(path);
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