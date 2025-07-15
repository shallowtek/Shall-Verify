using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Entities.Verify;

public class VerifyResult
{
    public ResultTypes Result { get; set; }
    public int FailThreshold { get; set; }
    public int Weighting { get; set; }
}

public class EmailVerifyResult : VerifyResult
{
    public string InputEmailAddress { get; set; }
    public int Score { get; set; }
}

public class PhoneVerifyResult : VerifyResult
{
    public string InputPhoneNumber { get; set; }
    public int Score { get; set; }
}

public class RecordCountVerifyResult : VerifyResult
{
    public RecordDataTypes DataType { get; set; }
    public string DataValue { get; set; }
    public int TimeRangeInSeconds { get; set; }
    public int Count { get; set; }
}

public class RecordMatchVerifyResult : VerifyResult
{
    public bool IsMatch { get; set; }
    public Guid MatchId { get; set; }
    public int MatchTotalWeight { get; set; }
}