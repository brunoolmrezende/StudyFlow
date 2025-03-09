using Bogus;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class ReviewBuilder
    {
        public static Review Build(User user, long topicId)
        {
            return new Faker<Review>()
                .RuleFor(review => review.TopicId, _ => topicId)
                .RuleFor(review => review.Difficulty, faker => faker.PickRandom<DifficultyLevel>())
                .RuleFor(review => review.Status, faker => faker.PickRandom<ReviewStatus>())
                .RuleFor(review => review.ScheduledDate, _ => DateTime.UtcNow.AddDays(1))
                .RuleFor(review => review.UserId, _ => user.Id);
        }
    }
}
