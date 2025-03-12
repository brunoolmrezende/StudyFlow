using Bogus;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class ReviewBuilder
    {
        public static IList<Review> Collection(User user, long topicId, uint count = 1)
        {
            var list = new List<Review>();

            if (count == 0)
            {
                count = 1;
            }

            var reviewId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeReview = Build(user, topicId);
                fakeReview.Id = reviewId++;

                list.Add(fakeReview);
            }

            return list;
        }

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
