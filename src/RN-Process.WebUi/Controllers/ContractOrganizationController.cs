using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;

namespace RN_Process.WebUi.Controllers
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/organizationcontracts")]
    [Route("api/organizationcontracts")]
    [Produces("application/json", "application/xml")]
    public class ContractOrganizationController : ControllerBase
    {
        private readonly IContractOrganizationDataServices _service;

        public ContractOrganizationController(IContractOrganizationDataServices service)
        {
            _service = service;
        }


        /// <summary>
        ///     Get a list of Organization Contracts
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Organization Contracts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ContractOrganization>>> ContractOrganizations()
        {
            var returnRepo = _service.GetContractOrganizations();
            if (returnRepo == null) return NotFound();
            //var data = JsonConvert.SerializeObject(returnRepo);
            return Ok(returnRepo);
        }

        /// <summary>
        ///     Get the ContractOrganizations by id for a specific Contract
        /// </summary>
        /// <param name="contractId">The id of the Contract</param>
        /// <returns>An ActionResult of type contract</returns>
        /// <response code="200">Returns the requested contractorganization</response>
        [HttpGet("{contractId}/organization")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/vnd.marvin.contractorganization+json")]
        public async Task<ActionResult<ContractOrganization>> ContractOrganization(string contractId)
        {
            if (string.IsNullOrWhiteSpace(contractId))
            {
                return BadRequest();
            }
            var contract = _service.GetContractOrganizationById(contractId);
            if (contract == null) return NotFound();

           // var data = JsonConvert.SerializeObject(contract);
            return Ok(contract);
        }

        /// <summary>
        ///   Get the ContractOrganizations by organization code´for specific organization
        /// </summary>
        /// <param name="codorg"></param>
        /// <returns>An ActionResult of type ContractOrganization</returns>
        /// <response code="200">Returns the requested Contractorganization</response>
        [HttpGet("{codorg}", Name = "ContractOrganizationDetail")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/vnd.marvin.contractorganizationdetail+json")]
        public async Task<ActionResult<ContractOrganization>> ContractOrganizationDetail(string codorg)
        {
            if (string.IsNullOrWhiteSpace(codorg))
            {
                return BadRequest("codorg is required");
            }


            var contract = _service.Search(codorg);
            if (contract == null) return NotFound();


            //var data = JsonConvert.SerializeObject(contract);
            return Ok(contract);
        }
        /// <summary>
        ///     Create a ContractOrganization for a specific Org
        /// </summary>
        /// <param name="contractForCreation">The ContractOrganization to create</param>
        /// <returns>An ActionResult of type ContractOrganization</returns>
        /// <response code="422">Validation error</response>
        [HttpPost]
        [Consumes("application/json", "application/vnd.marvin.contractforcreation+json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<ActionResult<ContractOrganization>> CreateContractOrganization([FromBody] ContractOrganization contractForCreation)
        {
            Guard.Against.Null(contractForCreation, nameof(ContractOrganization));

            await _service.CreateContractOrganization(contractForCreation);

            return CreatedAtRoute("OrganizationContract",
                new { contractId = contractForCreation.Id },
                contractForCreation);
        }


        /// <summary>
        ///    Sync Repository base from client to server Intrum
        /// </summary>
        /// <param name="organizationId">The id of the Contract</param>
        /// <returns>An ActionResult of type contract</returns>
        /// <response code="200">Returns the requested contractorganization</response>
        [HttpGet("syncrepository/{organizationId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/vnd.marvin.syncrepository+json")]
        public async Task<ActionResult<bool>> SyncRepositoryBase(string organizationId)
        {
            if (string.IsNullOrWhiteSpace(organizationId))
            {
                return BadRequest();
            }
            var contract = await _service.OrganizationSyncRepositories(organizationId);
            
            // var data = JsonConvert.SerializeObject(contract);
            return Ok(contract);
        }
    }
}