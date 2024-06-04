using LicenseKeyGenerator.Models;
using LicenseKeyGenerator.Service;
using Microsoft.AspNetCore.Mvc;

namespace LicenseKeyGenerator.Controllers
{
    public class LicenseKeyGeneratorController : Controller
    {
        private readonly ILogger<LicenseKeyGeneratorController> _logger;
        private readonly ILicenseKeyGeneratorService _licenseKeyGeneratorService;

        public LicenseKeyGeneratorController(ILogger<LicenseKeyGeneratorController> logger, 
                                            ILicenseKeyGeneratorService licensekeyGeneratorService)
        {
            _logger = logger;
            _licenseKeyGeneratorService = licensekeyGeneratorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateKey(LicenseKeyModel model)
        {
            string licenseKeyCode = _licenseKeyGeneratorService.GenerateLicenseKeyCode(model);
            return PartialView();
        }
    }
}
