using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.Deactivate;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Review.Deactivate
{
    public class DeactivateReviewUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var useCase = CreateUseCase(user, review);

            Func<Task> act = async () => await useCase.Execute(review.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Errror_Review_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(review.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count() == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.REVIEW_NOT_FOUND));
        }

        private static DeactivateReviewUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user,
            StudyFlow.Domain.Entities.Review? review = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateOnlyRepository = new ReviewUpdateOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (review is not null)
            {
                updateOnlyRepository.GetReviewById(review, user);
            }

            return new DeactivateReviewUseCase(loggedUser, updateOnlyRepository.Build(), unitOfWork);
        }
    }
}
