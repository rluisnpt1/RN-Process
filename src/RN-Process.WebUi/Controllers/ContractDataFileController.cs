using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;

namespace RN_Process.WebUi.Controllers
{

    [ApiController]
    [Route("api/contractdatafile")]
    [Produces("application/json", "application/xml")]
    public class ContractDataFileController : ControllerBase
    {
        private readonly IContractFileDataService _service;

        public ContractDataFileController(IContractFileDataService service)
        {
            _service = service;
        }

        /// <summary>
        ///     Get a list of FileDataContract by cod Org
        /// </summary>
        /// <param name="codorg">uniq code org of five digits </param>
        /// <returns>An ActionResult of type IEnumerable of File Data Contract</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FileDataContract>>> GetFileDataContracts(string codorg)
        {
            if (string.IsNullOrWhiteSpace(codorg))
            {
                return BadRequest();
            }
            var returnRepo = await _service.GetOrganizationFileByOrgCod(codorg);

            return Ok(returnRepo);
        }

        /// <summary>
        ///     Get the FileDataContract by id for a specific File
        /// </summary>
        /// <param name="fileDataId">The id of the File</param>
        /// <returns>An ActionResult of type file data contract</returns>
        /// <response code="200">Returns the requested filedatacontract</response>
        [HttpGet("{fileDataId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ContractOrganization>> GetFileDataContract(string fileDataId)
        {
            if (string.IsNullOrWhiteSpace(fileDataId))
            {
                return BadRequest();
            }

            var returnRepo = await _service.GetOrganizationFileById(fileDataId);
          
            if (returnRepo == null)
            {
                return NotFound();
            }

            var data = JsonConvert.SerializeObject(returnRepo);
            return Ok(data);
        }
    }
}