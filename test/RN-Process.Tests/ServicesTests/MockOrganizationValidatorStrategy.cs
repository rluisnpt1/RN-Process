using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;

namespace RN_Process.Tests.ServicesTests
{
    public class MockOrganizationValidatorStrategy : IValidatorStrategy<ContractOrganization>
    {
        public MockOrganizationValidatorStrategy()
        {
            IsValidReturnValue = true;
        }

        public bool IsValidReturnValue { get; set; }

        public bool IsValid(ContractOrganization validateThis)
        {
            return IsValidReturnValue;
        }
    }

    public class MockDueDetailStrategy : IValidatorStrategy<DueDetail>
    {
        public MockDueDetailStrategy()
        {
            IsValidReturnValue = true;
        }

        public bool IsValidReturnValue { get; set; }

        public bool IsValid(DueDetail validateThis)
        {
            return IsValidReturnValue;
        }
    }

}
