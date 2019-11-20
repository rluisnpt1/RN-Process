using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;
using Xunit;

namespace RN_Process.Tests.ModelsTests
{
    public class ContractOrganizationValidationStgShould
    {
        private DefaultValidatorStrategy<ContractOrganization> _sut;
        public DefaultValidatorStrategy<ContractOrganization> SystemUnderTest =>
        _sut ??= _sut = new DefaultValidatorStrategy<ContractOrganization>();


        [Fact]
        public void GivenAnDefaultConstructorValidContractThrowException()
        {
            // Arrange
            var newCntr = new ContractOrganization();

            // Act
            var result = SystemUnderTest.IsValid(newCntr);

            // Assert
            result.Should().BeFalse("Because exist required values");
        }

        [Fact]
        public void GivenAllRequiredFildsCoveredThenNewContractIsValid()
        {
            // Arrange
            var newCntr =UnitTestUtility.GetContractOrganizationModel();

            // Act
            var result = SystemUnderTest.IsValid(newCntr);

            // Assert
            result.Should().BeTrue();
        }
    }
}
