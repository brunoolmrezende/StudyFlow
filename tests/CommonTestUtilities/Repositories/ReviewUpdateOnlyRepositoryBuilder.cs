using Moq;
using StudyFlow.Domain.Repositories.Review;

namespace CommonTestUtilities.Repositories
{
    public class ReviewUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IReviewUpdateOnlyRepository> _mock;

        public ReviewUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<IReviewUpdateOnlyRepository>();
        }

        public ReviewUpdateOnlyRepositoryBuilder GetReviewById(
            StudyFlow.Domain.Entities.Review? review,
            StudyFlow.Domain.Entities.User loggedUser)
        {
            if (review is not null)
            {
                _mock.Setup(repository => repository.GetReviewById(It.IsAny<long>(), loggedUser)).ReturnsAsync(review);
            }

            return this;
        }

        public IReviewUpdateOnlyRepository Build() => _mock.Object;
    }
}
