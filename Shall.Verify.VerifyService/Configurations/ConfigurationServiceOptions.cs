namespace Shall.Verify.VerifyService.Configurations;

public class ConfigurationServiceOptions
{
    public const string ConfigurationService = "ConfigurationService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
}