using LicenseKeyGeneratorWPF.Helpers;
using LicenseKeyGeneratorWPF.Records;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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

        private const string encryptionKey = "HardCodedEncryptionKey123";
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show("Invalid Request Key.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return ;
            }

        }

        private bool ValidateExpiryDate(string date)
        {
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([0-9]{4})$";
            return Regex.IsMatch(date, pattern);
        }

        private string GenerateLicenseCode(string requestKey, int numberOfConnections, string expiryDate)
        {
            //decrypt key to get hashed value
            var key = Encoding.UTF8.GetString(Convert.FromBase64String(requestKey));
            var decryptedKey = EncryptionService.DecryptString(key, encryptionKey);

            // encrypt key and license details
            var detailsToEncryptModel = new LicenseKeyGeneratorModel(decryptedKey, numberOfConnections, expiryDate);
            string detailsToEncrypt = JsonSerializer.Serialize(detailsToEncryptModel);
            var encryptedKey = EncryptionService.EncryptString(detailsToEncrypt, encryptionKey);
            var resp = Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedKey));
            return resp;
        }

    }
}