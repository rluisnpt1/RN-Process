﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.DataAccessTests
{
    public class ContractTest : IDisposable
    {
        public ContractTest(ITestOutputHelper testOutputHelper)
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

        private Contract _sut;
        private Contract SystemUnderTest => _sut ??= _sut =UnitTestUtility.GetContractOrganizationToTest();

        private static Contract ContractInit(int contractNumber,Organization Organization)
        {
            return new Contract(contractNumber, Organization);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_Contract_Bson()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();
            Assert.Equal(BsonType.Int32, toBsonDocument["ContractNumber"].BsonType);
            Assert.Equal(BsonType.Int32, toBsonDocument["ContractNumber"].BsonType);

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MongodbToDocument_ContractWithAnId_IsRepresentedAsObjectId()
        {
            var toBsonDocument = SystemUnderTest.ToBsonDocument();

            _testOutputHelper.WriteLine(toBsonDocument.ToJson());
            Assert.Equal(BsonType.ObjectId, toBsonDocument["_id"].BsonType);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractIsValid()
        {
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);
            Assert.NotNull(SystemUnderTest.Organization);
            Assert.NotNull(SystemUnderTest.OrganizationId);

            Assert.NotEqual(0, SystemUnderTest.ContractNumber);
         
            //Assert.NotEqual(0, SystemUnderTest.TypeDebt);
            //Assert.NotEmpty(SystemUnderTest.DebtDescription);


            Assert.NotNull(SystemUnderTest.CreatedBy);
            UnitTestUtility.DateTimeAssertAreEqual(DateTime.UtcNow, SystemUnderTest.CreatedDate, TimeSpan.MaxValue);

            Assert.Null(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.ModifiedDate);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.NotNull(SystemUnderTest.RowVersion);
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
        public void WhenCreated_ContractNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() =>
                ContractInit(0, UnitTestUtility.GetBancoPortugalOrganizationToTest()));

            //assert
            Assert.Contains("Required input 'CONTRACTNUMBER' cannot be zero", ex.Message);
            Assert.Equal("ContractNumber", ex.ParamName);
        }
        
    }
}