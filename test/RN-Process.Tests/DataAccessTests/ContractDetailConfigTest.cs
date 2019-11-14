using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class ContractDetailConfigTest : IDisposable
    {
        public void Dispose()
        {
            _sut = null;
        }

        private ContractDetailConfig _sut;

        private ContractDetailConfig SystemUnderTest
        {
            get
            {
                _sut ??= _sut = UnitTestUtility.GetContractDetailConfigToTest();
                return _sut;
            }
        }

        public ContractDetailConfig InitizerTest(
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
            return new ContractDetailConfig(UnitTestUtility.GetContractOrganizationToTest(), communicationType, internalHost, baseWorkDir, linkToAccess, linkToAccessType, typeOfResponse, requiredLogin, authenticationLogin, authenticationPassword, authenticationCodeApp, pathToOriginFile, pathToDestinationFile, pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter, fileHeaderColumns, availableFieldsColumns);
        }

        [Fact]
        public void ContractDetailConfigTest_AvailableFields_ThrowExceptionIfListHasEmptyString()
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
                new List<string> {"ss", "s"},
                new List<string> {"ss", " "}));
            // Act            

            // Assert            
            Assert.Contains("list can not have empty strings", expect.Message);
        }

        [Fact]
        public void ContractDetailConfigTest_AvailableFields_ThrowExceptionIfNull()
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
                new List<string> {"ss", "s"}, null));

            Assert.Contains("Value cannot be null. (Parameter 'availableFieldsColumns')", expect.Message);
        }

        [Fact]
        public void ContractDetailConfigTest_AvailableFields_ThrowExceptionIfZero()
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
                new List<string> {"ss", "s"}, new List<string>()));


            Assert.Contains(
                "Required input 'AVAILABLEFIELDSCOLUMNS' cannot be zero. (Parameter 'availableFieldsColumns')",
                expect.Message);
        }

        [Fact]
        public void ContractDetailConfigTest_FileHeaderColumns_ThrowExceptionIfListHasEmptyString()
        {
            var expect = Assert.Throws<EncoderFallbackException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                new List<string> {"ss", " "}, new List<string> {"cdb"}));

            // Act            

            // Assert     
            Assert.Contains("list can not have empty strings", expect.Message);
        }

        [Fact]
        public void ContractDetailConfigTest_FileHeaderColumns_ThrowExceptionIfNull()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentNullException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                null, new List<string> {"cdb"}));

            Assert.Contains("Value cannot be null. (Parameter 'fileHeaderColumns')", expect.Message);
        }

        [Fact]
        public void ContractDetailConfigTest_FileHeaderColumns_ThrowExceptionIfZero()
        {
            // Arrange            
            var expect = Assert.Throws<ArgumentException>(() => InitizerTest(FileAccessType.FTP,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                true, "my login",
                "s", string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty,
                new List<string>(), new List<string> {"cdb"}));

            // Act            

            // Assert     
            Assert.Contains("Required input 'FILEHEADERCOLUMNS' cannot be zero.", expect.Message);
            Assert.Equal("fileHeaderColumns", expect.ParamName);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void Update_ContractConfiguration()
        {
            SystemUnderTest.UpdateContractConfiguration(
                SystemUnderTest.ContractId,
                FileAccessType.FTP,
                "LocalHost", "FTP:10.80.5.198", "ETL", "FTP", true, "MYLogin@MyName", "MyPass1234", null,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Origin",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Destination",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Backup",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BackupToHost",
                ",",
                new List<string>
                    {"CLI_NDIV", "CLI_CRED", "CLI_VAL1", "CLI_VAL2"},
                new List<string>
                    {"INTRUM_NDIV", "INTRUM_COD_CRED", "INTRUM_VAL1", "INTRUM_VAL2"},
                true);

            //assert contract created is the same in organization
            Assert.Equal("FTP:10.80.5.198", SystemUnderTest.LinkToAccess);
        }

        [Fact]
        public void WhenCreatedContractDetailConfig_Contract_isValid()
        {
            // Act
            var expect = UnitTestUtility.GetContractDetailConfigToTest();

            // Assert
            Assert.NotNull(SystemUnderTest.Contract);
            Assert.Equal(expect.Contract.ContractNumber, SystemUnderTest.Contract.ContractNumber);
        }


        [Fact]
        public void WhenCreatedContractDetailConfig_ContractId_IsValid()
        {
            // Act
            // Assert
            Assert.NotNull(SystemUnderTest.Contract);
            Assert.NotNull(SystemUnderTest.Contract.Id);
        }

        [Fact]
        public void WhenCreatedContractDetailConfig_delimiter_IsEqualCommaIfNull()
        {
            //arrange

            // Act
            var ex = InitizerTest(FileAccessType.FTP, string.Empty,
                string.Empty, string.Empty,
                string.Empty, string.Empty,
                false, string.Empty,
                string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, new List<string> {"cdb"}, new List<string> {"cdb"});


            // Assert
            Assert.NotNull(ex.FileDelimiter);
            Assert.Equal(",", ex.FileDelimiter);
        }


        [Fact]
        public void WhenCreatedContractDetailConfig_IsValid()
        {
            // Act
            // Assert
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);

            Assert.IsType<string>(SystemUnderTest.Id);

            Assert.Null(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.ModifiedDate);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.NotNull(SystemUnderTest.RowVersion);
        }

        [Fact]
        public void
            WhenCreatedContractDetailConfig_RequiredloginIsFalse_LoginIsNotNUll_ThrowException_requiredPassword()
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
                    string.Empty, string.Empty, new List<string> {"cdb"},
                    new List<string> {"ab"}));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);
        }


        [Fact]
        public void WhenCreatedContractDetailConfig_RequiredloginIsTrueLoginNull_ThrowException()
        {
            //arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                UnitTestUtility.GetContractDetailConfigToTest(
                    new ContractDetailConfig(UnitTestUtility.GetContractOrganizationToTest(), FileAccessType.FTP, string.Empty, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), string.Empty, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<string> {"cdb"}, new List<string> {"cdb"})));

            // Assert
            Assert.Equal("Required input 'AUTHENTICATIONLOGIN' was empty. (Parameter 'authenticationLogin')",
                ex.Message);
            Assert.Equal("authenticationLogin", ex.ParamName);
        }


        [Fact]
        public void WhenCreatedContractDetailConfig_RequiredloginIsTruePasswordEmpty_ThrowExceptionWhen()
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
                    new List<string> {"cdb"}, new List<string> {"cdb"}));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);
        }
    }
}