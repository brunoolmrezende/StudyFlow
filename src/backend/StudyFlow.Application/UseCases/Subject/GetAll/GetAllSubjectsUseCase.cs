using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Services.LoggedUser;

namespace StudyFlow.Application.UseCases.Subject.GetAll
{
    public class GetAllSubjectsUseCase : IGetAllSubjectsUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ISubjectReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetAllSubjectsUseCase(
            ILoggedUser loggedUser,
            ISubjectReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseSubjectsJson> Execute(bool? active)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var subjects = await _readOnlyRepository.GetAllSubjects(loggedUser, active);

            return new ResponseSubjectsJson
            {
                Subjects = _mapper.Map<IList<ResponseShortSubjectJson>>(subjects),
            };
        }
    }
}
