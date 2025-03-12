using Moq;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Review;

namespace CommonTestUtilities.Repositories
{
    public class ReviewReadOnlyRepositoryBuilder
    {
        private readonly Mock<IReviewReadOnlyRepository> _mock;

        public ReviewReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<IReviewReadOnlyRepository>();
        }

        public ReviewReadOnlyRepositoryBuilder GetAllReviews(IList<Review> reviews, User loggedUser)
        {
            _mock.Setup(repository => repository.GetAllReviews(loggedUser, null, null, null)).ReturnsAsync((reviews));

            return this;
        }

        public ReviewReadOnlyRepositoryBuilder GetReviewById(Review? review, User loggedUser)
        {
            if (review is not null)
            {
                _mock.Setup(repository => repository.GetReviewById(It.IsAny<long>(), loggedUser)).ReturnsAsync(review);
            }

            return this;
        }

        public IReviewReadOnlyRepository Build() => _mock.Object;
    }
}
