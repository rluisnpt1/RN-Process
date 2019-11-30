using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.WebUi.Controllers
{
    [ApiController] 
    //[Route("api/v{version:apiVersion}/organizationcontracts")]
    [Route("api/organizationcontracts")]
    [Produces("application/json", "application/xml")]
    public class OrganizationContractController : ControllerBase
    {
        private readonly IContractOrganizationDataServices _service;

        public OrganizationContractController(IContractOrganizationDataServices service)
        {
            _service = service;
        }

        /// <summary>
        /// Get a list of Organization Contracts
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Organization Contracts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContractOrganization>>> OrganizationContracts()
        {
            var returnRepo = _service.GetContractOrganizations();

            return Ok(returnRepo);
        }

        /// <summary>
        /// Get the ContractOrganizations by id for a specific Contract
        /// </summary>
        /// <param name="contractId">The id of the Contract</param>
        /// <returns>An ActionResult of type contract</returns> 
        /// <response code="200">Returns the requested contractorganization</response>
        [HttpGet("{contractId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/vnd.marvin.contractorganization+json")]
        public async Task<ActionResult<ContractOrganization>> OrganizationContract(string orgContractId)
        {
            var contract = _service.GetContractOrganizationById(orgContractId);
            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        /// <summary>
        /// Create a ContractOrganization for a specific Org
        /// </summary>
        /// <param name="orgContractId">The id of the ContractOrganization</param>
        /// <param name="contractForCreation">The ContractOrganization to create</param>
        /// <returns>An ActionResult of type ContractOrganization</returns>
        /// <response code="422">Validation error</response>
        [HttpPost()]
        [Consumes("application/json", "application/vnd.marvin.contractForCreation+json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
            Type = typeof(ValidationProblemDetails))]
        public async Task<ActionResult<ContractForCriation>> CreateContractOrganization(
            string orgContractId, [FromBody] ContractOrganization contractForCreation)
        {
        
            Guard.Against.Null(contractForCreation,nameof(ContractOrganization));
            //var contract = new ContractOrganization(contractForCreation.CodOrg,
            //    contractForCreation.Description, contractForCreation.ContractNumber);

            //TermsType parsedLanguage;
            //Enum.TryParse<TermsType>(contractForCreation.TermsType, true, out parsedLanguage);

            //contract.AddDueDetail(contractForCreation.DebtCode, parsedLanguage);

            _service.CreateContractOrganization(contractForCreation);

            return CreatedAtRoute("OrganizationContract",new { orgContractId },
                contractForCreation.Id);
        }
    }
}