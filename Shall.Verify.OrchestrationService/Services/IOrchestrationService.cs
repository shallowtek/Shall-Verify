using Shall.Verify.Common.Dtos;
using Shall.Verify.Common.Dtos.Record;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.OrchestrationService.Services;

public interface IOrchestrationService
{
    Task<VerifyResponse> VerifyAsync(VerifyRequest verifyRequest);

    Task CreateRecordAsync(RecordAttributes record);

    Task AddRecordStatusAsync(RecordStatusRequest recordStatusRequest);

    Task AddBatchRecordStatusAsync(BatchRecordStatusRequest batchRecordStatusRequest);
}