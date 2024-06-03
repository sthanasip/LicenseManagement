using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LicKey
{
    public class LicKey
    {
        private const string encryptionKey = "HardCodedEncryptionKey123!"; // Change this key as per your requirements

        public string GenerateRequestKey()
        {
            clsComputerInfo hw = new clsComputerInfo();
            string hwId = hw.GetHardwareId();
            string hashedHwId = hw.GenerateSHA512String(hwId);
            string encryptedHwId = EncryptString(hashedHwId, encryptionKey);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedHwId));
        }

        public string LicVerification(string licenseCode)
        {
            try
            {
                string decryptedLicenseCode = DecryptString(Encoding.UTF8.GetString(Convert.FromBase64String(licenseCode)), encryptionKey);

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

        private static string EncryptString(string plainText, string key)
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

        private static string DecryptString(string cipherText, string key)
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
    }
}
