using System.Globalization;
using System.Windows.Controls;

namespace LicenseKeyGeneratorWPF.ValidationRules
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Number of Connections is required.");
            }

            if (!int.TryParse(value.ToString(), out _))
            {
                return new ValidationResult(false, "Number of Connections must be a valid integer.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
