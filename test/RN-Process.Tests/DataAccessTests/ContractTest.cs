using System;
using RN_Process.Api.DataAccess.Entities;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class ContractTest : IDisposable
    {
        private const int TypeDebt = 003344;
        private const string NameDebt = "need to pay";

        public void Dispose()
        {
            _sut = null;
        }

        private Contract _sut;
        private Contract SystemUnderTest => _sut ?? UnitTestUtility.GetContractCustomerToTest();

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractIsValid()
        {
            Assert.NotNull(SystemUnderTest);
          //  Assert.NotNull(SystemUnderTest.Id);
         //   Assert.IsType<string>(SystemUnderTest.Id);
            Assert.Empty(SystemUnderTest.CreatedBy);
            Assert.Empty(SystemUnderTest.ModifiedBy);
            Assert.Null(SystemUnderTest.ModifiedDate);
            UnitTestUtility.DateTimeAssertAreEqual(DateTime.UtcNow, SystemUnderTest.CreatedDate, TimeSpan.FromMinutes(0.1));

            Assert.NotNull(SystemUnderTest.Customer);
            Assert.NotEqual(0, SystemUnderTest.ContractNumber);
            Assert.NotEqual(0, SystemUnderTest.TypeDebt);
            Assert.NotEmpty(SystemUnderTest.DebtDescription);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_ContractNumberZero_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentException>(() => ContractInit(0, TypeDebt, NameDebt, UnitTestUtility.GetBancoPortugalCustomerToTest()));

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
            var ex = Assert.Throws<ArgumentException>(() => ContractInit(14533686, 0, NameDebt, UnitTestUtility.GetBancoPortugalCustomerToTest()));

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

        private static Contract ContractInit(int contractNumber, int typeDebt, string nameDebt, Customer customer)
        {
            return new Contract(contractNumber, typeDebt, nameDebt, customer);
        }
    }
}
