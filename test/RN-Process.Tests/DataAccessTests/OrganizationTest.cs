using System;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class OrganizationTest : IDisposable
    {
        public void Dispose()
        {
            _sut = null;
        }

        private const string CodClientForTest = "003344";
        private const string Name = "First Organization";

        private Organization _sut;

        private Organization SystemUnderTest => _sut ??= _sut = CustomerInit(Name, CodClientForTest);

        private static Organization CustomerInit(string description, string uniqcode)
        {
            return new Organization(description, uniqcode);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_CustomerIsvalid()
        {
            Assert.NotNull(SystemUnderTest);
            Assert.NotNull(SystemUnderTest.Id);
            Assert.IsType<string>(SystemUnderTest.Id);

            Assert.IsType<string>(SystemUnderTest.Id);


            Assert.NotNull(SystemUnderTest.OrgCode);
            Assert.NotEmpty(SystemUnderTest.OrgCode);
            Assert.NotEmpty(SystemUnderTest.Description);

            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
            Assert.Null(SystemUnderTest.ModifiedDate);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_DataCriation_ThenThrowException()
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
        public void WhenCreated_DescriptionEmpty_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() => CustomerInit("", "123B3"));

            //assert
            Assert.Contains("Required input 'DESCRIPTION' was empty. (Parameter 'description')", ex.Message);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_DescriptionGreaterThen250_ThenSizeNameIsNotValid()
        {
            var bigName = "";
            for (var i = 0; i < 251; i++) bigName = bigName + "My task";
            //act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CustomerInit(bigName, "Open"));

            //assert
            Assert.Contains("Input 'DESCRIPTION' was out of range", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_DescriptionLessThenFive_ThenSizeNameIsNotValid()
        {
            //act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CustomerInit("myn", CodClientForTest));

            //assert
            Assert.Equal("description (Parameter 'Input 'DESCRIPTION' was out of range')", ex.Message);
            Assert.Contains("Input 'DESCRIPTION' was out of range", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_DescriptionNull_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentNullException>(() => CustomerInit(null, "124B45"));

            //assert
            Assert.Contains("Value cannot be null.", ex.Message);
            Assert.Equal("description", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_RowVersion_IsNotNull()
        {
            //assert
            Assert.NotNull(SystemUnderTest.RowVersion);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UniqCodeEmpty_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() => CustomerInit(Name, ""));

            //assert
            Assert.Contains("Required input 'ORGCODE' was empty", ex.Message);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UniqCodeGreaterThen10_ThenSizeNameIsNotValid()
        {
            var bigCode = "";
            for (var i = 0; i < 11; i++) bigCode = bigCode + "My task";
            //act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CustomerInit("mY NAME OK", bigCode));

            //assert
            Assert.Contains("Input 'ORGCODE' was out of range", ex.ParamName);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UniqCodeLessThenThree_ThenSizeNameIsNotValid()
        {
            //act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CustomerInit(Name, "w"));

            //assert
            Assert.Contains("orgCode (Parameter 'Input 'ORGCODE' was out of range')", ex.Message);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UNIQCODEnull_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentNullException>(() => CustomerInit(Name, null));

            //assert
            Assert.Contains("Value cannot be null.", ex.Message);
            Assert.Equal("orgCode", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddContract_ThenFactIsAddedToCollection_StringBasedContract()
        {
            //act

            SystemUnderTest.AddContract(null, 99, 665, "qualquer");

            Assert.Equal(1, SystemUnderTest.Contracts.Count);
            //get contract from organization
            var actual = SystemUnderTest.Contracts.First();

            //assert contract created is the same in organization
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddContractTwice_TheContractIsEqualTwo()
        {
            //act

            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 99, 665, "qualquer");

            Assert.Equal(2, SystemUnderTest.Contracts.Count);
            //get contract from organization
            var actual = SystemUnderTest.Contracts.First();

            //assert contract created is the same in organization
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }
        /// <summary>
        /// When contract is 
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddContract_TheContractConfigurationIsCreated()
        {
            //act
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");

            //has contract
            Assert.Equal(1, SystemUnderTest.Contracts.Count);

            //has detail
            Assert.Equal(1, SystemUnderTest.ContractDetails.Count);

            //get contract from organization
            var actualActual = SystemUnderTest.Contracts.First();
            var actualConfiguration = SystemUnderTest.ContractDetails.First();

            //assert contract created is the same in organization
            Assert.Same(SystemUnderTest.Id, actualActual.OrganizationId);
            Assert.Same(SystemUnderTest, actualActual.Organization);
            Assert.Same(SystemUnderTest.OrgCode, actualActual.OrgCode);
            Assert.Same(actualActual.Id, actualConfiguration.ContractId);

            //
            Assert.Equal(SystemUnderTest.OrgCode, actualConfiguration.OrgCode);

            Assert.Equal(1, actualActual.ContractDetailsConfigs.Count);
            Assert.True(actualConfiguration.Active);
            Assert.False(actualConfiguration.Deleted);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddContractIsCalledWithNonZeroIdThenContractIsModified_DateRangeContract()
        {
            //act

            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 99, 665, "qualquer");
            SystemUnderTest.AddContract(null, 1233, 665, "qualquer");

            //Update Existent
            var actual21 = SystemUnderTest.Contracts.Take(1).First();
            SystemUnderTest.AddContract(actual21.Id, 99, 34455, "credit");

            Assert.Equal(3, SystemUnderTest.Contracts.Count);
            //get contract from organization
            var actual = SystemUnderTest.Contracts.First();

            //assert contract created is the same in organization
            Assert.Equal(actual21.Id, actual.Id);
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }

        [Fact]
        public void RemoveOrganizationContractWhithSoftDeleteFalseById_ThenActiveContract_ShouldBeFalse()
        {
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 95, 6652, "qualquer");
            var actual21 = SystemUnderTest.Contracts.First();

            //not softdelete
            SystemUnderTest.RemoveContract(actual21.Id, false);

            Assert.False(SystemUnderTest.Contracts.Contains(actual21),
                "Should 'not contain' the contract softdelete = false.");

            Assert.Equal(1, SystemUnderTest.Contracts.Count);
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
        }

        /// <summary>
        /// When Contract is disable all configuration should also be desable
        /// </summary>
        [Fact]
        public void RemoveOrganizationContractWhithSoftDeleteTrueById_ThenConfigurationDetailRelated_ShouldNotBeActive()
        {
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 95, 6652, "qualquer");

            var contract = SystemUnderTest.Contracts.First();
            var config = contract.ContractDetailsConfigs.Where(x => x.ContractId == contract.Id).FirstOrDefault();

            //softdelete
            SystemUnderTest.RemoveContract(contract.Id, true);

            Assert.True(SystemUnderTest.Contracts.Contains(contract), "Should 'contain' the contract softdelete = true.");

            //
            Assert.Equal(2, SystemUnderTest.Contracts.Count);
            Assert.Equal(1, SystemUnderTest.ContractDetails.Count);

            //contract                        
            Assert.False(contract.Active);
            Assert.True(contract.Deleted);
            Assert.NotNull(contract.ModifiedBy);
            Assert.NotNull(contract.ModifiedDate);

            //configuration 
            Assert.False(config.Active);
            Assert.True(config.Deleted);
            Assert.NotNull(config.ModifiedBy);
            Assert.NotNull(config.ModifiedDate);
            Assert.Same(config.ContractId, contract.Id);

            //organization
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);

        }

        [Fact]
        public void RemoveOrganizationContractWhithSoftDeleteTrueById_ThenContractIsRemoved()
        {
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 95, 6652, "qualquer");
            var actual21 = SystemUnderTest.Contracts.First();

            SystemUnderTest.RemoveContract(actual21.Id, true);

            Assert.True(SystemUnderTest.Contracts.Contains(actual21),
                "Should not contain this contract after.");

            Assert.Equal(2, SystemUnderTest.Contracts.Count);
            Assert.False(actual21.Active);
            Assert.True(actual21.Deleted);
        }

        [Fact]
        public void RemoveOrganizationContractByInvalidId_TheContractsAreNotRemoved()
        {
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 95, 6652, "qualquer");


            SystemUnderTest.RemoveContract("21", false);

            Assert.Equal(2, SystemUnderTest.Contracts.Count);
        }

        [Fact]
        public void RemoveOrganizationContractNullId_TheContractsAreNotRemoved()
        {
            SystemUnderTest.AddContract(null, 99, 6651, "qualquer");
            SystemUnderTest.AddContract(null, 95, 6652, "qualquer");


            SystemUnderTest.RemoveContract(null, false);

            Assert.Equal(2, SystemUnderTest.Contracts.Count);
        }


    }
}