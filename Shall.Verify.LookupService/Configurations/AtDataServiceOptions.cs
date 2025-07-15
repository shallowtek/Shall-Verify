namespace Shall.Verify.LookupService.Configurations;

public class AtDataServiceOptions
{
    public const string AtDataService = "AtDataService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
    public string Token { get; set; } = string.Empty;
}
