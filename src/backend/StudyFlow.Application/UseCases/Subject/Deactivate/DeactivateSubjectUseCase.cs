using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Subject.Deactivate
{
    public class DeactivateSubjectUseCase : IDeactivateSubjectUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ISubjectUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateSubjectUseCase(
            ILoggedUser loggedUser,
            ISubjectUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var subject = await _updateOnlyRepository.GetSubjectById(id, loggedUser) ?? throw new NotFoundException(ResourceMessagesException.SUBJECT_NOT_FOUND);

            subject.Active = false;
            subject.UpdatedAt = DateTime.UtcNow;

            _updateOnlyRepository.Update(subject);
            await _unitOfWork.Commit();
        }
    }
}
