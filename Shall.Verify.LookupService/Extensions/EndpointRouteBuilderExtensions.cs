using Shall.Verify.LookupService.EndpointHandlers;

namespace Shall.Verify.LookupService.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterLookupEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var lookupEndpoints = endpointRouteBuilder.MapGroup("/lookup");

        lookupEndpoints.MapPost("", LookupHandlers.GetLookupAsync)
        .WithName("lookup")
        .WithOpenApi();
    }
}