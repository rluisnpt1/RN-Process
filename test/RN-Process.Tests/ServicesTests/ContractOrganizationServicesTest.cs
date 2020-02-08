using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
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

        private IContractOrganizationDataServices _sut;
        private InMemoryRepository<Organization> _repository;
        //private ContractOrganizationServices _sut;
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

        private IContractOrganizationDataServices SystemUnderTest =>
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

            Action act = () => SystemUnderTest.CreateContractOrganization(new ContractOrganization()).Wait();
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task GivenIHaveAnAlreadyExistingOrganization_WhenISaveThrowException()
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
              await  SystemUnderTest.CreateContractOrganization(userImput);
            }
            catch (Exception exception)
            {
                gotException = true;
                Assert.Contains("Can not save Organization", exception.Message);
            }

            gotException.Should().Be(true, "Didn't get exception.");
        }


        [Fact]
        public async Task GivenIHaveAnUnicOrganization_WhenISaveShouldBeAddinDataBase()
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

           await SystemUnderTest.CreateContractOrganization(uniqOrg);
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

            var actual = SystemUnderTest.Search("Nova Org",null);

            actual.Count().Should().BeGreaterOrEqualTo(2);

            var lastNames =
                (from temp in actual
                    select temp.Description).ToList();

            lastNames.Should().Contain("Nova Org");
            lastNames.Should().Contain("Nova Org 3");
        }

        [Fact]
        public async Task OrganizationServerRepositorySincronization()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization_2();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.AddAsync(organization);

            //act
            var sut = SystemUnderTest.OrganizationSyncRepositories(organization.Id).Result;

            //asset 
            sut.Should().BeTrue();
        }
        
        [Fact]
        public async Task OrganizationServerRepositorySincronization_throwException_when_invalidFTP_type()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.AddAsync(organization);

            var sut = Assert.Throws<AggregateException>(() => (bool)SystemUnderTest.OrganizationSyncRepositories(organization.Id).Result);
            //assert
            Assert.Contains("ERROR: Method available only for FTP", sut.Message);
        }

        [Fact]
        public async Task OrganizationServerRepositorySincronization_should_have_lastchangeddate_equal()
        {
            // Arrange

            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization_2();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.AddAsync(organization);

            //act
            bool sut = (bool)SystemUnderTest.OrganizationSyncRepositories(organization.Id).Result;
            Assert.True(sut);
        }

        [Fact]
        public async Task OrganizationServerRepositorySincronization_ShouldThrowExceptionIf_ParamEmpty()
        {
            // Arrange
            //representation of data in database
            var organization = UnitTestUtility.GetCompleteOrganization();

            //breaking dependence of our database. using fake in memory repository.
            await RepositoryInstance.AddAsync(organization);
            var sut = Assert.Throws<AggregateException>(() => (bool)SystemUnderTest.OrganizationSyncRepositories(string.Empty).Result);
            //assert
            Assert.Contains("Required input 'ORGANIZATIONID' was empty", sut.Message);
        }


        //Add the file in the underlying request object.
        //private ControllerContext RequestWithFile()
        //{
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        //    var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
        //    httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
        //    var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
        //    return new ControllerContext(actx);
        //}
    }
}