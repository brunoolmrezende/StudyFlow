using Bogus;
using StudyFlow.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class RefreshTokenBuilder
    {
        public static RefreshToken Build(StudyFlow.Domain.Entities.User user)
        {
            return new Faker<RefreshToken>()
                .RuleFor(r => r.Id, _ => 1)
                .RuleFor(r => r.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(r => r.Value, f => f.Lorem.Word())
                .RuleFor(r => r.UserId, _ => user.Id)
                .RuleFor(r => r.User, _ => user);
        }
    }
}
