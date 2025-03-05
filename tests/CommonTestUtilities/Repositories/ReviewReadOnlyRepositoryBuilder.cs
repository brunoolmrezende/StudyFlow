using Moq;
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

        public ReviewReadOnlyRepositoryBuilder GetReviewById(
            StudyFlow.Domain.Entities.Review? review, 
            StudyFlow.Domain.Entities.User loggedUser)
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
