using Shall.Verify.RecordService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Dtos.Record;

namespace Shall.Verify.RecordService.EndpointHandlers;

public static class RecordHandlers
{
    #region Record CRUD
    public static async Task<Results<Ok<RecordAttributes>, NotFound>> GetRecordAsync(
        Guid verifyId,
        IRecordService recordService)
    {
        var record = await recordService.GetRecordAsync(verifyId);

        if (record == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(record);
    }

    public static async Task<Results<Ok, ProblemHttpResult>> CreateRecordAsync(
        RecordAttributes record,
        IRecordService recordService)
    {
        var result = await recordService.CreateRecordAsync(record);

        if (!result)
        {
            return TypedResults.Problem();
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> DeleteRecordAsync(
       Guid verifyId,
       IRecordService recordService)
    {
        var result = await recordService.DeleteRecordAsync(verifyId);

        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
    #endregion Record CRUD

    #region Record Count
    public static async Task<Results<Ok<RecordCountResponse>, NotFound>> GetRecordCountAsync(
        RecordCountRequest recordCountRequest,
        IRecordService recordService)
    {
        var recordCountResponse = await recordService.GetRecordCountAsync(recordCountRequest);

        if (recordCountResponse == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(recordCountResponse);
    }

    #endregion Record Count

    #region Record Match
    public static async Task<Results<Ok<RecordMatchResponse>, NotFound>> GetRecordMatchAsync(
        RecordMatchRequest recordMatchRequest,
        IRecordService recordService)
    {
        var recordMatchResponse = await recordService.GetRecordMatchAsync(recordMatchRequest);

        if (recordMatchResponse == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(recordMatchResponse);
    }

    #endregion Record Match

    #region Record Status

    public static async Task<Results<Ok, NotFound>> AddRecordStatusAsync(
       RecordStatusRequest record,
       IRecordService recordService)
    {
        var result = await recordService.AddRecordStatusAsync(record);

        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> AddBatchRecordStatusAsync(
       BatchRecordStatusRequest batchRecordStatusRequest,
       IRecordService recordService)
    {
        var result = await recordService.AddBatchRecordStatusAsync(batchRecordStatusRequest);

        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    #endregion Record Status
}