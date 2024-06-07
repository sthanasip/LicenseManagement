using LicenseKeyGeneratorWPF.Constants;
using LicenseKeyGeneratorWPF.Helpers;
using LicenseKeyGeneratorWPF.Records;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LicenseClass;

namespace LicenseKeyGeneratorWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DEBUG START ==> DELETE THIS IN PRODUCTION
            var key = new LicKey();
            txtRequestKey.Text = key.GenerateRequestKey();
            //END
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string requestKey = txtRequestKey.Text;
            string numberOfConnectionsText = txtNumberOfConnections.Text;
            string expiryDate = txtExpiryDate.Text;
            int numberOfConnections = default;

            if (!ValidateForm(requestKey, numberOfConnectionsText, expiryDate, ref numberOfConnections))
            {
                return;
            }
            try
            {
                txtLicenseCode.Text = GenerateLicenseCode(requestKey, numberOfConnections, expiryDate);
            }
            catch
            {
                MessageBox.Show("Invalid Request Key.", AppConstants.InvalidInputType, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool ValidateForm(string requestKey, string numberOfConnectionsText, string expiryDate, ref int numberOfConnections)
        {
            if (string.IsNullOrEmpty(requestKey))
            {
                return DisplayMessageBox("Request Key cannot be empty", AppConstants.InvalidInputType);
            }

            if (!int.TryParse(numberOfConnectionsText, out numberOfConnections) || numberOfConnections < AppConstants.MinimumNumberOfLicenses)
            {
                return DisplayMessageBox("Number of Connections must be a valid integer and greater than 0.", AppConstants.InvalidInputType);
            }

            if (ExpiryDateIsNotFutureDate(expiryDate))
            {
                return DisplayMessageBox("License expiry date must be later than current date", AppConstants.InvalidDateType);
            }

            return true;
        }

        private bool DisplayMessageBox(string message, string type)
        {
            MessageBox.Show(message, type, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) => e.Handled = Regex.IsMatch(e.Text, AppConstants.NumberPattern);

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtLicenseCode.Text);
            MessageBox.Show("Text copied to clipboard!", "Copy Text", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DateTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string text = textBox.Text.Replace("/", string.Empty); // Remove existing slashes
                if (text.Length > AppConstants.LengthOfDate) // Limit the length to 8 characters (ddMMyyyy)
                {
                    text = text.Substring(0, AppConstants.LengthOfDate);
                }

                // Insert slashes after every 2 digits
                for (int i = AppConstants.IndexOfDays; i < text.Length; i += 3)
                {
                    if (i <= AppConstants.IndexAfterMonth)
                    {
                        text = text.Insert(i, "/");
                    }
                }

                // Validate day and month
                if (text.Length >= AppConstants.IndexOfDays && !IsValidDay(text.Substring(0, AppConstants.IndexOfDays)))
                {
                    textBox.Text = text.Substring(0, 1);
                    textBox.CaretIndex = textBox.Text.Length;
                    return;
                }

                if (text.Length >= AppConstants.IndexAfterMonth && !IsValidMonth(text.Substring(3, 2)))
                {
                    textBox.Text = text.Substring(0, 4);
                    textBox.CaretIndex = textBox.Text.Length;
                    return;
                }

                textBox.Text = text;
                textBox.CaretIndex = textBox.Text.Length; // Move caret to the end
            }
        }

        private bool IsValidDay(string dayText)
        {
            if (int.TryParse(dayText, out int day))
            {
                return day >= AppConstants.MinimumDaysAndMonth && day <= AppConstants.MaxNumberOfDays;
            }
            return false;
        }

        private bool IsValidMonth(string monthText)
        {
            if (int.TryParse(monthText, out int month))
            {
                return month >= AppConstants.MinimumDaysAndMonth && month <= AppConstants.MaxNumberOfMonth;
            }
            return false;
        }

        private bool ExpiryDateIsNotFutureDate(string date)
        {
            var expiryDate = DateTime.ParseExact(date, "dd/MM/yyyy", null); ;
            return expiryDate.Date < DateTime.UtcNow.AddDays(1).Date;
        }

        private string GenerateLicenseCode(string requestKey, int numberOfConnections, string expiryDate)
        {
            //decrypt key to get hashed value
            var key = Encoding.UTF8.GetString(Convert.FromBase64String(requestKey));
            var decryptedKey = EncryptionService.DecryptString(key, AppConstants.EncryptionKey);

            // encrypt key and license details
            var detailsToEncryptModel = new LicenseKeyGeneratorModel(decryptedKey, numberOfConnections, expiryDate);
            string detailsToEncrypt = JsonSerializer.Serialize(detailsToEncryptModel);
            var encryptedKey = EncryptionService.EncryptString(detailsToEncrypt, AppConstants.EncryptionKey);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedKey));
        }

    }
}