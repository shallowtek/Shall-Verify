namespace Shall.Verify.VerifyService.Configurations;

public class LookupServiceOptions
{
    public const string LookupService = "LookupService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
}