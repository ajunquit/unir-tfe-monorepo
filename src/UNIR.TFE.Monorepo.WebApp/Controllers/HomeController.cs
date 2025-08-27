using UNIR.TFE.Monorepo.WebApp.Models;
using UNIR.TFE.Monorepo.WebApp.Models.Calculator;
using UNIR.TFE.Monorepo.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UNIR.TFE.Monorepo.WebApp.Controllers
{
    public class HomeController(
        ICalculatorService calculatorService, 
        ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ICalculatorService _calculatorService = calculatorService;
        public CalculatorModel _calculatorModel { get; set; }

        public IActionResult Index()
        {
            return View(new CalculatorModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Calculate(CalculatorModel model)
        {
            try
            {
                model.Result = model.Operation switch
                {
                    "Addition" => _calculatorService.Addition(model.Number1, model.Number2),
                    "Subtraction" => _calculatorService.Subtraction(model.Number1, model.Number2),
                    "Multiplication" => _calculatorService.Multiplication(model.Number1, model.Number2),
                    "Division" => _calculatorService.Division(model.Number1, model.Number2),
                    _ => throw new InvalidOperationException("Operación no válida")
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogError(ex, "Error en cálculo");
            }

            return View("Index", model);
        }
    }
}
