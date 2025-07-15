using Shall.Verify.Common.Dtos.Verify;

namespace Shall.Verify.VerifyService.Services;

public interface IVerifyService
{
    Task<VerifyResponse> VerifyAsync(VerifyRequest verifyRequest);
}