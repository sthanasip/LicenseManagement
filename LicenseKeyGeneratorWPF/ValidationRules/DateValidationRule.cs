using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace LicenseKeyGeneratorWPF.ValidationRules
{
    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Expiry Date is required.");
            }

            string date = value.ToString();
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([0-9]{4})$";
            if (!Regex.IsMatch(date, pattern))
            {
                return new ValidationResult(false, "Date must be in the format dd/MM/yyyy.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
