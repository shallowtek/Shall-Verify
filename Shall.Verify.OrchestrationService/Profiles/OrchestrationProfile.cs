using AutoMapper;
using Shall.Verify.Common.Dtos.Orchestration;
using Shall.Verify.Common.Dtos.Verify;
using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.OrchestrationService.Profiles;

public class OrchestrationProfile : Profile
{
    public OrchestrationProfile()
    {
        CreateMap<VerifyRequest, RecordAttributes>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Record.Email.Select(s => s.EmailAddress).ToHashSet()))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Record.Phone.Select(s => s.PhoneNumber).ToHashSet()));

        CreateMap<OrchestrationVerifyRequest, VerifyRequest>();
        CreateMap<VerifyResponse, OrchestrationVerifyResponse>();
    }
}