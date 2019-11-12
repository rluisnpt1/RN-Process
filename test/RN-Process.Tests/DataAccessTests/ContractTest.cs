using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using RN_Process.Api.DataAccess.Entities;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class ContractTest : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private const int TypeDebt = 003344;
        private const string NameDebt = "need to pay";

        public ContractTest(ITestOutputHelper testOutputHelper)
        {
            JsonWriterSettings.Defaults.Indent = true;
            _testOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
            _sut = null;
        }

        private Contract _sut;
        private Contract SystemUnderTest => _sut ?? UnitTestUtility.GetContractOrganizationToTest();

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractIsValid()
        {
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);
        //   Assert.IsType<string>(SystemUnderTest.Id);
            Assert.NotNull(SystemUnderTest.Organization);
            Assert.NotEqual(0, SystemUnderTest.ContractNumber);
            Assert.NotEqual(0, SystemUnderTest.TypeDebt);
            Assert.NotEmpty(SystemUnderTest.DebtDescription);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() => ContractInit(0, TypeDebt, NameDebt, UnitTestUtility.GetBancoPortugalOrganizationToTest()));

            //assert
            Assert.Contains("Required input 'CONTRACTNUMBER' cannot be zero", ex.Message);
            Assert.Equal("ContractNumber", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractNumber_IsValid()
        {
            Assert.NotEqual(0, SystemUnderTest.ContractNumber);
            Assert.Equal(14533686, SystemUnderTest.ContractNumber);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_TypeDebtNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() => ContractInit(14533686, 0, NameDebt, UnitTestUtility.GetBancoPortugalOrganizationToTest()));

            //assert
            Assert.Contains("Required input 'TYPEDEBT' cannot be zero", ex.Message);
            Assert.Equal("typeDebt", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_TypeDebt_IsValid()
        {
            Assert.NotEqual(0, SystemUnderTest.TypeDebt);
            Assert.Equal(5546, SystemUnderTest.TypeDebt);
        }
       
        [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_Contract_Bson()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();
            Assert.Equal(BsonType.Int32,toBsonDocument["ContractNumber"].BsonType);
            Assert.Equal(BsonType.Int32,toBsonDocument["ContractNumber"].BsonType);

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
        }

         [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_ContractWithAnId_IsRepresentedAsObjectId()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
            Assert.Equal(BsonType.ObjectId,toBsonDocument["_id"].BsonType);
        }


        private static Contract ContractInit(int contractNumber, int typeDebt, string nameDebt, Organization Organization)
        {
            return new Contract(contractNumber, typeDebt, nameDebt, Organization);
        }
    }
}
