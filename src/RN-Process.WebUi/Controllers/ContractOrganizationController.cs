using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.WebUi.Controllers
{
    public class ContractOrganizationController : Controller
    {
        private readonly IContractOrganizationDataServices _service;
        private readonly IValidatorStrategy<ContractOrganization> _validator;

        public ContractOrganizationController(IValidatorStrategy<ContractOrganization> validator,
            IContractOrganizationDataServices service)
        {
            _validator = validator;
            _service = service;
        }

        // GET: ContractOrganization
        [AllowAnonymous]
        public ActionResult Index()
        {
            var contractOrganization = _service.GetContractOrganizations();

            return View(contractOrganization);
        }

        [AllowAnonymous]
        [Route("/[controller]/[action]/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var contractOrganization = _service.GetContractOrganizationById(id);

            if (contractOrganization == null)
            {
                return NotFound();
            }

            return View(contractOrganization);
        }

        [Route("/contractOrganization/{description:alpha}/{codorg:alpha}")]
        public ActionResult Details(string description, string codorg)
        {
            if (String.IsNullOrWhiteSpace(description) == true ||
                 String.IsNullOrWhiteSpace(codorg) == true)
            {
                return new BadRequestResult();
            }

            var contractOrganization = _service.Search(description, codorg).FirstOrDefault();

            if (contractOrganization == null)
            {
                return NotFound();
            }

            return View("Details", contractOrganization);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit", new { id = "" });
        }

        // GET: ContractOrganization/Create
        public ActionResult Edit(string id, int debtCode, TermsType termsType)
        {
            if (id == null || debtCode == 0)
            {
                return new BadRequestResult();
            }

            ContractOrganization contractOrganization;

            if (string.IsNullOrWhiteSpace(id))
            {
                // create new
                contractOrganization = new ContractOrganization();
                contractOrganization.AddDueDetail(debtCode, termsType);
            }
            else
            {
                contractOrganization = _service.GetContractOrganizationById(id);
            }

            if (contractOrganization == null)
            {
                return NotFound();
            }

            return View(contractOrganization);
        }

        // POST: ContractOrganization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractOrganization contractOrganization)
        {
            if (_validator.IsValid(contractOrganization) == true)
            {
                bool isCreateNew = false;

                if (string.IsNullOrWhiteSpace(contractOrganization.Id))
                {
                    isCreateNew = true;
                }
                else
                {
                    ContractOrganization toValue =
                        _service.GetContractOrganizationById(contractOrganization.Id);

                    if (toValue == null)
                    {
                        return new BadRequestObjectResult(
                            $"Unknown contractOrganization id '{contractOrganization.Id}'.");
                    }
                }

                _service.CreateContractOrganization(contractOrganization);

                if (isCreateNew == true)
                {
                    RedirectToAction("Edit", new { id = contractOrganization.Id });
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }

            return View(contractOrganization);
        }
    }
}