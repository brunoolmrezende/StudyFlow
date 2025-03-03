using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Topic.GetById
{
    public class GetTopicByIdUseCase : IGetTopicByIdUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetTopicByIdUseCase(
            ILoggedUser loggedUser,
            ITopicReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseTopicJson> Execute(long id)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var topic = await _readOnlyRepository.GetTopicById(id, loggedUser);

            if (topic is null)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            return _mapper.Map<ResponseTopicJson>(topic);
        }
    }
}
