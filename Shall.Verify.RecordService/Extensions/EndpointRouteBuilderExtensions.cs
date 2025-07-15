using Shall.Verify.RecordService.EndpointHandlers;

namespace Shall.Verify.RecordService.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRecordEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var recordEndpoint = endpointRouteBuilder.MapGroup("/record");

        var recordCountEndpoint = endpointRouteBuilder.MapGroup("/record/count");

        var recordMatchEndpoint = endpointRouteBuilder.MapGroup("/record/match");

        var recordWithGuidIdEndpoints = endpointRouteBuilder.MapGroup("/record/{verifyId:Guid}");

        var recordStatusEndpoint = endpointRouteBuilder.MapGroup("/record/status");

        var recordBatchStatusEndpoint = endpointRouteBuilder.MapGroup("/record/status/batch");

        #region Record CRUD

        recordWithGuidIdEndpoints.MapGet("", RecordHandlers.GetRecordAsync)
        .WithName("GetRecord")
        .WithOpenApi();

        recordEndpoint.MapPost("", RecordHandlers.CreateRecordAsync)
        .WithName("CreateRecord")
        .WithOpenApi();

        recordWithGuidIdEndpoints.MapDelete("", RecordHandlers.DeleteRecordAsync)
        .WithName("DeleteRecord")
        .WithOpenApi();

        #endregion Record CRUD

        #region Record Count

        recordCountEndpoint.MapPost("", RecordHandlers.GetRecordCountAsync)
        .WithName("GetRecordCount")
        .WithOpenApi();

        #endregion Record Count

        #region Record Match

        recordMatchEndpoint.MapPost("", RecordHandlers.GetRecordMatchAsync)
        .WithName("GetRecordMatch")
        .WithOpenApi();

        #endregion Record Match

        #region Record Status

        recordStatusEndpoint.MapPost("", RecordHandlers.AddRecordStatusAsync)
       .WithName("AddRecordStatus")
       .WithOpenApi();

        recordBatchStatusEndpoint.MapPost("", RecordHandlers.AddBatchRecordStatusAsync)
       .WithName("BatchAddRecordStatuses")
       .WithOpenApi();

        #endregion Record Status


    }
}
