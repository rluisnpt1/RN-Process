using IntrumCommon.Lib;
using NUnit.Framework;
using System.Linq;
using System.Text;

namespace Fundos_Auto.Test
{
    [TestFixture]
    public class IntrumFileTesttTest1
    {
        private string filename = "test.txt";

        private const string FILETestepath = @"C:\\Intrum\\fundos\\";
        private const string FILETestepathzip = FILETestepath + @"Enviados\\";
        private string ZipfullPath = "";
        private string ZipfullPath2 = "";
        private string ZipfullPath3 = "";

        private string ZipPath1 = "";
        private string ZipPath2 = "";
        private string ZipPath3 = "";

        [SetUp]
        public void OnTestInitialize()
        {
            //CREATE BASE FOLDER
            var v1 = IntrumFile.CreateDirectory(FILETestepath + "1\\");
            var v2 = IntrumFile.CreateDirectory(FILETestepath + "2\\");
            var v3 = IntrumFile.CreateDirectory("c:\\TEMP\\" + "2\\");
            IntrumFile.CreateDirectory(FILETestepathzip);

            //CREATE BASE FILE
            IntrumFile.CreateFile(v1 + filename);
            IntrumFile.CreateFile(v2 + filename);
            IntrumFile.CreateFile(v3 + filename);

            ZipfullPath = v1 + filename;
            ZipfullPath2 = v2 + filename;
            ZipfullPath3 = v3 + filename;

            ZipPath1 = v1;
            ZipPath2 = v2;
            ZipPath3 = v3;
        }

        [Test]
        public void Should_DeleteFile_If_Exits()
        {
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.DeleteFileIfExit(ZipfullPath2);

            var actual = 0;

            var getList = IntrumFile.GetFilesInDirectory(ZipPath1);

            Assert.AreEqual(actual, getList.Length);
        }

        [Test]
        public void Should_CopyFile()
        {
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.CopyFileFromDirectoryTo(ZipfullPath3, ZipfullPath);

            var actual = 1;

            var getList = IntrumFile.GetFilesInDirectory(ZipPath1);

            Assert.AreEqual(actual, getList.Length);
        }

        [Test]
        public void Should_MoveFile()
        {
            IntrumFile.MoveFileFromDirectory(ZipfullPath3, ZipfullPath);

            var actual = 1;

            var getListTo = IntrumFile.GetFilesInDirectory(ZipPath1);
            var getListFrom = IntrumFile.GetFilesInDirectory(ZipPath3);

            Assert.IsNotNull(getListTo);
            Assert.IsNotEmpty(getListTo);
            Assert.AreEqual(0, getListFrom.Length);
        }

        [Test]
        public void Should_CreateZipeFile()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.CopyFileFromDirectoryTo(ZipfullPath3, ZipfullPath);
            IntrumFile.MoveFileFromDirectory(ZipfullPath3, ZipfullPath);

            var actual = 1;
            var file  =  IntrumFile.CreateDoubleZipFileContent(ZipPath1, ZipfullPath.Replace(".txt", ""), "123456");
            var sut = IntrumFile.GetFilesInDirectory(ZipPath1, @"*.zip");

            Assert.AreEqual(actual, sut.Length);
            IntrumFile.DeleteFileIfExit(file);
            var sut2 = IntrumFile.GetFilesInDirectory(ZipPath1, @"*.zip");
            Assert.AreEqual(0, sut2.Length);
        }

        [Test]
        public void Should_ListOnliZipeFile()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.CopyFileFromDirectoryTo(ZipfullPath3, ZipfullPath);
           

            
            var actual = IntrumFile.CreateDoubleZipFileContent(ZipPath1, ZipfullPath.Replace(".txt", ""), "123456");
            var sut = IntrumFile.GetFilesInDirectory(ZipPath1, @"*.zip");
            Assert.AreEqual(1, sut.Length);
            Assert.Contains(actual, sut);
            
        }

        [Test]
        public void Should_DeliteAllFiles()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.CopyFileFromDirectoryTo(ZipfullPath3, ZipfullPath);
                       
            var actual = IntrumFile.CreateDoubleZipFileContent(ZipPath1, ZipfullPath.Replace(".txt", ""), "123456");
            var list = IntrumFile.GetFilesInDirectory(ZipPath1, @"*.zip");

            foreach (var item in list)
            {
                IntrumFile.DeleteFileIfExit(item);
            }

            var sut = IntrumFile.GetFilesInDirectory(ZipPath1, @"*.zip");

            Assert.AreEqual(0, sut.Length);
            Assert.IsEmpty(sut);
            
        }

              [Test]
        public void Should_MoveFileZipeToEnviadosEndCleanFolder()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IntrumFile.DeleteFileIfExit(ZipfullPath);
            IntrumFile.CopyFileFromDirectoryTo(ZipfullPath3, ZipfullPath);
            IntrumFile.MoveFileFromDirectory(ZipfullPath3, ZipfullPath);

            
            var actual = IntrumFile.CreateDoubleZipFileContent(ZipPath1, null, "123456");
            var name = actual.Split('\\').Last();

            IntrumFile.MoveFileFromDirectory(actual, FILETestepathzip + name);
            IntrumFile.DeleteFileIfExit(ZipfullPath);

            var sut = IntrumFile.GetFilesInDirectory(FILETestepathzip, @"*.zip");            
            var acutualPath = IntrumFile.GetFilesInDirectory(ZipPath1);

            Assert.AreEqual(1, sut.Length);            
            Assert.AreEqual(0, acutualPath.Length);
            Assert.IsEmpty(acutualPath);
            IntrumFile.DeleteFileIfExit(FILETestepathzip);
            

        }

    }
}