using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.GetAll;
using StudyFlow.Communication.Enums;

namespace UseCases.Test.Review.GetAll
{
    public class GetAllReviewsUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var reviews = ReviewBuilder.Collection(user, 1);

            var useCase = CreateUseCase(user, reviews);

            var result = await useCase.Execute(null);

            result.Should().NotBeNull();
            result.Reviews.Should()
                .HaveCountGreaterThan(0)
                .And.OnlyHaveUniqueItems(review => review.Id)
                .And.AllSatisfy(review =>
                {
                    review.Id.Should().NotBeNullOrWhiteSpace();
                    review.Status.Should().Match(status => Enum.IsDefined(typeof(ReviewStatus), status!));
                    review.Difficulty.Should().Match(difficulty => Enum.IsDefined(typeof(DifficultyLevel), difficulty!));
                });
        }

        private static GetAllReviewsUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, IList<StudyFlow.Domain.Entities.Review> reviews)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var readOnlyRepository = new ReviewReadOnlyRepositoryBuilder().GetAllReviews(reviews, user).Build();

            return new GetAllReviewsUseCase(loggedUser, readOnlyRepository, mapper);
        }
    }
}
