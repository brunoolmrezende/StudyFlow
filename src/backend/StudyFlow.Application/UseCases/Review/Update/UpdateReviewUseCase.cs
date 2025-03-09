using AutoMapper;
using StudyFlow.Communication.Requests;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Review.Update
{
    public class UpdateReviewUseCase : IUpdateReviewUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IReviewUpdateOnlyRepository _updateOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewUseCase(
            ILoggedUser loggedUser,
            IReviewUpdateOnlyRepository updateOnlyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateOnlyRepository = updateOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateReviewJson request, long reviewId)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            Validate(request);

            var review = await _updateOnlyRepository.GetReviewById(reviewId, loggedUser) ?? throw new NotFoundException(ResourceMessagesException.REVIEW_NOT_FOUND);

            review = _mapper.Map(request, review);
            review.UpdatedAt = DateTime.UtcNow;

            _updateOnlyRepository.Update(review);

            await _unitOfWork.Commit();
        }

        private static void Validate(RequestUpdateReviewJson request)
        {
            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
