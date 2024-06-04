using LicenseKeyGenerator.Models;

namespace LicenseKeyGenerator.Service
{
    public interface ILicenseKeyGeneratorService
    {
        string GenerateLicenseKeyCode(LicenseKeyModel model);
    }
}
