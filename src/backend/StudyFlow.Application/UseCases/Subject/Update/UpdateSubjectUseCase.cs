using StudyFlow.Communication.Requests;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Subject.Update
{
    public class UpdateSubjectUseCase : IUpdateSubjectUseCase
    {
        private readonly ISubjectUpdateOnlyRepository _updateOnlyRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSubjectUseCase(
            ILoggedUser loggedUser,
            ISubjectUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id, RequestUpdateSubjectJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.GetLoggedUser();

            var subject = await _updateOnlyRepository.GetSubjectById(id, loggedUser);

            if (subject is null)
            {
                throw new NotFoundException(ResourceMessagesException.SUBJECT_NOT_FOUND);
            }

            subject.Name = request.Name;
            subject.Active = (bool)request.Active!;

            _updateOnlyRepository.Update(subject);

            await _unitOfWork.Commit();
        }

        private static void Validate(RequestUpdateSubjectJson request)
        {
            var validator = new RequestUpdateSubjectValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
