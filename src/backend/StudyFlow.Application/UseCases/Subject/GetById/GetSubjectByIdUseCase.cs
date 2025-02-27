using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Subject.GetById
{
    public class GetSubjectByIdUseCase : IGetSubjectByIdUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ISubjectReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetSubjectByIdUseCase(
            ILoggedUser loggedUser,
            ISubjectReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseSubjectJson> Execute(long id)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var subject = await _readOnlyRepository.GetSubjectById(id, loggedUser);

            if (subject is null)
            {
                throw new NotFoundException(ResourceMessagesException.SUBJECT_NOT_FOUND);
            }

            return _mapper.Map<ResponseSubjectJson>(subject);
        }
    }
}
