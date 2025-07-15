namespace Shall.Verify.OrchestrationService.Configurations;

public class RecordServiceOptions
{
    public const string RecordService = "RecordService";
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 0;
}
