using System;
using System.Linq;
using MongoDB.Bson.IO;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class TermDetailTest : IDisposable
    {
        public TermDetailTest(ITestOutputHelper testOutputHelper)
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
        public void TermWhenCreated_TypeDebt_IsValid()
        {
            Assert.NotEqual(0, SystemUnderTest.DebtCode);
            Assert.Equal(54856, SystemUnderTest.DebtCode);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TermWhenCreated_AddDetails_Configuration()
        {
            SystemUnderTest.AddDetailConfig(null,FileAccessType.FTP);
            SystemUnderTest.AddDetailConfig(null,FileAccessType.API);
            SystemUnderTest.AddDetailConfig(null,FileAccessType.Email);
            
            //has configurations
            Assert.Equal(3, SystemUnderTest.TermDetailConfigs.Count);

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
    }
}