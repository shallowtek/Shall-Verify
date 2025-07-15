using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Dtos.Record;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Enums;
using System.Collections.ObjectModel;

namespace Shall.Verify.RecordService.Services;

public class RecordService : IRecordService
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<RecordService> _logger;
    const string recordIndexName = "record";
    const string verifyIdFieldName = "verifyId";
    const string siteIdFieldName = "siteId";
    const string createdDateFieldName = "createdDate";

    public RecordService(
        ElasticsearchClient client,
        ILogger<RecordService> logger)
    {
        _client = client;
        _logger = logger;
    }

    #region Record CRUD

    public async Task<RecordAttributes> GetRecordAsync(Guid verifyId)
    {
        var response = await _client.GetAsync<RecordAttributes>(recordIndexName, verifyId);

        if (!response.IsSuccess())
        {
            _logger.LogError($"Error getting record: RecordId: {verifyId} HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
            return null;
        }

        return response.Source;
    }

    public async Task<bool> CreateRecordAsync(RecordAttributes record)
    {
        #region DEV -  Only used for local dev. Create index if does not exist.
        var existsResponse = await _client.Indices.ExistsAsync(recordIndexName);

        if (existsResponse.IsSuccess() && !existsResponse.Exists)
        {
            var createResponse = await _client.Indices.CreateAsync<RecordAttributes>(recordIndexName, c => c
            .Mappings(map => map
              .Properties(p => p
              .Keyword(t => t.VerifyId)
              .Keyword(t => t.SiteId)
              .Date(t => t.CreatedDate)
              .Date(t => t.ModifiedDate)
              .Keyword(t => t.Email)
              .Keyword(t => t.Phone)
              .Keyword(t => t.Status)
              )));
        }
        #endregion DEV - Create index

        record.CreatedDate = DateTime.UtcNow;
        record.Status = new HashSet<string> { Enum.GetName(RecordStatusType.Placed) };

        var response = await _client.IndexAsync(record, recordIndexName, record.VerifyId);

        if (!response.IsSuccess())
        {
            _logger.LogError($"Error calling Elastic: VerifyId: {record.VerifyId} HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteRecordAsync(Guid verifyId)
    {
        var response = await _client.DeleteAsync(index: recordIndexName, id: verifyId);

        if (!response.IsSuccess())
        {
            _logger.LogError($"Error calling Elastic: RecordId: {verifyId} HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
            return false;
        }

        return true;
    }

    #endregion Record CRUD

    #region Record Status
    public async Task<bool> AddRecordStatusAsync(RecordStatusRequest record)
    {
        var response = await _client.UpdateAsync<RecordAttributes, RecordAttributes>(
            index: recordIndexName,
            id: record.VerifyId,
            u => u.Script(s => s
                .Source("ctx._source.status.add(params.item)")
                .Params(p => p
                    .Add("item", Enum.GetName(record.Status))
                    )
                )
            );

        if (!response.IsSuccess())
        {
            _logger.LogError($"Error calling Elastic: VerifyId: {record.VerifyId} HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
            return false;
        }

        return true;
    }

    public async Task<bool> AddBatchRecordStatusAsync(BatchRecordStatusRequest batchRecordStatusRequest)
    {
        var response = await _client.BulkAsync(b => b
            .Index(recordIndexName)
            .UpdateMany(batchRecordStatusRequest.RecordStatusDictionary.Select(x => new { x.Key, x.Value }), (descriptor, update) => descriptor
                .Id(update.Key)
                .Script(s => s
                    .Source("ctx._source.status.add(params.item)")
                    .Params(p => p
                        .Add("item", Enum.GetName(update.Value))))));


        if (!response.IsSuccess())
        {
            _logger.LogError($"Error calling Elastic HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
            return false;
        }

        return true;
    }
    #endregion Record Status

    #region Record Count
    public async Task<RecordCountResponse> GetRecordCountAsync(RecordCountRequest recordCountRequest)
    {
        var recordCountResponse = new RecordCountResponse();
        recordCountResponse.RecordCountResults = new List<RecordCountResult>();

        foreach (var item in recordCountRequest.RecordCountSearchItems)
        {
            var boolQuery = BuildCountBoolQuery(recordCountRequest.SiteId, recordCountRequest.VerifyId, item);

            var response = await _client.SearchAsync<RecordCountResult>(s => s
               .Index(recordIndexName)
               .Query(boolQuery)
               .Aggregations(aggs => aggs
                   .Add(verifyIdFieldName, agg => agg
                        .Cardinality(c => c.Field(verifyIdFieldName))))
               .Size(0));


            if (!response.IsSuccess())
            {
                _logger.LogError($"Error calling Elastic: VerifyId: {recordCountRequest.VerifyId} HttpStatus: {response.ApiCallDetails.HttpStatusCode}");
                continue;
            }

            recordCountResponse.RecordCountResults.Add(CreateRecordCountResult(response, item));
        }

        return recordCountResponse;
    }

    private BoolQuery BuildCountBoolQuery(Guid siteId, Guid verifyId, RecordCountSearchItem recordCountSearchItem)
    {
        var dataType = Enum.GetName(typeof(RecordDataTypes), recordCountSearchItem.DataType);

        var primaryTermQuery = new TermQuery(dataType.ToLowerInvariant())
        {
            Value = recordCountSearchItem.DataValue,
            CaseInsensitive = true
        };

        var filterTermQuery = new TermQuery(siteIdFieldName)
        {
            Value = siteId.ToString()
        };

        var dateRangeQuery = new DateRangeQuery(createdDateFieldName)
        {
            Gte = $"now-{recordCountSearchItem.TimeRangeInSeconds}s/s",
            Lte = $"now/s"
        };

        var omittedDataTypeQuery = new TermQuery(verifyIdFieldName)
        {
            Value = verifyId.ToString()
        };

        var boolQuery = new BoolQuery
        {
            Must = new Collection<Query>() { primaryTermQuery },
            Filter = new Collection<Query>() { dateRangeQuery, filterTermQuery },
            MustNot = new Collection<Query>() { omittedDataTypeQuery }
        };

        return boolQuery;
    }

    private RecordCountResult CreateRecordCountResult(SearchResponse<RecordCountResult> response, RecordCountSearchItem item)
    {
        var count = 0;

        foreach (var agg in response.Aggregations)
        {

            if (agg.Key == verifyIdFieldName)
            {
                count = (int)response.Aggregations.GetCardinality(verifyIdFieldName).Value;
                continue;
            }
        }

        return new RecordCountResult
        {
            DataType = item.DataType,
            DataValue = item.DataValue,
            TimeRangeInSeconds = item.TimeRangeInSeconds,
            Count = count
        };
    }
    #endregion Record Count

    #region Record Match
    public async Task<RecordMatchResponse> GetRecordMatchAsync(RecordMatchRequest recordMatchRequest)
    {
        var recordMatchResponse = new RecordMatchResponse();
        recordMatchResponse.RecordMatchResults = new List<RecordMatchResult>();

        var functionScoreQuery = BuildMatchQuery(recordMatchRequest.RecordMatchSearchItems);

        var response = await _client.SearchAsync<RecordAttributes>(s => s
       .Index(recordIndexName)
       .Query(functionScoreQuery)
       .Size(1));

        if (!response.IsSuccess())
        {
            _logger.LogError($"Error calling Elastic Match HttpStatus: {response.ApiCallDetails.HttpStatusCode}");

            return null;
        }

        if (response?.Hits != null && response.Hits.Any())
        {
            return null;
        }

        return recordMatchResponse;

    }

    public FunctionScoreQuery BuildMatchQuery(HashSet<RecordMatchSearchItem> recordMatchSearchItems)
    {
        var weightFunctionList = new List<FunctionScore>();
        var shouldQueryTerms = new Collection<Query>();

        foreach (var item in recordMatchSearchItems)
        {
            var fieldName = Enum.GetName(item.DataType).ToLowerInvariant();

            var matchQuery = new MatchQuery(fieldName)
            {
                Query = item.DataValue
            };

            var termQuery = new TermQuery(fieldName)
            {
                Value = item.DataValue
            };

            var weightFunction = FunctionScore.WeightScore(item.DataWeighting);
            weightFunction.Filter = matchQuery;


            weightFunctionList.Add(weightFunction);
            shouldQueryTerms.Add(termQuery);
        }

        var boolQuery = new BoolQuery()
        {
            Should = shouldQueryTerms
        };


        var functionScoreQuery = new FunctionScoreQuery
        {
            ScoreMode = FunctionScoreMode.Sum,
            Functions = weightFunctionList,
            Query = new ConstantScoreQuery()
            {
                Filter = boolQuery
            }
        };

        return functionScoreQuery;
    }

    #endregion Record Match
}