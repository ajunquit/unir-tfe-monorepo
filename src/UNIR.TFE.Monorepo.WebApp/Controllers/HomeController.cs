using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UNIR.TFE.Monorepo.WebApp.Infrastructure.External.GitHub;
using UNIR.TFE.Monorepo.WebApp.Models;
using UNIR.TFE.Monorepo.WebApp.Models.Common;
using UNIR.TFE.Monorepo.WebApp.Services;

namespace UNIR.TFE.Monorepo.WebApp.Controllers
{
    public class HomeController(
        ICalculatorService calculatorService,
        ILogger<HomeController> logger,
        IGitRepositoryAnalyzerService gitRepositoryAnalyzerService,
        IConfiguration configuration
        ) : Controller
    {
        private readonly ICalculatorService _calculatorService = calculatorService;
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IGitRepositoryAnalyzerService _gitRepositoryAnalyzerService = gitRepositoryAnalyzerService;
        private readonly IConfiguration _configuration = configuration;

        public CalculatorModel CalculatorModel { get; set; } = new();

        public async Task<IActionResult> Index()
        {
            await LoadGitRepositoryInfo();
            return View(CalculatorModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(CalculatorModel model)
        {
            try
            {
                model.Result = CalculateOperation(model);
                await LoadGitRepositoryInfo(model);
                _logger.LogInformation("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                HandleCalculationError(ex);
            }

            return View("Index", model);
        }

        private decimal CalculateOperation(CalculatorModel model)
        {
            return model.Operation switch
            {
                "Addition" => _calculatorService.Addition(model.Number1, model.Number2),
                "Subtraction" => _calculatorService.Subtraction(model.Number1, model.Number2),
                "Multiplication" => _calculatorService.Multiplication(model.Number1, model.Number2),
                "Division" => _calculatorService.Division(model.Number1, model.Number2),
                _ => throw new InvalidOperationException("Invalid operation")
            };
        }

        private async Task LoadGitRepositoryInfo(CalculatorModel model = null)
        {
            var gitModel = await _gitRepositoryAnalyzerService.AnalyzeRepositoryAsync(
                _configuration.GetValue<string>("Git:Superproject")!,
                _configuration.GetValue<string>("Git:Branch")!);

            if (model == null)
            {
                CalculatorModel.GitModel = gitModel;
            }
            else
            {
                model.GitModel = gitModel;
            }
        }

        private void HandleCalculationError(Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            _logger.LogError(ex, "Calculation error");
        }
    }
}
