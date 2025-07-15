using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Dtos.Record;
using Shall.Verify.Common.Entities.Lookup;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.LookupService.Clients;

namespace Shall.Verify.LookupService.Services;

public class LookupService : ILookupService
{
    private readonly AtDataClient _atDataClient;
    private readonly TelesignClient _telesignClient;
    private readonly RecordClient _recordClient;

    public LookupService(
        AtDataClient atDataClient,
        TelesignClient telesignClient,
        RecordClient recordClient)
    {
        _atDataClient = atDataClient;
        _telesignClient = telesignClient;
        _recordClient = recordClient;
    }

    public async Task<List<EmailLookupResult>> GetEmailLookupResultsAsync(IList<Email> emails)
    {
        if (emails == null || !emails.Any())
        {
            return null;
        }

        var results = new List<EmailLookupResult>();

        await Parallel.ForEachAsync(emails, async (email, token) =>
        {
            var response = await _atDataClient
            .GetAtDataFraudPreventionResponseAsync(email.EmailAddress, token);

            if (response != null)
            {
                results.Add(new EmailLookupResult
                {
                    InputEmailAddress = email.EmailAddress,
                    Score = response.Risk.Score
                });
            }
        });

        return results;
    }

    public async Task<List<PhoneLookupResult>> GetPhoneLookupResultsAsync(IList<Phone> phones)
    {
        if (phones == null || !phones.Any())
        {
            return null;
        }

        var results = new List<PhoneLookupResult>();

        await Parallel.ForEachAsync(phones, async (phone, token) =>
        {
            var response = await _telesignClient
            .GetTelesignDetectApiResponseAsync(phone.PhoneNumber, token);

            if (response != null)
            {
                results.Add(new PhoneLookupResult
                {
                    InputPhoneNumber = phone.PhoneNumber,
                    Score = response.risk.score
                });
            }
        });

        return results;
    }

    public async Task<List<RecordCountLookupResult>> GetRecordCountLookupResultsAsync(LookupRequest lookupRequest)
    {
        if (lookupRequest.RecordCountSearchItems == null ||
            !lookupRequest.RecordCountSearchItems.Any())
        {
            return null;
        }

        var results = new List<RecordCountLookupResult>();

        var countRequest = new RecordCountRequest()
        {
            VerifyId = lookupRequest.VerifyId,
            SiteId = lookupRequest.SiteId,
            RecordCountSearchItems = lookupRequest.RecordCountSearchItems
        };

        var response = await _recordClient.GetRecordCountAsync(countRequest);

        if (response == null ||
            !response.RecordCountResults.Any())
        {
            return null;
        }

        foreach (var item in response.RecordCountResults)
        {
            var lookupResult = new RecordCountLookupResult()
            {
                DataType = item.DataType,
                DataValue = item.DataValue,
                TimeRangeInSeconds = item.TimeRangeInSeconds,
                Count = item.Count
            };

            results.Add(lookupResult);
        };

        return results;
    }

    public async Task<List<RecordMatchLookupResult>> GetRecordMatchLookupResultsAsync(LookupRequest lookupRequest)
    {
        if (lookupRequest.RecordMatchSearchItems == null ||
            !lookupRequest.RecordMatchSearchItems.Any())
        {
            return null;
        }

        var results = new List<RecordMatchLookupResult>();

        var matchRequest = new RecordMatchRequest()
        {
            VerifyId = lookupRequest.VerifyId,
            SiteId = lookupRequest.SiteId,
            RecordMatchSearchItems = lookupRequest.RecordMatchSearchItems
        };

        var response = await _recordClient.GetRecordMatchAsync(matchRequest);

        if (response == null)
        {
            return null;
        }

        foreach (var item in response.RecordMatchResults)
        {
            var lookupResult = new RecordMatchLookupResult()
            {
                IsMatch = item.IsMatch,
                MatchId = item.MatchId
            };

            results.Add(lookupResult);
        };

        return results;
    }
}