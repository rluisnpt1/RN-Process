using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
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
            var toValue = new Organization(string.Empty, fromValue.Description, fromValue.CodOrg);
            // Act
            SystemUnderTest.Adapt(fromValue, toValue);

            // Assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [Fact]
        public void AdaptFileDataContractToOrganizationFile()
        {
            // Arrange
            var contract = UnitTestUtility.GetContractOrganizationModel();
            var organization = new Organization(string.Empty, contract.Description, contract.CodOrg);
            // Act
            SystemUnderTest.Adapt(contract, organization);

          var config = organization.TermDetails.Select(x => x.TermDetailConfigs.Select(s => s).FirstOrDefault()).FirstOrDefault();

            var fromValue = UnitTestUtility.GetFileDataContract("BBEE");
          
            // Act
            SystemUnderTest.AdaptOrganizationFile(fromValue, config);

            config.OrganizationFiles.Count.Should().BeGreaterThan(0);
            // Assert
            // UnitTestUtility.AssertAreEqual(fromValue, toValue);
            // UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }
    }
}
