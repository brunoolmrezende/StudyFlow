using Moq;
using StudyFlow.Domain.Repositories.Subject;

namespace CommonTestUtilities.Repositories
{
    public class SubjectReadOnlyRepositoryBuilder
    {
        private readonly Mock<ISubjectReadOnlyRepository> _mock;

        public SubjectReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<ISubjectReadOnlyRepository>();
        }

        public SubjectReadOnlyRepositoryBuilder IsSubjectCreatedAndActive(StudyFlow.Domain.Entities.User user, string? name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _mock.Setup(repository => repository.IsSubjectCreatedAndActive(name, user)).ReturnsAsync(true);
            }

            return this;
        }

        public ISubjectReadOnlyRepository Build() => _mock.Object;
    }
}
