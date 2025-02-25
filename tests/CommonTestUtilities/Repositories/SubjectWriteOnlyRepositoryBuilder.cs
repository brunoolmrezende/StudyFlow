using Moq;
using StudyFlow.Domain.Repositories.Subject;

namespace CommonTestUtilities.Repositories
{
    public class SubjectWriteOnlyRepositoryBuilder
    {
        public static ISubjectWriteOnlyRepository Build()
        {
            var mock = new Mock<ISubjectWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
