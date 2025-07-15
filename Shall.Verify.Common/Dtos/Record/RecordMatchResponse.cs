namespace Shall.Verify.Common.Dtos.Record;

public class RecordMatchResponse
{
    public IList<RecordMatchResult> RecordMatchResults { get; set; }
}

public class RecordMatchResult
{
    public bool IsMatch { get; set; }
    public Guid MatchId { get; set; }
}