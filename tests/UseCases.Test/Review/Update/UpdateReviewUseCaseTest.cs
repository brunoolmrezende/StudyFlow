using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.Update;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Review.Update
{
    public class UpdateReviewUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var request = RequestUpdateReviewJsonBuilder.Build();

            var useCase = CreateUseCase(user, review);

            Func<Task> act = async () => await useCase.Execute(request, review.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Review_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var request = RequestUpdateReviewJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request, review.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.Message.Equals(ResourceMessagesException.REVIEW_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Date_Cannot_Be_In_The_Past()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var request = RequestUpdateReviewJsonBuilder.Build();
            request.ScheduledDate = DateTime.UtcNow.AddDays(-1);

            var useCase = CreateUseCase(user, review);

            Func<Task> act = async () => await useCase.Execute(request, review.Id);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.DATE_CANNOT_BE_IN_THE_PAST));
        }

        public static UpdateReviewUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user,
            StudyFlow.Domain.Entities.Review? review = null
            )
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateOnlyRepository = new ReviewUpdateOnlyRepositoryBuilder();

            if (review is not null)
            {
                updateOnlyRepository.GetReviewById(review, user);
            }

            return new UpdateReviewUseCase(loggedUser, updateOnlyRepository.Build(), mapper, unitOfWork);
        }
    }
}
