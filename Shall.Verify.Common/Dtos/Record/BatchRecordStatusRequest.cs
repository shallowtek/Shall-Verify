using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Dtos.Record;

public class BatchRecordStatusRequest
{
    public IDictionary<Guid, RecordStatusType> RecordStatusDictionary { get; set; }
}