
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Topic.Deactivate
{
    public class DeactivateTopicUseCase : IDeactivateTopicUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateTopicUseCase(
            ILoggedUser loggedUser,
            ITopicUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var topic = await _updateOnlyRepository.GetTopicById(id, loggedUser) ?? throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);

            topic.Active = false;
            topic.UpdatedAt = DateTime.UtcNow;

            _updateOnlyRepository.Update(topic);
            await _unitOfWork.Commit();
        }
    }
}
