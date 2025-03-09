using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.Create;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Review.Create
{
    public class CreateReviewUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var request = RequestCreateReviewJsonBuilder.Build(topic.Id);

            var useCase = CreateUseCase(user, topic, review);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.TopicName.Should().Be(topic.Name);
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var review = ReviewBuilder.Build(user, topic.Id);

            var request = RequestCreateReviewJsonBuilder.Build(topic.Id);

            var useCase = CreateUseCase(user: user ,review: review);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(errors => errors.GetErrorMessages().Count == 1
                    && errors.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static CreateReviewUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user, 
            StudyFlow.Domain.Entities.Topic? topic = null,
            StudyFlow.Domain.Entities.Review? review = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var idEncoder = IdEncrypterBuilder.Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var topicReadOnlyRepository = new TopicReadOnlyRepositoryBuilder();
            var writeOnlyRepository = ReviewWriteOnlyRepositoryBuilder.Build();
            var readOnlyRepository = new ReviewReadOnlyRepositoryBuilder();

            if (topic is not null)
            {
                topicReadOnlyRepository.GetTopicById(user, topic);
            }

            if (review is not null)
            {
                review.Topic = topic;
                readOnlyRepository.GetReviewById(review, user);
            }

            return new CreateReviewUseCase(
                loggedUser, 
                topicReadOnlyRepository.Build(), 
                idEncoder, 
                mapper, 
                writeOnlyRepository, 
                unitOfWork, 
                readOnlyRepository.Build());
        }
    }
}
