using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Entities.Lookup;
using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.LookupService.Services;

public interface ILookupService
{
    Task<List<EmailLookupResult>> GetEmailLookupResultsAsync(IList<Email> emails);

    Task<List<PhoneLookupResult>> GetPhoneLookupResultsAsync(IList<Phone> phones);

    Task<List<RecordCountLookupResult>> GetRecordCountLookupResultsAsync(LookupRequest lookupRequest);

    Task<List<RecordMatchLookupResult>> GetRecordMatchLookupResultsAsync(LookupRequest lookupRequest);
}