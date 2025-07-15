namespace Shall.Verify.LookupService.Entities.Email;

public class AtDataFraudPreventionResponse
{
    public Risk Risk { get; set; }
}

public class Risk
{
    public int Score { get; set; }
}
