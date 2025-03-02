using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Domain.Services.LoggedUser;

namespace StudyFlow.Application.UseCases.Topic.GetAll
{
    public class GetAllTopicsUseCase : IGetAllTopicsUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetAllTopicsUseCase(
            ILoggedUser loggedUser,
            ITopicReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseTopicsJson> Execute(bool? active)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var topics = await _readOnlyRepository.GetAllTopics(loggedUser, active);

            return new ResponseTopicsJson
            {
               Topics = _mapper.Map<IList<ResponseShortTopicJson>>(topics),
            };
        }
    }
}
