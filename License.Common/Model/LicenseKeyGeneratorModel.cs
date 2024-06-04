namespace License.Common.Model
{
    public record LicenseKeyGeneratorModel(string RequestKey, int NumberOfLicenses, string ExpiryDate);
    public record LicenseKeyResponse(bool result, int license, string validity, string message);
}
