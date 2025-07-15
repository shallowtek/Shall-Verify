using MiniValidation;
using Shall.Verify.Common.Dtos.Orchestration;

namespace Shall.Verify.OrchestrationService.EndpointFilters;

public class ValidateAnnotationsFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var verifyRequest = context.GetArgument<OrchestrationVerifyRequest>(0);

        if (!MiniValidator.TryValidate(verifyRequest, out var validationErrors))
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        return await next(context);
    }
}