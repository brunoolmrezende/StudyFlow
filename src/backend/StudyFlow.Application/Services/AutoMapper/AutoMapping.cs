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
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(source => _idEncoder.Decode(source.SubjectId)[0]));

            CreateMap<RequestUpdateTopicJson, Domain.Entities.Topic>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(source => _idEncoder.Decode(source.SubjectId)[0]))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description != null ? source.Description.Trim() : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(source => source.Active))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<RequestCreateReviewJson, Domain.Entities.Review>()
                .ForMember(dest => dest.TopicId, opt => opt.MapFrom(source => _idEncoder.Decode(source.TopicId)[0]))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(source => source.Difficulty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(source => source.ScheduledDate ?? DateTime.UtcNow.AddDays(ReviewScheduler.GetDaysUntilNextReview((Domain.Enums.DifficultyLevel)source.Difficulty))));

            CreateMap<RequestUpdateReviewJson, Domain.Entities.Review>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(source => source.Difficulty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(source => source.ScheduledDate ?? DateTime.UtcNow.AddDays(ReviewScheduler.GetDaysUntilNextReview((Domain.Enums.DifficultyLevel)source.Difficulty))))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(source => source.Active))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
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

            CreateMap<Domain.Entities.Topic, ResponseTopicJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(source => _idEncoder.Encode(source.SubjectId)));

            CreateMap<Domain.Entities.Review, ResponseCreatedReviewJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(source => source.Topic.Name));

            CreateMap<Domain.Entities.Review, ResponseShortReviewJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(source => source.Topic.Name));
        }
    }
}
