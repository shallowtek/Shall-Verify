namespace Shall.Verify.LookupService.Configurations;

public class TelesignServiceOptions
{
    public const string TelesignService = "TelesignService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
    public string Token { get; set; } = string.Empty;
}
