using LicenseClass;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LicenseRequestKey
{
    public partial class MainWindow : Window
    {
        private LicKey _licKey;

        public MainWindow()
        {
            InitializeComponent();
            _licKey = new LicKey();
        }

        private void GenerateRequestKey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string requestKey = _licKey.GenerateRequestKey();
                RequestKeyTextBox.Text = requestKey;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating request key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerifyLicenseKey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string licenseKey = LicenseKeyTextBox.Text;
                if (string.IsNullOrEmpty(licenseKey))
                {
                    MessageBox.Show("Please enter a license key.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string result = _licKey.LicVerification(licenseKey);
                ResultTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying license key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyRequestKey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(RequestKeyTextBox.Text);
                MessageBox.Show("Request key copied to clipboard.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying request key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}