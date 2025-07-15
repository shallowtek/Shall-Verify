using AutoMapper;
using Confluent.Kafka;
using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Configuration;
using Shall.Verify.Common.Enums;
using Shall.Verify.VerifyService.Clients;

namespace Shall.Verify.VerifyService.Services;

public class VerifyService : IVerifyService
{
    private readonly ConfigurationClient _configurationClient;
    private readonly LookupClient _lookupClient;
    private readonly IMapper _mapper;
    private readonly IProducer<string, VerifyResponse> _producer;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    const string verifyTopic = "verifyTopic";

    public VerifyService(
        ConfigurationClient configurationClient,
        LookupClient lookupClient,
        IMapper mapper,
        IProducer<string, VerifyResponse> producer)
    {
        _configurationClient = configurationClient;
        _lookupClient = lookupClient;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<VerifyResponse> VerifyAsync(VerifyRequest verifyRequest)
    {
        var featureConfiguration = await _configurationClient.GetFeatureConfigurationAsync(
            verifyRequest.SiteId, _cancellationTokenSource.Token);

        var ruleConfiguration = await _configurationClient.GetRuleConfigurationAsync(
            verifyRequest.SiteId, _cancellationTokenSource.Token);

        if (featureConfiguration?.Features == null ||
            (ruleConfiguration?.Rules == null && ruleConfiguration?.RecordCountRules == null))
        {
            return null;
        }

        var lookupRequest = _mapper.Map<LookupRequest>(verifyRequest);

        if (featureConfiguration.Features.Any(a => a
        .FeatureType.Equals(FeatureTypes.RecordCount) && a
        .Enabled) && ruleConfiguration.RecordCountRules != null)
        {
            lookupRequest.RecordCountSearchItems = CreateRecordCountSearchItems(
                verifyRequest,
                ruleConfiguration.RecordCountRules);
        }

        if (featureConfiguration.Features.Any(a => a
        .FeatureType.Equals(FeatureTypes.RecordMatch) && a
        .Enabled) && ruleConfiguration.RecordMatchRules != null)
        {
            lookupRequest.RecordMatchSearchItems = CreateRecordMatchSearchItems(
                verifyRequest,
                ruleConfiguration.RecordMatchRules);
        }

        var lookupResponse = await _lookupClient.GetLookupResponseAsync(lookupRequest, _cancellationTokenSource.Token);

        if (lookupResponse == null)
        {
            return null;
        }

        var verifyResponse = _mapper.Map<VerifyResponse>(lookupResponse);

        verifyResponse.VerifyId = verifyRequest.VerifyId;
        verifyResponse.SiteId = verifyRequest.SiteId;
        verifyResponse.ReferenceId = verifyRequest.ReferenceId;
        verifyResponse.FailThreshold = ruleConfiguration.FailThreshold;
        verifyResponse.FlagThreshold = ruleConfiguration.FlagThreshold;

        CreateVerifyResults(verifyResponse, ruleConfiguration);

        if (verifyResponse.Score > ruleConfiguration.FailThreshold)
        {
            verifyResponse.Result = ResultTypes.Fail;

            if (verifyResponse.Score > ruleConfiguration.FlagThreshold &&
                verifyResponse.Score < ruleConfiguration.FailThreshold)
            {
                verifyResponse.Result = ResultTypes.Flag;
            }
        }

        await _producer.ProduceAsync(verifyTopic, new Message<string, VerifyResponse>
        {
            Key = verifyRequest.VerifyId.ToString(),
            Value = verifyResponse
        });

        return verifyResponse;
    }

    private void CreateVerifyResults(VerifyResponse verifyResponse, RuleConfiguration ruleConfiguration)
    {

        foreach (var result in verifyResponse.EmailResults)
        {
            var rule = ruleConfiguration.Rules.FirstOrDefault(
                        x => x.FeatureType == FeatureTypes.Email);

            if (rule != null)
            {
                result.FailThreshold = rule.FailThreshold;
                result.Weighting = rule.Weighting;

                if (result.Score > rule.FailThreshold)
                {
                    result.Result = ResultTypes.Fail;
                    if (rule.IsNegativeWeighting)
                    {
                        verifyResponse.Score -= rule.Weighting;
                    }
                    else
                    {
                        verifyResponse.Score += rule.Weighting;
                    }
                }
            }
        }

        foreach (var result in verifyResponse.PhoneResults)
        {
            var rule = ruleConfiguration.Rules.FirstOrDefault(
                        x => x.FeatureType == FeatureTypes.Phone);

            if (rule != null)
            {
                result.FailThreshold = rule.FailThreshold;
                result.Weighting = rule.Weighting;

                if (result.Score > rule.FailThreshold)
                {
                    result.Result = ResultTypes.Fail;
                    if (rule.IsNegativeWeighting)
                    {
                        verifyResponse.Score -= rule.Weighting;
                    }
                    else
                    {
                        verifyResponse.Score += rule.Weighting;
                    }
                }
            }
        }

        foreach (var result in verifyResponse.RecordCountResults)
        {
            var rule = ruleConfiguration.RecordCountRules.FirstOrDefault(
                        x => x.DataType == result.DataType
                        && x.TimeRangeInSeconds == result.TimeRangeInSeconds);

            if (rule != null)
            {
                result.FailThreshold = rule.FailThreshold;
                result.Weighting = rule.Weighting;

                if (result.Count > rule.FailThreshold)
                {
                    result.Result = ResultTypes.Fail;

                    if (rule.IsNegativeWeighting)
                    {
                        verifyResponse.Score -= rule.Weighting;
                    }
                    else
                    {
                        verifyResponse.Score += rule.Weighting;
                    }
                }
            }
        }

        foreach (var result in verifyResponse.RecordMatchResults)
        {
            var rule = ruleConfiguration.RecordCountRules.FirstOrDefault(
                        x => x.FeatureType == FeatureTypes.RecordMatch);

            if (rule != null)
            {
                result.FailThreshold = rule.FailThreshold;
                result.Weighting = rule.Weighting;

                if (result.MatchTotalWeight > rule.FailThreshold)
                {
                    result.Result = ResultTypes.Fail;

                    if (rule.IsNegativeWeighting)
                    {
                        verifyResponse.Score -= rule.Weighting;
                    }
                    else
                    {
                        verifyResponse.Score += rule.Weighting;
                    }
                }
            }
        }
    }

    public List<RecordCountSearchItem> CreateRecordCountSearchItems(VerifyRequest verifyRequest, IList<RecordCountRule> recordCountRules)
    {
        var recordCountSearchItems = new List<RecordCountSearchItem>();

        foreach (var rule in recordCountRules)
        {
            switch (rule.DataType)
            {
                case RecordDataTypes.Email:

                    foreach (var item in verifyRequest.Record.Email)
                    {
                        var recordCountSearchItem = new RecordCountSearchItem
                        {
                            DataType = RecordDataTypes.Email,
                            DataValue = item.EmailAddress,
                            TimeRangeInSeconds = rule.TimeRangeInSeconds
                        };

                        recordCountSearchItems.Add(recordCountSearchItem);
                    }

                    break;
                case RecordDataTypes.Phone:
                    foreach (var item in verifyRequest.Record.Phone)
                    {

                        var recordCountSearchItem = new RecordCountSearchItem
                        {
                            DataType = RecordDataTypes.Phone,
                            DataValue = item.PhoneNumber,
                            TimeRangeInSeconds = rule.TimeRangeInSeconds
                        };

                        recordCountSearchItems.Add(recordCountSearchItem);
                    }
                    break;
                default:
                    break;
            }
        }

        return recordCountSearchItems;
    }

    public HashSet<RecordMatchSearchItem> CreateRecordMatchSearchItems(VerifyRequest verifyRequest, IList<RecordMatchRule> recordMatchRules)
    {
        var recordMatchSearchItems = new HashSet<RecordMatchSearchItem>();

        foreach (var rule in recordMatchRules)
        {
            var nonZeroWeightedList = rule.MatchWeightings.Where(v => v.DataWeight != 0).ToList();

            foreach (var item in nonZeroWeightedList)
            {
                switch (item.DataType)
                {
                    case RecordDataTypes.Email:

                        foreach (var email in verifyRequest.Record.Email)
                        {
                            var recordMatchSearchItem = new RecordMatchSearchItem
                            {
                                DataType = item.DataType,
                                DataValue = email.EmailAddress,
                                DataWeighting = item.DataWeight
                            };

                            recordMatchSearchItems.Add(recordMatchSearchItem);
                        }

                        break;
                    case RecordDataTypes.Phone:

                        foreach (var phone in verifyRequest.Record.Phone)
                        {
                            var recordMatchSearchItem = new RecordMatchSearchItem
                            {
                                DataType = item.DataType,
                                DataValue = phone.PhoneNumber,
                                DataWeighting = item.DataWeight
                            };

                            recordMatchSearchItems.Add(recordMatchSearchItem);
                        }
                        break;
                    case RecordDataTypes.Status:
                        break;
                    default:
                        break;
                }
            }
        }

        return recordMatchSearchItems;
    }
}