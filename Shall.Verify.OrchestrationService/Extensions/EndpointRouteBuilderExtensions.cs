using Shall.Verify.OrchestrationService.EndpointFilters;
using Shall.Verify.OrchestrationService.EndpointHandlers;

namespace Shall.Verify.OrchestrationService.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterOrchestrationEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var orchestrationVerifyEndpoints = endpointRouteBuilder.MapGroup("/verify");
        //.RequireAuthorization("RequiredAdminFromIreland");

        var orchestrationStatusEndpoints = endpointRouteBuilder.MapGroup("/record/status");

        var orchestrationStatusBatchEndpoints = endpointRouteBuilder.MapGroup("/record/status/batch");

        orchestrationVerifyEndpoints.MapPost("", OrchestrationHandlers.VerifyAsync)
        //.AddEndpointFilter<ValidateAnnotationsFilter>()
        .WithName("Verify")
        .WithOpenApi();

        orchestrationStatusEndpoints.MapPost("", OrchestrationHandlers.AddRecordStatusAsync)
        .WithName("AddRecordStatus")
        .WithOpenApi();

        orchestrationStatusBatchEndpoints.MapPost("", OrchestrationHandlers.AddBatchRecordStatusAsync)
        .WithName("AddBatchRecordStatus")
        .WithOpenApi();
    }
}