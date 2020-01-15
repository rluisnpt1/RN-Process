using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.IO;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class TermDetailFixture : IDisposable
    {
        public TermDetailFixture(ITestOutputHelper testOutputHelper)
        {
            JsonWriterSettings.Defaults.Indent = true;
            _testOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
            _sut = null;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        private TermDetail _sut;
        private TermDetail SystemUnderTest => _sut ??= _sut = UnitTestUtility.GetTermBaseToTeste();


        [Fact]
        [Trait("Category", "Unit")]
        public void TermDetail_WhenCreatedDataCriation_ThenThrowException()
        {
            //arrange
            var expect = DateTime.UtcNow;
            //act
            var ex = Assert.Throws<Exception>(() =>
                UnitTestUtility.DateTimeAssertAreEqual(expect, SystemUnderTest.CreatedDate, TimeSpan.MinValue));

            //assert
            Assert.Contains("Expected Date: " + expect, ex.Message);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void TermDetail_WhenCreatedTypeDebtNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() =>
                new TermDetail(0, TermsType.Loan, UnitTestUtility.GetTermOrganizationToTest()));

            //assert
            Assert.Contains("Required input 'DEBTCODE' cannot be zero", ex.Message);
            Assert.Equal("debtCode", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TermWhenCreated_AddDetails_Configuration()
        {
            var teste1 = new TermDetailConfig(null, SystemUnderTest, FileAccessType.LocalMachine,
                string.Empty, string.Empty, string.Empty,
                string.Empty, false, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, string.Empty,
                new List<string> {"teste1"}, new List<string> {"teste2"});

            var teste2 = new TermDetailConfig(null, SystemUnderTest, FileAccessType.Email,
                string.Empty, string.Empty, string.Empty,
                string.Empty, false, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, true, string.Empty,
                new List<string> {"teste1"}, new List<string> {"teste2"});

            SystemUnderTest.AddDetailConfig(string.Empty,
                teste1.CommunicationType,
                string.Empty,
                teste1.LinkToAccess,
                teste1.LinkToAccessType,
                teste1.TypeOfResponse,
                teste1.RequiredLogin,
                teste1.AuthenticationLogin,
                Encoding.ASCII.GetString(teste1.AuthenticationPassword),
                Encoding.ASCII.GetString(teste1.HostKeyFingerPrint),
                teste1.AuthenticationCodeApp,
                teste1.PathToOriginFile,
                teste1.PathToDestinationFile,
                teste1.PathToFileBackupAtClient,
                string.Empty,
                teste1.FileDelimiter,
                teste1.HasHeader,
                teste1.FileProtectedPassword,
                teste1.FileHeaderColumns,
                teste1.AvailableFieldsColumns);

            SystemUnderTest.AddDetailConfig(string.Empty,
                teste2.CommunicationType,
                string.Empty,
                teste2.LinkToAccess,
                teste2.LinkToAccessType,
                teste2.TypeOfResponse,
                teste2.RequiredLogin,
                teste2.AuthenticationLogin,
                Encoding.ASCII.GetString(teste2.AuthenticationPassword),
                Encoding.ASCII.GetString(teste2.HostKeyFingerPrint),
                teste2.AuthenticationCodeApp,
                teste2.PathToOriginFile,
                teste2.PathToDestinationFile,
                teste2.PathToFileBackupAtClient,
                string.Empty,
                teste2.FileDelimiter,
                teste2.HasHeader,
                teste2.FileProtectedPassword,
                teste2.FileHeaderColumns,
                teste2.AvailableFieldsColumns);
            ;

            //has configurations
            Assert.Equal(2, SystemUnderTest.TermDetailConfigs.Count);

            //has term  the same
            Assert.Same(SystemUnderTest.TermId, SystemUnderTest.Term.Id);

            //get term from get 
            var actualConfig = SystemUnderTest.TermDetailConfigs.First();

            //assert configuration has term 
            Assert.Same(SystemUnderTest, actualConfig.TermDetail);
            Assert.Same(SystemUnderTest.Id, actualConfig.TermDetailId);
            Assert.Same(SystemUnderTest.OrgCode, actualConfig.OrgCode);
            //

            Assert.NotNull(actualConfig.BaseWorkDirectoryHost);
            Assert.True(actualConfig.Active);
            Assert.False(actualConfig.Deleted);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void TermWhenCreated_TypeDebt_IsValid()
        {
            Assert.NotEqual(0, SystemUnderTest.DebtCode);
            Assert.Equal(54856, SystemUnderTest.DebtCode);
        }
    }
}