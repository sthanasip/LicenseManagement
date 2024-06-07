using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LicenseClass
{
    public class LicKey
    {
        private readonly string encryptionKey = "HardCodedEncryptionKey123";
        public string GenerateRequestKey()
        {
            clsComputerInfo hw = new clsComputerInfo();
            string hwId = hw.GetHardwareId();
            string hashedHwId = hw.GenerateSHA256String(hwId);
            string encryptedHwId = EncryptString(hashedHwId, encryptionKey);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedHwId));
        }

        public string LicVerification(string licenseCode)
        {
            try
            {
                string decodedLicense = Encoding.UTF8.GetString(Convert.FromBase64String(licenseCode));
                string decryptedLicense = DecryptString(decodedLicense, encryptionKey);
                var licenseData = JsonSerializer.Deserialize<LicenseKeyGeneratorModel>(decryptedLicense);

                if (licenseData == null)
                {
                    return JsonSerializer.Serialize(new LicenseKeyResponse(false, 0, "", "Invalid license for this machine."));
                }
                DateTime validityDate = DateTime.ParseExact(licenseData.ExpiryDate, "dd/MM/yyyy", null);
                clsComputerInfo hw = new clsComputerInfo();
                string hwId = hw.GetHardwareId();
                string hashedHwId = hw.GenerateSHA256String(hwId);

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

        private string EncryptString(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
                aesAlg.IV = new byte[16]; // Zero IV for simplicity. In production, use a random IV.

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private string DecryptString(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
                aesAlg.IV = new byte[16]; // Zero IV for simplicity. In production, use a random IV.

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public record LicenseKeyGeneratorModel(string RequestKey, int NumberOfLicenses, string ExpiryDate);
        public record LicenseKeyResponse(bool result, int license, string validity, string message);
    }
}
