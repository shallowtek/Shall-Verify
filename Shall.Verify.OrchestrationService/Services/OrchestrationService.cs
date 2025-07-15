using AutoMapper;
using Shall.Verify.OrchestrationService.Clients;
using Shall.Verify.Common.Dtos;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Dtos.Record;

namespace Shall.Verify.OrchestrationService.Services;

public class OrchestrationService : IOrchestrationService
{
    private readonly VerifyClient _verifyClient;
    private readonly RecordClient _recordClient;
    private readonly IMapper _mapper;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public OrchestrationService(
        VerifyClient verifyClient,
        RecordClient recordClient,
        IMapper mapper)
    {
        _verifyClient = verifyClient;
        _recordClient = recordClient;
        _mapper = mapper;
    }

    public async Task<VerifyResponse> VerifyAsync(VerifyRequest verifyRequest)
    {
        var response = await _verifyClient.VerifyAsync(verifyRequest, _cancellationTokenSource.Token);

        if (response == null)
        {
            return null;
        }

        return response;
    }

    public async Task CreateRecordAsync(RecordAttributes record)
    {
        await _recordClient.CreateRecordAsync(record, _cancellationTokenSource.Token);
    }

    public async Task AddRecordStatusAsync(RecordStatusRequest recordStatusRequest)
    {
        await _recordClient.UpdateRecordStatusAsync(recordStatusRequest, _cancellationTokenSource.Token);
    }

    public async Task AddBatchRecordStatusAsync(BatchRecordStatusRequest batchRecordStatusRequest)
    {
        await _recordClient.BatchUpdateRecordStatusAsync(_mapper.Map<BatchRecordStatusRequest>(batchRecordStatusRequest), _cancellationTokenSource.Token);
    }
}