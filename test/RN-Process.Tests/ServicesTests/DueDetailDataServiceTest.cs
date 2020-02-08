using System;
using ApprovalUtilities.Utilities;
using FluentAssertions;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Api.Services;
using Xunit;

namespace RN_Process.Tests.ServicesTests
{
   public class DueDetailDataServiceTest :IDisposable
    {
        public void Dispose()
        {
            _sut = null;
            _repository = null;
        }

        private IDueDetailDataService _sut;
        private InMemoryRepository<TermDetail> _repository;

        private MockDueDetailStrategy _validatorStrategyInstance;

        private MockDueDetailStrategy ValidatorStrategyInstance
        {
            get { return _validatorStrategyInstance ??= new MockDueDetailStrategy(); }
        }

        private InMemoryRepository<TermDetail> RepositoryInstance =>
            _repository ??= _repository = new InMemoryRepository<TermDetail>();

        //private IDueDetailDataService SystemUnderTest =>
        //    _sut ??= _sut = new DueDetailDataService(RepositoryInstance, ValidatorStrategyInstance);

     

    }
}
