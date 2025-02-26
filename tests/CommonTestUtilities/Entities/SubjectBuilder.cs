using Bogus;
using StudyFlow.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class SubjectBuilder
    {
        public static IList<Subject> Collection(User user, uint count = 2)
        {
            var list = new List<Subject>();

            if (count == 0)
            {
                count = 1;
            }

            var subjectId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeSubject = Build(user);
                fakeSubject.Id = subjectId++;

                list.Add(fakeSubject);
            }

            return list;
        }

        public static Subject Build(User user)
        {
            return new Faker<Subject>()
                .RuleFor(subject => subject.Id, _ => 1)
                .RuleFor(subject => subject.Name, faker => faker.Commerce.ProductName())
                .RuleFor(subject => subject.UserId, _ => user.Id);
        }
    }
}
