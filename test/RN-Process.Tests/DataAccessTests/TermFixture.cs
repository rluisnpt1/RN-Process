using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using RN_Process.Api.DataAccess.Entities;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class TermTest : IDisposable
    {
        public TermTest(ITestOutputHelper testOutputHelper)
        {
            JsonWriterSettings.Defaults.Indent = true;
            _testOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
            _sut = null;
        }

        private readonly ITestOutputHelper _testOutputHelper;
        private const int TypeDebt = 003344;
        private const string NameDebt = "need to pay";

        private Term _sut;
        private Term SystemUnderTest => _sut ??= _sut = UnitTestUtility.GetTermOrganizationToTest();

        private static Term TermInit(int termNumber, Organization Organization)
        {
            return new Term(termNumber, Organization);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_Term_Bson()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();
            Assert.Equal(BsonType.Int32, toBsonDocument["TermNumber"].BsonType);
            Assert.Equal(BsonType.Int32, toBsonDocument["TermNumber"].BsonType);

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_TermWithAnId_IsRepresentedAsObjectId()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
            Assert.Equal(BsonType.ObjectId, toBsonDocument["_id"].BsonType);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_TermIsValid()
        {
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);
            Assert.NotNull(SystemUnderTest.Organization);
            Assert.NotNull(SystemUnderTest.OrganizationId);

            Assert.NotEqual(0, SystemUnderTest.TermNumber);

            //Assert.NotEqual(0, SystemUnderTest.TypeDebt);
            //Assert.NotEmpty(SystemUnderTest.DebtDescription);


            Assert.NotNull(SystemUnderTest.CreatedBy);
            UnitTestUtility.DateTimeAssertAreEqual(DateTime.UtcNow, SystemUnderTest.CreatedDate, TimeSpan.MaxValue);

            Assert.Null(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.UpdatedDate);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.NotNull(SystemUnderTest.RowVersion);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_TermNumber_IsValid()
        {
            Assert.NotEqual(0, SystemUnderTest.TermNumber);
            Assert.Equal(14533686, SystemUnderTest.TermNumber);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_TermNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() =>
                TermInit(0, UnitTestUtility.GetBancoPortugalOrganizationToTest()));

            //assert
            Assert.Contains("Required input 'TERMNUMBER' cannot be zero", ex.Message);
            Assert.Equal("TermNumber", ex.ParamName);
        }
    }
}