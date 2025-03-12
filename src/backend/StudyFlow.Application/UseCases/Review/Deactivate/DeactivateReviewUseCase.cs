using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Review.Deactivate
{
    public class DeactivateReviewUseCase : IDeactivateReviewUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IReviewUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateReviewUseCase(
            ILoggedUser loggedUser,
            IReviewUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var review = await _updateOnlyRepository.GetReviewById(id, loggedUser) ?? throw new NotFoundException(ResourceMessagesException.REVIEW_NOT_FOUND);

            review.Active = false;
            review.UpdatedAt = DateTime.UtcNow;

            _updateOnlyRepository.Update(review);
            await _unitOfWork.Commit();
        }
    }
}
