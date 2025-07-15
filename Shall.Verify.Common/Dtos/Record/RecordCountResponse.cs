using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Dtos.Record;

public class RecordCountResponse
{
    public IList<RecordCountResult> RecordCountResults { get; set; }
}

public class RecordCountResult
{
    public RecordDataTypes DataType { get; set; }
    public string DataValue { get; set; }
    public int TimeRangeInSeconds { get; set; }
    public int Count { get; set; }
}