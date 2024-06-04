namespace LicenseKeyGenerator.Models
{
    public class LicenseKeyModel
    {
        public required string Key { get; set; }
        public int NumberOfLicenses { get; set; }
        public required string LicenseEndDate {  get; set; }
        public required string Code { get; set; }   
    }
}
