using Bogus;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class ReviewBuilder
    {
        public static Review Build(long topicId)
        {
            return new Faker<Review>()
                .RuleFor(review => review.TopicId, _ => topicId)
                .RuleFor(review => review.Difficulty, faker => faker.PickRandom<DifficultyLevel>())
                .RuleFor(review => review.Status, faker => faker.PickRandom<ReviewStatus>());
        }
    }
}
