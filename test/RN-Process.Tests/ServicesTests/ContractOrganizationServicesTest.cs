using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.Api.Services;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;
using Xunit;

namespace RN_Process.Tests.ServicesTests
{
    public class ContractOrganizationServicesTest : IDisposable
    {
        public void Dispose()
        {
            _sut = null;
            _repository = null;
        }

        private InMemoryRepository<Organization> _repository;
        private ContractOrganizationServices _sut;
        private MockOrganizationValidatorStrategy _validatorStrategyInstance;

        public MockOrganizationValidatorStrategy ValidatorStrategyInstance
        {
            get { return _validatorStrategyInstance ??= new MockOrganizationValidatorStrategy(); }
        }

        private InMemoryRepository<Organization> RepositoryInstance =>
            _repository ??= _repository = new InMemoryRepository<Organization>();

        //private IOrganizationToContractOrganizationAdapter AdapterInstance =>
        //    _adapter ??= _adapter = new OrganizationToContractOrganizationAdapter();

        //private IMongoContext Context => _context ??= _context =
        //    new RnProcessMongoDbContext(InitConfiguration());

        private ContractOrganizationServices SystemUnderTest =>
            _sut ??= _sut = new ContractOrganizationServices(RepositoryInstance, ValidatorStrategyInstance);


        private async void PopulateRepositoryWithTestData()
        {
            await RepositoryInstance.AddAsync(UnitTestUtility.GetNOWO_Organization_OrganizationToTest());
            await RepositoryInstance.AddAsync(UnitTestUtility.GetUNICRE_OrganizationToTest());

            var personWithNoFacts = new Organization(string.Empty, "Nova Org", "@4566");
            await RepositoryInstance.AddAsync(personWithNoFacts);

            var personWhoWasVP = new Organization(string.Empty, "Nova Org 3", "@9898");

            personWhoWasVP.AddTerm(null, 9998655, 87556, TermsType.Leasing, FileAccessType.LocalMachine
                , string.Empty, "\\Test\\teste", "LOCAL", "XML", false, string.Empty,
                string.Empty, string.Empty, string.Empty, "/", string.Empty,
                string.Empty, string.Empty, ",", true, string.Empty,
                RnProcessConstant.AvailableColumnsIntrum, RnProcessConstant.AvailableColumnsIntrum);

            await RepositoryInstance.AddAsync(personWhoWasVP);
        }


        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", false, true)
                .Build();
            return config;
        }

        [Fact]
        public void GetAllOrganizationsOnlyReturnsContractOrganizations()
        {
            PopulateRepositoryWithTestData();

            var actual = SystemUnderTest.GetContractOrganizations();

            Assert.Equal(4, actual.Count());

            var lastNames =
                (from temp in actual
                    select temp.Description).ToList();

            lastNames.Should().Contain("Nova Org");
            lastNames.Should().Contain("Nova Org 3");
        }

        [Fact]
        public void GivenAnInvalidContractWhenSaveIsCalledThenThrowAnException()
        {
            ValidatorStrategyInstance.IsValidReturnValue = false;

            Action act = () => SystemUnderTest.CreateContractOrganization(new ContractOrganization());
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async void GivenIHaveAnAlreadyExistingOrganization_WhenISaveThrowException()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.AddAsync(organization);

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
            await RepositoryInstance.AddAsync(organization);

            //double check
            organization.Id.Should().NotBeNullOrEmpty("Organization do not exist in data base");

            // Act
            //represent de duplication
            var uniqOrg = UnitTestUtility.GetContractOrganizationModel();

            SystemUnderTest.CreateContractOrganization(uniqOrg);
            uniqOrg.Id.Should().NotBeNullOrEmpty("Organization was not saved");
            var fromRepository = _repository.GetByIdAsync(uniqOrg.Id);
            fromRepository.Should().NotBeNull();
        }

        [Fact]
        public void SearchByCodOrg()
        {
            PopulateRepositoryWithTestData();

            var actual = SystemUnderTest.Search(string.Empty, "@9859");

            actual.Count().Should().BeGreaterOrEqualTo(1);

            var lastNames =
                (from temp in actual
                    select temp.Description).ToList();

            lastNames.Should().Contain("Nova Org 3");
        }

        [Fact]
        public void SearchByContractNumber()
        {
            PopulateRepositoryWithTestData();

            var actual =
                SystemUnderTest.Search(string.Empty, string.Empty, 9998655);

            actual.Count().Should().BeGreaterOrEqualTo(1);

            var lastNames =
                (from temp in actual
                    select temp.Description).ToList();

            lastNames.Should().Contain("Nova Org 3");
        }

        [Fact]
        public void SearchByDescription()
        {
            PopulateRepositoryWithTestData();

            var actual = SystemUnderTest.Search("Nova Org");

            actual.Count().Should().BeGreaterOrEqualTo(2);

            var lastNames =
                (from temp in actual
                    select temp.Description).ToList();

            lastNames.Should().Contain("Nova Org");
            lastNames.Should().Contain("Nova Org 3");
        }
    }
}