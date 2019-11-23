using System;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using RN_Process.Api.DataAccess;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Services;
using RN_Process.DataAccess.MongoDb;
using Xunit;

namespace RN_Process.Tests.ServicesTests
{
    public class ContractOrganizationServicesTest : IDisposable
    {
        public void Dispose()
        {
            _sut = null;
            _adapter = null;
            _repository = null;
            _context = null;
        }

        private IMongoContext _context;
        private IOrganizationToContractOrganizationAdapter _adapter;
        private InMemoryRepository<Organization, string> _repository;
        private ContractOrganizationServices _sut;

        private InMemoryRepository<Organization, string> RepositoryInstance =>
            _repository ??= _repository = new InMemoryRepository<Organization, string>();

        private IOrganizationToContractOrganizationAdapter AdapterInstance =>
            _adapter ??= _adapter = new OrganizationToContractOrganizationAdapter();

        private IMongoContext Context => _context ??= _context =
            new RnProcessMongoDbContext(InitConfiguration());

        private ContractOrganizationServices SystemUnderTest =>
            _sut ??= _sut = new ContractOrganizationServices(Context, AdapterInstance);

        [Fact]
        public async void GivenIHaveAnAlreadyExistingOrganization_WhenISaveThrowException()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.Add(organization);

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
                SystemUnderTest.CreateContractOrganization(userImput);
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
            await RepositoryInstance.Add(organization);

            //double check
            organization.Id.Should().NotBeNullOrEmpty("Organization do not exist in data base");

            // Act
            //represent de duplication
            var uniqOrg = UnitTestUtility.GetContractOrganizationModel();

            SystemUnderTest.CreateContractOrganization(uniqOrg);
            uniqOrg.Id.Should().NotBeNullOrEmpty("Organization was not saved");
            var fromRepository = _repository.GetById(uniqOrg.Id);
            fromRepository.Should().NotBeNull();
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .Build();
            return config;
        }
    }
}