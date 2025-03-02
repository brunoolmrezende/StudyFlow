using AutoMapper;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Subject.Create
{
    public class CreateSubjectUseCase : ICreateSubjectUseCase
    {
        private readonly ISubjectWriteOnlyRepository _writeOnlyRepository;
        private readonly ISubjectReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public CreateSubjectUseCase(
            ISubjectWriteOnlyRepository writeOnlyRepository,
            ISubjectReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser,
            IMapper mapper)
        {
            
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _mapper = mapper;
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ResponseCreatedSubjectJson> Execute(RequestCreateSubjectJson request)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            await Validate(request, loggedUser);

            var subject = _mapper.Map<Domain.Entities.Subject>(request);
            subject.UserId = loggedUser.Id;

            await _writeOnlyRepository.Add(subject);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseCreatedSubjectJson>(subject);
        }

        private async Task Validate(RequestCreateSubjectJson request, Domain.Entities.User loggedUser)
        {
            var validator = new CreateSubjectValidator();

            var result = validator.Validate(request);

            var subjectAlreadyCreated = await _readOnlyRepository.IsSubjectAlreadyCreated(request.Name, loggedUser);

            if (subjectAlreadyCreated)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Subject already exists.", ResourceMessagesException.SUBJECT_ALREADY_CREATED));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
