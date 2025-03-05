using Bogus;
using CommonTestUtilities.IdEncrypter;
using StudyFlow.Communication.Enums;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestCreateReviewJsonBuilder
    {
        public static RequestCreateReviewJson Build(long topicId)
        {
            return new Faker<RequestCreateReviewJson>()
                .RuleFor(request => request.TopicId, _ => IdEncrypterBuilder.Build().Encode(topicId))
                .RuleFor(request => request.Status, faker => faker.PickRandom<ReviewStatus>())
                .RuleFor(request => request.Difficulty, faker => faker.PickRandom<DifficultyLevel>());
        }
    }
}
