using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;

namespace RN_Process.WebUi.Controllers
{
    [ApiController]
    [Route("api/contractduedetail")]
    [Produces("application/json", "application/xml")]
    public class ContractDueDetailController : ControllerBase
    {
        private readonly IDueDetailDataService _service;

        public ContractDueDetailController(IDueDetailDataService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractduedetailid"></param>
        /// <returns></returns>
        [HttpGet("{contractduedetailid}", Name = "contractduedetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/vnd.marvin.duedetail+json")]
        public async Task<ActionResult<IEnumerable<DueDetail>>> GetContractDueDetail(string contractduedetailid)
        {
            if (string.IsNullOrWhiteSpace(contractduedetailid))
            {
                return BadRequest();
            }

            var returnRepo = await _service.GetDueDetailById(contractduedetailid);

            if (returnRepo == null) return NotFound();

            //var data = JsonConvert.SerializeObject(returnRepo);
            return Ok(returnRepo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractduedetailid"></param>
        /// <returns></returns>
        [HttpGet("duedetailconfiguration/{contractduedetailid}", Name = "duedetailconfiguration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/vnd.marvin.duedetailconfiguration+json")]
        public async Task<ActionResult<IEnumerable<DueDetailConfiguration>>> GetContractDueDetailConfiguration(string contractduedetailid)
        {
            if (string.IsNullOrWhiteSpace(contractduedetailid))
            {
                return BadRequest();
            }

            var returnRepo = await _service.GetDueDetailConfigurationByDueDetailId(contractduedetailid);

            if (returnRepo == null) return NotFound();

            //var data = JsonConvert.SerializeObject(returnRepo);
            return Ok(returnRepo);
        }
    }
}