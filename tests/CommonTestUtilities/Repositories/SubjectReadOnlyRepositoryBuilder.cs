using Moq;
using StudyFlow.Domain.Entities;
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

        public SubjectReadOnlyRepositoryBuilder IsSubjectCreatedAndActive(User user, string? name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _mock.Setup(repository => repository.IsSubjectCreatedAndActive(name, user)).ReturnsAsync(true);
            }

            return this;
        }

        public SubjectReadOnlyRepositoryBuilder GetAllSubjects(User user, IList<Subject> subjects)
        {
           _mock.Setup(repository => repository.GetAllSubjects(user)).ReturnsAsync(subjects);

            return this;
        }

        public ISubjectReadOnlyRepository Build() => _mock.Object;
    }
}
