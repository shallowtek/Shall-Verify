using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Dtos.Lookup;

public class LookupRequest
{
    public Guid VerifyId { get; set; }
    public Guid SiteId { get; set; }
    public RecordData Record { get; set; }
    public IList<RecordCountSearchItem> RecordCountSearchItems { get; set; }

    public HashSet<RecordMatchSearchItem> RecordMatchSearchItems { get; set; }
}

public class RecordSearchItem 
{
    public RecordDataTypes DataType { get; set; }
    public string DataValue { get; set; }
}

public class RecordCountSearchItem : RecordSearchItem
{
    public int TimeRangeInSeconds { get; set; }
}

public class RecordMatchSearchItem : RecordSearchItem
{
    public int DataWeighting { get; set; }
}