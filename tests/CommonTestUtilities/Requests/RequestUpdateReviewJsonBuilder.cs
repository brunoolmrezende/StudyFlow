using Bogus;
using StudyFlow.Communication.Enums;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateReviewJsonBuilder
    {
        public static RequestUpdateReviewJson Build()
        {
            return new Faker<RequestUpdateReviewJson>()
                .RuleFor(request => request.Status, faker => faker.PickRandom<ReviewStatus>())
                .RuleFor(request => request.Difficulty, faker => faker.PickRandom<DifficultyLevel>())
                .RuleFor(request => request.ScheduledDate, _ => DateTime.UtcNow.AddDays(7));
        }
    }
}
