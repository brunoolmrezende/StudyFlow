using AutoMapper;
using FluentValidation.Results;
using Sqids;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Review.Create
{
    public class CreateReviewUseCase : ICreateReviewUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicReadOnlyRepository _topicReadOnlyRepository;
        private readonly SqidsEncoder<long> _idEncoder;
        private readonly IMapper _mapper;
        private readonly IReviewWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewReadOnlyRepository _readOnlyRepository;

        public CreateReviewUseCase(
            ILoggedUser loggedUser,
            ITopicReadOnlyRepository topicReadOnlyRepository,
            SqidsEncoder<long> idEncoder,
            IMapper mapper,
            IReviewWriteOnlyRepository writeOnlyRepository,
            IUnitOfWork unitOfWork,
            IReviewReadOnlyRepository readOnlyRepository)
        {
            _loggedUser = loggedUser;
            _topicReadOnlyRepository = topicReadOnlyRepository;
            _idEncoder = idEncoder;
            _mapper = mapper;
            _writeOnlyRepository = writeOnlyRepository;
            _unitOfWork = unitOfWork;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ResponseCreatedReviewJson> Execute(RequestCreateReviewJson request)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            await Validate(request, loggedUser);

            var review = _mapper.Map<Domain.Entities.Review>(request);

            review.UserId = loggedUser.Id;

            await _writeOnlyRepository.Add(review);

            await _unitOfWork.Commit();

            var reviewWithTopicName = await _readOnlyRepository.GetReviewById(review.Id, loggedUser);

            return _mapper.Map<ResponseCreatedReviewJson>(reviewWithTopicName);
        }

        private async Task Validate(RequestCreateReviewJson request, Domain.Entities.User loggedUser)
        {
            var validator = new CreateReviewValidator();

            var result = validator.Validate(request);

            var topicId = _idEncoder.Decode(request.TopicId)[0];

            var topicExists = await _topicReadOnlyRepository.GetTopicById(topicId, loggedUser);

            if (topicExists is null)
            {
                result.Errors.Add(new ValidationFailure("Topic not found", ResourceMessagesException.TOPIC_NOT_FOUND));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
