using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using RN_Process.Api;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;
using WinSCP;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class TermDetailConfigFixture : IDisposable
    {
        public TermDetailConfigFixture(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
            _sut = null;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        private TermDetailConfig _sut;

        private TermDetailConfig SystemUnderTest => _sut ??= _sut = UnitTestUtility.GetTermDetailConfigToTest();


        public TermDetailConfig InitizerTest(
            FileAccessType communicationType,
            string internalHost,
            string baseWorkDir,
            string linkToAccess,
            string linkToAccessType,
            string typeOfResponse,
            bool requiredLogin,
            string authenticationLogin,
            string authenticationPassword,
            string authenticationCodeApp,
            string pathToOriginFile,
            string pathToDestinationFile,
            string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer,
            string fileDeLimiter,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns
        )
        {
            return new TermDetailConfig("", UnitTestUtility.GetTermBaseToTeste(), communicationType, internalHost,
                linkToAccess, linkToAccessType, typeOfResponse, requiredLogin, authenticationLogin,
                authenticationPassword, "", authenticationCodeApp, pathToOriginFile, pathToDestinationFile,
                pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter, fileHeaderColumns,
                availableFieldsColumns);
        }


        [Fact]
        public void CreateCommunicationToDownloadFilesFromFtp()
        {
            var sut = UnitTestUtility.GetRealTermUnicreDetailConfigToTest();
            var file = sut.FtpDownloadingTheMostRecentFileRemoteDir();
            //local dir
            var expect = IntrumFile.GetFilesInDirectory(SystemUnderTest.BaseWorkDirectoryHost, "*");

            expect.Should().NotBeNullOrEmpty("Directory is empty");
            expect.Should().Contain(file);

            Directory.Delete(SystemUnderTest.BaseWorkDirectoryHost, true);
            Assert.Throws<DirectoryNotFoundException>(() =>
                IntrumFile.GetFilesInDirectory(SystemUnderTest.BaseWorkDirectoryHost, "*"));
        }


        [Fact]
        public void
            TermConfigWhenCreated_RequiredLoginIsFalse_And_HasOthersLogin_ThrowException_requiredPassword()
        {
            //arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitizerTest(FileAccessType.FTP, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    false, "my login",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, new List<string> { "cdb" },
                    new List<string> { "ab" }));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);
        }


        [Fact]
        public void TermConfigWhenCreated_termDetail_IsValid()
        {
            // Act
            // Assert
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);

            Assert.IsType<string>(SystemUnderTest.Id);

            Assert.Null(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.UpdatedDate);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.NotNull(SystemUnderTest.RowVersion);
        }


        [Fact]
        public void TermConfigWhenCreated_TermDetailIsValid()
        {
            // Act
            var expect = UnitTestUtility.GetTermDetailConfigToTest();

            // Assert
            Assert.NotNull(SystemUnderTest.TermDetail);
            Assert.Equal(expect.TermDetail.DebtCode, SystemUnderTest.TermDetail.DebtCode);
        }


        [Fact]
        public void TermConfigWhenCreated_then_termDetailId_IsValid()
        {
            // Act
            // Assert
            Assert.NotNull(SystemUnderTest.TermDetail);
            Assert.NotNull(SystemUnderTest.TermDetail.Id);
        }

        [Fact]
        public void TermDetailConfigTest_AvailableFields_ThrowExceptionIfListHasEmptyString()
        {
            // Arrange            
            var expect = Assert.Throws<EncoderFallbackException>(() => InitizerTest(
                FileAccessType.FTP,
                string.Empty,
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                string.Empty,
                string.Empty,
                string.Empty,
                true,
                "my login",
                "s",
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new List<string> { "ss", "s" },
                new List<string> { "ss", " " }));
            // Act            

            // Assert            
            Assert.Contains("list can not have empty strings", expect.Message);
        }

        [Fact]
        public void TermDetailConfigTest_AvailableFields_ThrowExceptionIfNull()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentNullException>(() =>
                InitizerTest(FileAccessType.FTP,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    true, "my login",
                    "s", string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    new List<string> { "ss", "s" }, null));

            Assert.Contains("Value cannot be null. (Parameter 'availableFieldsColumns')", expect.Message);
        }

        [Fact]
        public void TermDetailConfigTest_AvailableFields_ThrowExceptionIfZero()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentException>(() => InitizerTest(
                FileAccessType.FTP,
                string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                new List<string> { "ss", "s" }, new List<string>()));


            Assert.Contains(
                "Required input 'AVAILABLEFIELDSCOLUMNS' cannot be zero. (Parameter 'availableFieldsColumns')",
                expect.Message);
        }

        [Fact]
        public void TermDetailConfigTest_FileHeaderColumns_ThrowExceptionIfListHasEmptyString()
        {
            var expect = Assert.Throws<EncoderFallbackException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                new List<string> { "ss", " " }, new List<string> { "cdb" }));

            // Act            

            // Assert     
            Assert.Contains("list can not have empty strings", expect.Message);
        }

        [Fact]
        public void TermDetailConfigTest_FileHeaderColumns_ThrowExceptionIfNull()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentNullException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                null, new List<string> { "cdb" }));

            Assert.Contains("Value cannot be null. (Parameter 'fileHeaderColumns')", expect.Message);
        }

        [Fact]
        public void TermDetailConfigTest_FileHeaderColumns_ThrowExceptionIfZero()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                new List<string>(), new List<string> { "cdb" }));

            // Act            

            // Assert     
            Assert.Contains("Required input 'FILEHEADERCOLUMNS' cannot be zero.", expect.Message);
            Assert.Equal("fileHeaderColumns", expect.ParamName);
        }

        [Fact]
        public void WhenCreatedTermDetailConfig_delimiter_IsEqualCommaIfNull()
        {
            //arrange

            // Act
            var ex = InitizerTest(FileAccessType.FTP, string.Empty,
                string.Empty, string.Empty,
                string.Empty, string.Empty,
                false, string.Empty,
                string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, new List<string> { "cdb" }, new List<string> { "cdb" });


            // Assert
            Assert.NotNull(ex.FileDelimiter);
            Assert.Equal(",", ex.FileDelimiter);
        }


        [Fact]
        public void WhenCreatedTermDetailConfig_RequiredloginIsTrueLoginNull_ThrowException()
        {
            //arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                UnitTestUtility.GetTermDetailConfigToTest(
                    new TermDetailConfig("", UnitTestUtility.GetTermBaseToTeste(), FileAccessType.FTP, string.Empty,
                        string.Empty, string.Empty, string.Empty, true, string.Empty, string.Empty, "", string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<string> { "cdb" },
                        new List<string> { "cdb" })));

            // Assert
            Assert.Equal("Required input 'AUTHENTICATIONLOGIN' was empty. (Parameter 'authenticationLogin')",
                ex.Message);
            Assert.Equal("authenticationLogin", ex.ParamName);
        }


        [Fact]
        public void WhenCreatedTermDetailConfig_RequiredloginIsTruePasswordEmpty_ThrowExceptionWhen()
        {
            //arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                InitizerTest(FileAccessType.FTP, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    true, "my login",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    new List<string> { "cdb" }, new List<string> { "cdb" }));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);
        }
    }
}