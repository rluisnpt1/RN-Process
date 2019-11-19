using System;
using FluentAssertions;
using Microsoft.Extensions.Options;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Services;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;
using Xunit;

namespace RN_Process.Tests.ServicesTests
{
    public class ContractOrganizationServicesTest : IDisposable
    {
        private readonly IOptions<MongoDbSettings> _options;
        private IOrganizationToContractOrganizationAdapter _adapter;
        private InMemoryRepository<Organization, string> _repository;
        private ContractOrganizationServices _sut;

        private InMemoryRepository<Organization, string> RepositoryInstance =>
            _repository ??= _repository = new InMemoryRepository<Organization, string>();

        private IOrganizationToContractOrganizationAdapter AdapterInstance =>
            _adapter ??= _adapter = new OrganizationToContractOrganizationAdapter();

        private ContractOrganizationServices SystemUnderTest =>
            _sut ??= _sut = new ContractOrganizationServices(_options, AdapterInstance, RepositoryInstance);

        public void Dispose()
        {
            _sut = null;
            _adapter = null;
            _repository = null;
        }

        [Fact]
        public async void GivenIHaveAnAlreadyExistingOrganization_WhenISaveThrowException()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.SaveOneAsync(organization);

            //double check
            Assert.NotNull(organization.Id);

            // Act
            //represent de duplication
            var userImput = UnitTestUtility.GetContractOrganizationModelTwo();

            bool gotException;
            try
            {
                // Assert
                gotException = false;
                SystemUnderTest.SaveContract(userImput);
            }
            catch (Exception exception)
            {
                gotException = true;
                Assert.Contains("Can not save Organization", exception.Message);
            }

            gotException.Should().Be(true, "Didn't get exception.");
        }


        [Fact]
        public async void GivenIHaveAnUnicOrganization_WhenISaveShouldBeAddinDataBase()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.SaveOneAsync(organization);

            //double check
            organization.Id.Should().NotBeNullOrEmpty("Organization do not exist in data base");

            // Act
            //represent de duplication
            var uniqOrg = UnitTestUtility.GetContractOrganizationModel();

            SystemUnderTest.SaveContract(uniqOrg);
            uniqOrg.Id.Should().NotBeNullOrEmpty("Organization was not saved");
            var fromRepository = _repository.GetOneAsync(uniqOrg.Id);
            fromRepository.Should().NotBeNull();
        }
    }
}