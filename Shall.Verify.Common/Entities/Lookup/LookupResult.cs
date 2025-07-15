using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Entities.Lookup;

public class EmailLookupResult
{
    public string InputEmailAddress { get; set; }
    public int Score { get; set; }
}

public class PhoneLookupResult
{
    public string InputPhoneNumber { get; set; }
    public int Score { get; set; }
}

public class RecordCountLookupResult
{
    public RecordDataTypes DataType { get; set; }
    public string DataValue { get; set; }
    public int TimeRangeInSeconds { get; set; }
    public int Count { get; set; }
}

public class RecordMatchLookupResult
{
    public bool IsMatch { get; set; }

    public Guid MatchId { get; set; }
}