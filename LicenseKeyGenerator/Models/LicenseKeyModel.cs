using License.Common.Constant;
using System.ComponentModel.DataAnnotations;

namespace LicenseKeyGenerator.Models
{
    public class LicenseKeyModel
    {
        public required string RequestKey { get; set; }
        public int NumberOfLicenses { get; set; }

        [RegularExpression(RegexConstant.DateRegex,
                           ErrorMessage = "Date must be in the format dd/MM/yyyy.")]
        public required string ExpiryDate { get; set; }
    }
}
