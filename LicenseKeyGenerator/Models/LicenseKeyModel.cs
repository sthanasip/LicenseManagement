using System.ComponentModel.DataAnnotations;

namespace LicenseKeyGenerator.Models
{
    public class LicenseKeyModel
    {
        public required string RequestKey { get; set; }
        public int NumberOfLicenses { get; set; }

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/([0-9]{4})$",
                           ErrorMessage = "Date must be in the format dd/MM/yyyy.")]
        public required string ExpiryDate { get; set; }
    }
}
