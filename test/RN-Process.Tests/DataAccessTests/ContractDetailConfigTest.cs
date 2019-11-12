using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class ContractDetailConfigTest : IDisposable
    {

        public void Dispose()
        {
            _sut = null;
        }

        public ContractDetailConfigTest()
        {

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


        [Fact]
        public void WhenCreatedContractDetailConfig_RequiredloginIsTrueLoginNull_ThrowException()
        {
            //arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => 
                UnitTestUtility.GetContractDetailConfigToTest(
                new ContractDetailConfig(string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    true, string.Empty,
                    string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, new List<string>(),
                    UnitTestUtility.GetContractOrganizationToTest())));

            // Assert
            Assert.Equal("Required input 'AUTHENTICATIONLOGIN' was empty. (Parameter 'authenticationLogin')", ex.Message);
            Assert.Equal("authenticationLogin", ex.ParamName);

        }
        
        [Fact]
        public void WhenCreatedContractDetailConfig_delimiter_IsEqualCommaIfNull()
        {
            //arrange

            // Act
            var ex = InitizerTest("FTP",
                string.Empty, string.Empty,
                string.Empty, string.Empty,
                false, string.Empty,
                string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, new List<string>());


            // Assert
            Assert.NotNull(ex.FileDelimiter);
            Assert.Equal(",", ex.FileDelimiter);

        }


         [Fact]
        public void WhenCreatedContractDetailConfig_RequiredloginIsTruePasswordEmpty_ThrowExceptionWhen()
        {
            //arrange

            // Act
            var ex = Assert.Throws<System.ArgumentException>(() =>
                InitizerTest("FTP",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    true, "my login",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, new List<string>()));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);

        }  
        
        [Fact]
        public void WhenCreatedContractDetailConfig_RequiredloginIsFalse_LoginIsNotNUll_ThrowException_requiredPassword()
        {
            //arrange

            // Act
            var ex = Assert.Throws<System.ArgumentException>(() =>
              InitizerTest("FTP",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty,
                    false, "my login",
                    string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, new List<string>()));

            // Assert
            Assert.Equal("Required input 'PASSWORD' was empty. (Parameter 'password')", ex.Message);
            Assert.Equal("password", ex.ParamName);

        }

        [Fact]
        public void WhenCreatedContractDetailConfig_Contract_isValid()
        {
            // Act
            var expect = UnitTestUtility.GetContractDetailConfigToTest();

            // Assert
            Assert.NotNull(SystemUnderTest.Contract);
            Assert.Equal(expect.Contract.ContractNumber,SystemUnderTest.Contract.ContractNumber);
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

        public ContractDetailConfig InitizerTest( 
            string communicationType,
              string internalHost,
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
              IList<string> fileHeaderColumns)
        {
            
           return new ContractDetailConfig(
               communicationType,
               internalHost,
               linkToAccess,
               linkToAccessType,
               typeOfResponse,
               requiredLogin,
               authenticationLogin,
               authenticationPassword,
               authenticationCodeApp,
               pathToOriginFile,
               pathToDestinationFile,
               pathToFileBackupAtClient,
               pathToFileBackupAtHostServer,
               fileDeLimiter,
               fileHeaderColumns,
               UnitTestUtility.GetContractOrganizationToTest()
                );
        }
    }
}
