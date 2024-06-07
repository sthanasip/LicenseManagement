using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace LicenseClass
{
    internal class clsComputerInfo
    {
        private string GetProcessorId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                {
                    using (var collection = searcher.Get())
                    {
                        foreach (var item in collection)
                        {
                            return item["ProcessorId"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ProcessorId: {ex.Message}");
            }
            return "N/A";
        }

        private string GetVolumeSerial(string driveLetter)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher($"SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE DeviceID = '{driveLetter}:'"))
                {
                    using (var collection = searcher.Get())
                    {
                        foreach (var item in collection)
                        {
                            return item["VolumeSerialNumber"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting VolumeSerialNumber: {ex.Message}");
            }
            return "N/A";
        }

        private string GetMotherboardId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    using (var collection = searcher.Get())
                    {
                        foreach (var item in collection)
                        {
                            return item["SerialNumber"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting MotherboardId: {ex.Message}");
            }
            return "N/A";
        }

        private string GetMacAddress()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT MacAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'"))
                {
                    using (var collection = searcher.Get())
                    {
                        foreach (var item in collection)
                        {
                            return item["MacAddress"]?.ToString().Replace(":", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting MacAddress: {ex.Message}");
            }
            return "N/A";
        }

        public string GenerateSHA512String(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateSHA256String(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert the byte array to a Base64 string
                return Convert.ToBase64String(hashBytes);//.Substring(0, 22); // Truncate to 22 characters
            }
        }

        public string GetHardwareId()
        {
            return GetProcessorId() + GetVolumeSerial("C") + GetMotherboardId() + GetMacAddress();
        }

    }

}
