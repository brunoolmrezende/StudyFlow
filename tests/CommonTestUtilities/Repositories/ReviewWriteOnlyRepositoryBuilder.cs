using Moq;
using StudyFlow.Domain.Repositories.Review;

namespace CommonTestUtilities.Repositories
{
    public class ReviewWriteOnlyRepositoryBuilder
    {
        public static IReviewWriteOnlyRepository Build()
        {
            var mock = new Mock<IReviewWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
