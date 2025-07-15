using Shall.Verify.VerifyService.EndpointHandlers;

namespace Shall.Verify.VerifyService.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterVerifyEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var verifyEndpoints = endpointRouteBuilder.MapGroup("/verify");

        verifyEndpoints.MapPost("", VerifyHandlers.VerifyAsync)
        .WithName("Verify")
        .WithOpenApi();
    }
}