using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace LicenseKeyGeneratorWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string requestKey = txtRequestKey.Text;
            string numberOfConnectionsText = txtNumberOfConnections.Text;
            string expiryDate = txtExpiryDate.Text;

            if (!ValidateExpiryDate(expiryDate))
            {
                MessageBox.Show("Date must be in the format dd/MM/yyyy.", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(numberOfConnectionsText, out int numberOfConnections))
            {
                MessageBox.Show("Number of Connections must be a valid integer.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string licenseCode = GenerateLicenseCode(requestKey, numberOfConnections, expiryDate);
            txtLicenseCode.Text = licenseCode;
        }

        private bool ValidateExpiryDate(string date)
        {
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([0-9]{4})$";
            return Regex.IsMatch(date, pattern);
        }

        private string GenerateLicenseCode(string requestKey, int numberOfConnections, string expiryDate)
        {
            // Your logic to generate the license code using requestKey, numberOfConnections, and expiryDate
            // Here we simply return a placeholder string
            return "success";
            //string encryptedData = EncryptData(requestKey, numberOfConnections, expiryDate);
            //return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedData));
        }

        private string EncryptData(string requestKey, int numberOfConnections, string expiryDate)
        {
            string dataToEncrypt = $"{{\"RequestKey\":\"{requestKey}\",\"NumberOfLicenses\":{numberOfConnections},\"ExpiryDate\":\"{expiryDate}\"}}";
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("A-very-secure-key");
                aesAlg.IV = Encoding.UTF8.GetBytes("An-initialization-");

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(dataToEncrypt);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
    }
}


//using LicenseKeyGeneratorWPF.ValidationRules;
//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;

//namespace LicenseKeyGeneratorWPF
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//            DataContext = this;
//        }

//        private void btnGenerate_Click(object sender, RoutedEventArgs e)
//        {
//            if (ValidateInputs())
//            {
//                string requestKey = txtRequestKey.Text;
//                string numberOfConnectionsText = txtNumberOfConnections.Text;
//                string expiryDate = txtExpiryDate.Text;

//                int numberOfConnections = int.Parse(numberOfConnectionsText);
//                string licenseCode = GenerateLicenseCode(requestKey, numberOfConnections, expiryDate);
//                txtLicenseCode.Text = licenseCode;
//            }
//            else
//            {
//                MessageBox.Show("Please fix the validation errors.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private bool ValidateInputs()
//        {
//            bool isRequestKeyValid = !string.IsNullOrEmpty(txtRequestKey.Text);
//            bool isNumberOfConnectionsValid = string.IsNullOrEmpty(txtNumberOfConnections.Text) || !Validation.GetHasError(txtNumberOfConnections);
//            bool isExpiryDateValid = string.IsNullOrEmpty(txtExpiryDate.Text) || !Validation.GetHasError(txtExpiryDate);

//            return isRequestKeyValid && isNumberOfConnectionsValid && isExpiryDateValid;
//        }


//        private string GenerateLicenseCode(string requestKey, int numberOfConnections, string expiryDate)
//        {
//            string encryptedData = EncryptData(requestKey, numberOfConnections, expiryDate);
//            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedData));
//        }

//        private string EncryptData(string requestKey, int numberOfConnections, string expiryDate)
//        {
//            string dataToEncrypt = $"{{\"RequestKey\":\"{requestKey}\",\"NumberOfLicenses\":{numberOfConnections},\"ExpiryDate\":\"{expiryDate}\"}}";
//            using (Aes aesAlg = Aes.Create())
//            {
//                aesAlg.Key = Encoding.UTF8.GetBytes("A-very-secure-key");
//                aesAlg.IV = Encoding.UTF8.GetBytes("An-initialization-");

//                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

//                using (var msEncrypt = new System.IO.MemoryStream())
//                {
//                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
//                    {
//                        using (var swEncrypt = new StreamWriter(csEncrypt))
//                        {
//                            swEncrypt.Write(dataToEncrypt);
//                        }
//                        return Convert.ToBase64String(msEncrypt.ToArray());
//                    }
//                }
//            }
//        }
//    }
//}
