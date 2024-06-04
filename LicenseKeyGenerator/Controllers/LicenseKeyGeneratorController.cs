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
            var licKey = new LicKey.LicKey();
            return View(new LicenseKeyModel { RequestKey = licKey.GenerateRequestKey(), ExpiryDate = string.Empty });
        }

        [HttpPost]
        public IActionResult GenerateCode(LicenseKeyModel model)
        {
            string licenseKeyCode = _licenseKeyGeneratorService.GenerateLicenseKeyCode(model);
            return new OkObjectResult(licenseKeyCode);
        }

    }
}
