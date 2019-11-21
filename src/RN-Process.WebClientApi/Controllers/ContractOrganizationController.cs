using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RN_Process.Api.Interfaces;

namespace RN_Process.WebClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractOrganizationController : ControllerBase
    {
        private readonly IContractOrganizationDataServices _services;

        public ContractOrganizationController(IContractOrganizationDataServices services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _services.GetContractOrganizationById("ss");
            return Ok();
        }
    }
}