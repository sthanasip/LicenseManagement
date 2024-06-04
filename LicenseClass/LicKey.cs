using License.Common.Constant;
using License.Common.Helper;
using System;
using System.Text;
using System.Text.Json;

namespace LicKey
{
    public class LicKey
    {
        private readonly string encryptionKey = KeyConstant.EncryptionKey;
        public string GenerateRequestKey()
        {
            clsComputerInfo hw = new clsComputerInfo();
            string hwId = hw.GetHardwareId();
            string hashedHwId = hw.GenerateSHA512String(hwId);
            string encryptedHwId = EncryptionService.EncryptString(hashedHwId, encryptionKey);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedHwId));
        }

        public string LicVerification(string licenseCode)
        {
            try
            {
                string decryptedLicenseCode = EncryptionService.DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(licenseCode)), encryptionKey);

                clsComputerInfo hw = new clsComputerInfo();
                string currentHwId = hw.GenerateSHA512String(hw.GetHardwareId());

                if (decryptedLicenseCode == currentHwId)
                {
                    var result = new
                    {
                        result = true,
                        license = 20,
                        validity = "31/12/2030",
                        message = ""
                    };
                    return JsonSerializer.Serialize(result);
                }
                else
                {
                    var result = new
                    {
                        result = false,
                        license = 0,
                        validity = "",
                        message = "Hardware ID does not match."
                    };
                    return JsonSerializer.Serialize(result);
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    result = false,
                    license = 0,
                    validity = "",
                    message = $"Error: {ex.Message}"
                };
                return JsonSerializer.Serialize(result);
            }
        }

        
    }
}
