using AutoMapper;
using Sqids;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        private readonly SqidsEncoder<long> _idEncoder;

        public AutoMapping(SqidsEncoder<long> idEncoder)
        {
            _idEncoder = idEncoder;

            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<RequestCreateSubjectJson, Domain.Entities.Subject>();

            CreateMap<RequestCreateTopicJson, Domain.Entities.Topic>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(source => _idEncoder.Decode(source.SubjectId).FirstOrDefault()));
        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.User, ResponseUserProfileJson>();

            CreateMap<Domain.Entities.Subject, ResponseCreatedSubjectJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<Domain.Entities.Subject, ResponseShortSubjectJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<Domain.Entities.Subject, ResponseSubjectJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<Domain.Entities.Topic, ResponseCreatedTopicJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<Domain.Entities.Topic, ResponseShortTopicJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

        }
    }
}
