using Shall.Verify.Common.Entities.Lookup;

namespace Shall.Verify.Common.Dtos.Lookup;

public class LookupResponse
{
    public IList<EmailLookupResult> EmailResults { get; set; }
    public IList<PhoneLookupResult> PhoneResults { get; set; }
    public IList<RecordCountLookupResult> RecordCountResults { get; set; }
    public IList<RecordMatchLookupResult> RecordMatchResults { get; set; }
}