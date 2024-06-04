using License.Common.Constant;
using License.Common.Helper;
using LicenseKeyGenerator.Models;
using System.Text;
using LicKey;
using System.Text.Json;
using License.Common.Model;

namespace LicenseKeyGenerator.Service
{
    public class LicenseKeyGeneratorService : ILicenseKeyGeneratorService
    {
        private readonly string EncryptionKey = KeyConstant.EncryptionKey;
        public string GenerateLicenseKeyCode(LicenseKeyModel model)
        {
            //decrypt key to get hashed value
            var key = Encoding.UTF8.GetString(Convert.FromBase64String(model.RequestKey));
            var decryptedKey = EncryptionService.DecryptString(key, EncryptionKey);

            // encrypt key and license details
            var detailsToEncryptModel = new LicenseKeyGeneratorModel(decryptedKey,model.NumberOfLicenses,model.ExpiryDate);
            string detailsToEncrypt = JsonSerializer.Serialize(detailsToEncryptModel);
            var encryptedKey = EncryptionService.EncryptString(detailsToEncrypt, EncryptionKey);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedKey));

        }
    }
}
