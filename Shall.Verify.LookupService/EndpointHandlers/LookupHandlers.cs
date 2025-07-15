using AutoMapper;
using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.LookupService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Shall.Verify.LookupService.EndpointHandlers;

public static class LookupHandlers
{
    public static async Task<Results<Ok<LookupResponse>, NotFound>> GetLookupAsync(
        LookupRequest lookupRequest,
        IMapper mapper,
        ILookupService lookupService,
        ILogger<LookupResponse> logger)
    {
        var tasks = new List<Task>();
        var emailLookupResults = lookupService.GetEmailLookupResultsAsync(lookupRequest.Record.Email);
        tasks.Add(emailLookupResults);

        var phoneLookupResults = lookupService.GetPhoneLookupResultsAsync(lookupRequest.Record.Phone);
        tasks.Add(phoneLookupResults);

        var recordCountLookupResults = lookupService.GetRecordCountLookupResultsAsync(lookupRequest);
        tasks.Add(recordCountLookupResults);

        var recordMatchLookupResults = lookupService.GetRecordMatchLookupResultsAsync(lookupRequest);
        tasks.Add(recordMatchLookupResults);

        await Task.WhenAll(tasks);

        var lookupResponse = new LookupResponse()
        {
            EmailResults = await emailLookupResults,
            PhoneResults = await phoneLookupResults,
            RecordCountResults = await recordCountLookupResults,
            RecordMatchResults = await recordMatchLookupResults
        };

        return TypedResults.Ok(lookupResponse);
    }
}