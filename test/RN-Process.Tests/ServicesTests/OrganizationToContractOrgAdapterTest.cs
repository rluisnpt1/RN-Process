using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.Api.Services;
using Xunit;
using Xunit.Abstractions;

namespace RN_Process.Tests.ServicesTests
{
    public class OrganizationToContractOrgAdapterTest : IDisposable
    {
        private ITestOutputHelper _testOutput;

        public void Dispose()
        {
            _sut = null;
        }

        public OrganizationToContractOrganizationAdapter _sut;

        public OrganizationToContractOrgAdapterTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        public OrganizationToContractOrganizationAdapter SystemUnderTest => _sut ??=
            _sut = new OrganizationToContractOrganizationAdapter();


        [Fact]
        public void AdaptOrganizationToContractOrgnization()
        {
            // Arrange
            var fromValue = UnitTestUtility.GetCompleteOrganization();
            var toValue = new ContractOrganization();

            // Act
            SystemUnderTest.Adapt(fromValue, toValue);

            // Assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }


        [Fact]
        public void AdaptContractOrgnizationToOrganization()
        {
            // Arrange
            var fromValue = UnitTestUtility.GetContractOrganizationModel();
            // Act
            SystemUnderTest.Adapt(fromValue,fromValue.Description,fromValue.CodOrg);

            var toValue = new Organization(fromValue.Description, fromValue.CodOrg);

            // Assert
            UnitTestUtility.AssertAreEqual(fromValue,toValue);
        }
    }
}
