using FluentAssertions;
using IntrumCommon.Lib;
using IntrumCommon.Lib.FTP;
using NUnit.Framework;

namespace IntrumCommon.Test
{
    [TestFixture]
    public class FtpTest
    {
        private const string FromIntrum = "/from_intrum/";
        private const string ToIntrum = "/to_intrum/";
        private const string FILETestepath = @"c:\temp\";
        private const string FILETestepath2 = @"c:\temp\nivel2\";
        private const string FILETestepath3 = @"c:\temp\nivel3";
        private const string FILETesteName = @"INTRUMTESTEFTP.txt";
        private const string DirectoryName = @"TESTFTP";
        private FtpClientCredencial testeCredential;
        private IFtpClient _ftp;

        [SetUp]
        public void Setup()
        {
            SetUserCredential();
            IntrumFile.CreateFile(FILETestepath + FILETesteName);
            IntrumFile.CreateDirectory(FILETestepath2);
            IntrumFile.CreateDirectory(FILETestepath3);
        }

        [TearDown]
        public void TestCleanup()
        {
            // Runs after each test. (Optional)
            ReMoveDirectoryIfExist(FromIntrum);
        }


        [Test]
        public void SFTP_shouldStartWithFTP_WhenDnsNAme()
        {
            var conenection = _ftp.ToString();
            _ftp.Should().NotBeNull();
            conenection.ToUpper().Should().Contain("FTP");
        }


        [Test]
        public void SFTP_should_Be_Loged_InFtp()
        {
            var SUT = _ftp.CreateCredential("");
            var SUT2 = _ftp.DirectoryListSimple("");

            SUT.RequestUri.Should().NotBeNull();
            SUT.RequestUri.Host.Should().NotBeNull();
            SUT.RequestUri.AbsoluteUri.ToUpper().Should().Contain("FTP");

            SUT2.Should().NotBeNullOrEmpty();
            SUT2.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void SFTP_ListClients_should_Be_Loged_InFtp()
        {
            var SUT = _ftp.CreateCredential("");
            var SUT2 = _ftp.DirectoryListSimple(FromIntrum);

            SUT.RequestUri.Should().NotBeNull();
            SUT.RequestUri.Host.Should().NotBeNull();
            SUT.RequestUri.AbsoluteUri.ToUpper().Should().Contain("FTP");
            SUT2.Should().NotBeNullOrEmpty();
        }


        [Test]
        public void SFTP_shouldCreateDirectory()
        {
            //Arrange
            ReMoveDirectoryIfExist(FromIntrum);
            // Act
            _ftp.CreateDirectory(FromIntrum + DirectoryName);
            var SUT = _ftp.DirectoryListSimple(FromIntrum);

            // Assert
            SUT.Should().NotBeNullOrEmpty();
            SUT.Should().Contain(DirectoryName);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SFTP_ShouldUploadFile()
        {

            //Arrange
            ReMoveDirectoryIfExist(FromIntrum);

            var fileFrom = FILETestepath + FILETesteName;
            var fileTo = FromIntrum + DirectoryName + "/" + FILETesteName;
            _ftp.CreateDirectory(FromIntrum + DirectoryName);
            
            
            // Act
            _ftp.UploadFile(fileTo, fileFrom);
            var SUT = _ftp.DirectoryListSimple(FromIntrum + DirectoryName);

            // Assert
            SUT.Should().NotBeNullOrEmpty();
            SUT.Should().HaveCountGreaterThan(0);
            SUT.Should().Contain(DirectoryName + "/" + FILETesteName);

            //test delete after upload well successed           
            _ftp.Delete(fileTo);
        }


        private void ReMoveDirectoryIfExist(string directory)
        {
            var verifyFolder = _ftp.DirectoryListSimple(directory);

            foreach (var dado in verifyFolder)
            {
                if (dado.Contains(DirectoryName))
                    //test delete after create success
                    _ftp.DeleteDir(FromIntrum + DirectoryName);
            }

        }

        private void SetUserCredential()
        {
            if (testeCredential == null)
            {
                _ftp = new FtpClient("anticimex", "hEkF?08q", "IDCFTPGW.INTRUM.NET", "22222");
            }
            else
                _ftp = new FtpClient(testeCredential.User, testeCredential.Pass, testeCredential.Host);
        }


    }
}
