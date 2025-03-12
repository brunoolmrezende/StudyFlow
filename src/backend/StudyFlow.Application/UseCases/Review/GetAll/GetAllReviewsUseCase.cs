using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Domain.Services.LoggedUser;

namespace StudyFlow.Application.UseCases.Review.GetAll
{
    public class GetAllReviewsUseCase : IGetAllReviewsUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IReviewReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetAllReviewsUseCase(
            ILoggedUser loggedUser,
            IReviewReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<ResponseReviewsJson> Execute(bool? active)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            var reviews = await _readOnlyRepository.GetAllReviews(loggedUser, active);

            return new ResponseReviewsJson
            {
                Reviews = _mapper.Map<IList<ResponseShortReviewJson>>(reviews)
            };
        }
    }
}
