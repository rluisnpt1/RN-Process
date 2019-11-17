using System;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Shared.Enums;
using Xunit;

namespace RN_Process.Tests.DataAccessTests
{
    public class OrganizationFixture : IDisposable
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
        public void RemoveOrganizationTermByInvalidId_TheTermsAreNotRemoved()
        {
            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 95, 6652, TermsType.Loan, UnitTestUtility.TermDetailIdNull());


            SystemUnderTest.RemoveTerms("21");

            Assert.Equal(2, SystemUnderTest.Terms.Count);
        }

        [Fact]
        public void RemoveOrganizationTermNullId_TheTermsAreNotRemoved()
        {
            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 95, 6652, TermsType.Loan, UnitTestUtility.TermDetailIdNull());


            SystemUnderTest.RemoveTerms(null);

            Assert.Equal(2, SystemUnderTest.Terms.Count);
        }

        //[Fact]
        //public void RemoveOrganizationTermWhithSoftDeleteFalseById_ThenActiveTerm_ShouldBeFalse()
        //{
        //    SystemUnderTest.AddTerm(null, 99, 6651);
        //    SystemUnderTest.AddTerm(null, 95, 6652);
        //    var actual21 = SystemUnderTest.Terms.First();

        //    //not softdelete
        //    SystemUnderTest.RemoveTerms(actual21.Id, false);

        //    Assert.False(SystemUnderTest.Terms.Contains(actual21),
        //        "Should 'not contain' the term softdelete = false.");

        //    Assert.Equal(1, SystemUnderTest.Terms.Count);
        //    Assert.True(SystemUnderTest.Active);
        //    Assert.False(SystemUnderTest.Deleted);
        //}

        /// <summary>
        ///     When Term is disable all configuration should also be desable
        /// </summary>
        [Fact]
        public void RemoveOrganizationTermWhithSoftDeleteTrueById_ThenConfigurationDetailRelated_ShouldNotBeActive()
        {
            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 95, 6652, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            var term = SystemUnderTest.Terms.First();
            var config = term.TermDetails.FirstOrDefault(x => x.TermId == term.Id);

            //softdelete
            SystemUnderTest.RemoveTerms(term.Id);

            Assert.True(SystemUnderTest.Terms.Contains(term),
                "Should 'contain' the term softdelete = true.");

            //
            Assert.Equal(2, SystemUnderTest.Terms.Count);
            Assert.Equal(1, SystemUnderTest.TermDetails.Count);

            //term                        
            Assert.False(term.Active);
            Assert.True(term.Deleted);
            Assert.NotNull(term.ModifiedBy);
            Assert.NotNull(term.ModifiedDate);

            //configuration 
            Assert.False(config.Active);
            Assert.True(config.Deleted);
            Assert.NotNull(config.ModifiedBy);
            Assert.NotNull(config.ModifiedDate);
            Assert.Same(config.TermId, term.Id);

            //organization
            Assert.True(SystemUnderTest.Active);
            Assert.False(SystemUnderTest.Deleted);
        }

        [Fact]
        public void RemoveOrganizationTermWhithSoftDeleteTrueById_ThenTermIsRemoved()
        {
            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 95, 6652, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            var actual21 = SystemUnderTest.Terms.First();

            SystemUnderTest.RemoveTerms(actual21.Id);

            Assert.True(SystemUnderTest.Terms.Contains(actual21),
                "Should not contain this term after.");

            Assert.Equal(2, SystemUnderTest.Terms.Count);
            Assert.False(actual21.Active);
            Assert.True(actual21.Deleted);
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

        /// <summary>
        ///     When term is
        /// </summary>
        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddTerm_TheTermTermIsCreated()
        {
            //act
            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            //has term
            Assert.Equal(1, SystemUnderTest.Terms.Count);

            //has detail
            Assert.Equal(1, SystemUnderTest.TermDetails.Count);



            //get term from organization
            var actualActualTerm = SystemUnderTest.Terms.First();
            var actualTermDetail = SystemUnderTest.TermDetails.First();
            var actualTermConfig = actualTermDetail.TermDetailConfigs.First();


            //assert term created is the same in organization
            Assert.Same(SystemUnderTest.Id, actualActualTerm.OrganizationId);
            Assert.Same(SystemUnderTest, actualActualTerm.Organization);
            Assert.Same(SystemUnderTest.OrgCode, actualActualTerm.OrgCode);
            Assert.Same(actualActualTerm.Id, actualTermDetail.TermId);

            //organization exist
            Assert.Equal(SystemUnderTest.OrgCode, actualTermDetail.OrgCode);
           
            //term exist
            Assert.Equal(1, actualActualTerm.TermDetails.Count);
            Assert.True(actualTermDetail.Active);
            Assert.False(actualTermDetail.Deleted);

            //llist Configuration exist in termdetail
            Assert.Equal(1, actualTermDetail.TermDetailConfigs.Count);

            Assert.Same(actualTermConfig.TermDetailId, actualTermDetail.Id);
            Assert.True(actualTermConfig.Active);
            Assert.False(actualTermConfig.Deleted);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddTerm_ThenTermIsAddedToCollection_StringBasedTerm()
        {
            //act

            SystemUnderTest.AddTerm(null, 99, 665, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            Assert.Equal(1, SystemUnderTest.Terms.Count);
            //get term from organization
            var actual = SystemUnderTest.Terms.First();

            //assert term created is the same in organization
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddTerm_ThenTermIsAddedToCollection_andTerms_isNotnull_StringBasedTerm()
        {
            //act

            SystemUnderTest.AddTerm(null, 99, 665, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            Assert.Equal(1, SystemUnderTest.Terms.Count);
            //get term from organization
            var actual = SystemUnderTest.Terms.First();

            //exist terms
            var termesInCollection = SystemUnderTest.Terms.Where(x => x.TermDetails.Count > 0);

            //assert term created is the same in organization
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
            Assert.Single(termesInCollection);
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddTermIsCalledWithNonZeroIdThenTermIsModified_DateRangeTerm()
        {
            //act

            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 99, 665, TermsType.Loan, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 1233, 665, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            //Update Existent
            var actual21 = SystemUnderTest.Terms.Take(1).First();
            SystemUnderTest.AddTerm(actual21.Id, 99, 34455, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            Assert.Equal(3, SystemUnderTest.Terms.Count);
            //get term from organization
            var actual = SystemUnderTest.Terms.First();

            //assert term created is the same in organization
            Assert.Equal(actual21.Id, actual.Id);
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WhenOrganizationAddTermTwice_TheTermIsEqualTwo()
        {
            //act

            SystemUnderTest.AddTerm(null, 99, 6651, TermsType.CreditCards, UnitTestUtility.TermDetailIdNull());
            SystemUnderTest.AddTerm(null, 99, 665, TermsType.Loan, UnitTestUtility.TermDetailIdNull());

            Assert.Equal(2, SystemUnderTest.Terms.Count);
            //get term from organization
            var actual = SystemUnderTest.Terms.First();

            //assert term created is the same in organization
            Assert.Same(SystemUnderTest.Id, actual.OrganizationId);
            Assert.Same(SystemUnderTest, actual.Organization);
        }
    }
}