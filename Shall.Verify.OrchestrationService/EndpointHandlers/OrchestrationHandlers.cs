using AutoMapper;
using Shall.Verify.OrchestrationService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Shall.Verify.Common.Dtos.Orchestration;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Dtos.Record;

namespace Shall.Verify.OrchestrationService.EndpointHandlers;

public static class OrchestrationHandlers
{
    public static async Task<Results<Ok<OrchestrationVerifyResponse>, NotFound>> VerifyAsync(
        OrchestrationVerifyRequest orchestrationVerifyRequest,
        IMapper mapper,
        IOrchestrationService orchestrationService,
        ILogger<OrchestrationVerifyResponse> logger)
    {

        var verifyId = Guid.NewGuid();

        var verifyRequest = mapper.Map<VerifyRequest>(orchestrationVerifyRequest);
        verifyRequest.VerifyId = verifyId;
        

        await orchestrationService.CreateRecordAsync(mapper.Map<RecordAttributes>(verifyRequest));

        var verifyResponse = await orchestrationService.VerifyAsync(verifyRequest);

        if (verifyResponse == null)
        {
            logger.LogInformation($"verifyResponse is null. VerifyId: {verifyRequest.VerifyId}");

            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.Map<OrchestrationVerifyResponse>(verifyResponse));
    }

    public static async Task<Results<Ok, NotFound>> AddRecordStatusAsync(
        RecordStatusRequest recordStatusRequest,
        IMapper mapper,
        IOrchestrationService orchestrationService,
        ILogger<VerifyResponse> logger)
    {
        await orchestrationService.AddRecordStatusAsync(recordStatusRequest);

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> AddBatchRecordStatusAsync(
        BatchRecordStatusRequest batchRecordStatusRequest,
        IMapper mapper,
        IOrchestrationService orchestrationService,
        ILogger<VerifyResponse> logger)
    {
        await orchestrationService.AddBatchRecordStatusAsync(batchRecordStatusRequest);

        return TypedResults.Ok();
    }
}