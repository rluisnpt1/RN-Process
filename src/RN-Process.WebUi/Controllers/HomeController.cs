using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.DataAccess.Repositories;
using RN_Process.WebUi.Models;

namespace RN_Process.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IContractRepository _productService;

        public HomeController(ILogger<HomeController> logger, IContractRepository productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var ct = new Contract(5454, 55, "Consumo", new Organization("Banco BB", "BBB"));
            var ss = _productService.SaveOneAsync(ct);
            var s2 = _productService.GetAllAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}