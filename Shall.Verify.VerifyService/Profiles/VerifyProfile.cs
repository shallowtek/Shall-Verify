using AutoMapper;
using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Lookup;
using Shall.Verify.Common.Entities.Verify;

namespace Shall.Verify.VerifyService.Profiles;

public class VerifyProfile : Profile
{
    public VerifyProfile()
    {
        CreateMap<VerifyRequest, LookupRequest>();
        CreateMap<EmailLookupResult, EmailVerifyResult>();
        CreateMap<PhoneLookupResult, PhoneVerifyResult>();
        CreateMap<RecordCountLookupResult, RecordCountVerifyResult>();
        CreateMap<RecordMatchLookupResult, RecordMatchVerifyResult>();
        CreateMap<LookupResponse, VerifyResponse>();
    }
}