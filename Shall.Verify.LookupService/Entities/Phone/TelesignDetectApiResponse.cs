namespace Shall.Verify.LookupService.Entities.Phone;

public class TelesignDetectApiResponse
{
    public string reference_id { get; set; }
    public string external_id { get; set; }
    public Status status { get; set; }
    public Risk risk { get; set; }
    public RiskInsights risk_insights { get; set; }
}

public class Risk
{
    public string level { get; set; }
    public string recommendation { get; set; }
    public int score { get; set; }
}

public class RiskInsights
{
    public int status { get; set; }
    public List<int> category { get; set; }
    public List<int> a2p { get; set; }
    public List<int> p2p { get; set; }
    public List<int> number_type { get; set; }
    public List<int> ip { get; set; }
    public List<int> email { get; set; }
}

public class Status
{
    public int code { get; set; }
    public string description { get; set; }
    public DateTime updated_on { get; set; }
}