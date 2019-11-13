using System;
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


            Assert.NotNull(SystemUnderTest.UniqCode);
            Assert.NotEmpty(SystemUnderTest.UniqCode);
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
            Assert.Contains("Required input 'UNIQCODE' was empty", ex.Message);
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
            Assert.Contains("Input 'UNIQCODE' was out of range", ex.ParamName);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UniqCodeLessThenThree_ThenSizeNameIsNotValid()
        {
            //act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CustomerInit(Name, "w"));

            //assert
            Assert.Equal("uniqCode (Parameter 'Input 'UNIQCODE' was out of range')", ex.Message);
            Assert.Contains("Input 'UNIQCODE' was out of range", ex.ParamName);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenCreated_UNIQCODEnull_ThenThrowException()
        {
            //act
            var ex = Assert.Throws<ArgumentNullException>(() => CustomerInit(Name, null));

            //assert
            Assert.Contains("Value cannot be null.", ex.Message);
            Assert.Equal("uniqCode", ex.ParamName);
        }
    }
}