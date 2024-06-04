using License.Common.Constant;
using License.Common.Helper;
using License.Common.Model;
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
                string decodedLicense = Encoding.UTF8.GetString(Convert.FromBase64String(licenseCode));
                string decryptedLicense = EncryptionService.DecryptString(decodedLicense, encryptionKey);
                var licenseData = JsonSerializer.Deserialize<LicenseKeyGeneratorModel>(decryptedLicense);

                if (licenseData == null)
                {
                    return JsonSerializer.Serialize(new LicenseKeyResponse(false, 0, "", "Invalid license for this machine."));
                }
                DateTime validityDate = DateTime.ParseExact(licenseData.ExpiryDate, "dd/MM/yyyy", null);
                clsComputerInfo hw = new clsComputerInfo();
                string hwId = hw.GetHardwareId();
                string hashedHwId = hw.GenerateSHA512String(hwId);


                if (licenseData.RequestKey != hashedHwId)
                {
                    return JsonSerializer.Serialize(new LicenseKeyResponse(false, 0, "", "Invalid license for this machine."));
                }

                if (validityDate < DateTime.Now)
                {
                    return JsonSerializer.Serialize(new LicenseKeyResponse(false, 0, "", "License has expired."));
                }

                return JsonSerializer.Serialize(new LicenseKeyResponse(true, licenseData.NumberOfLicenses, licenseData.ExpiryDate, ""));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new LicenseKeyResponse(false, 0, "", $"Error: {ex.Message}"));
            }
        }


    }
}
