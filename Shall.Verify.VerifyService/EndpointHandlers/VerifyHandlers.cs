using AutoMapper;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.VerifyService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Shall.Verify.VerifyService.EndpointHandlers;

public static class VerifyHandlers
{
    public static async Task<Results<Ok<VerifyResponse>, NotFound>> VerifyAsync(
        VerifyRequest verifyRequest,
        IMapper mapper,
        IVerifyService verifyService,
        ILogger<VerifyResponse> logger)
    {
        var verifyResponse = await verifyService.VerifyAsync(verifyRequest);

        if (verifyResponse == null)
        {
            logger.LogInformation($"verifyResponse is null. VerifyId: {verifyRequest.VerifyId}");

            return TypedResults.NotFound();
        }

        return TypedResults.Ok(verifyResponse);
    }
}