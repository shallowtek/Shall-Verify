namespace Shall.Verify.OrchestrationService.Configurations;

public class VerifyServiceOptions
{
    public const string VerifyService = "VerifyService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
}
