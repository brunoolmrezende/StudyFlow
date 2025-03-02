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

        public SubjectReadOnlyRepositoryBuilder IsSubjectAlreadyCreated(User user, string? name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _mock.Setup(repository => repository.IsSubjectAlreadyCreated(name, user)).ReturnsAsync(true);
            }

            return this;
        }

        public SubjectReadOnlyRepositoryBuilder GetAllSubjects(User user, IList<Subject> subjects)
        {
           _mock.Setup(repository => repository.GetAllSubjects(user, null)).ReturnsAsync(subjects);

            return this;
        }

        public SubjectReadOnlyRepositoryBuilder GetSubjectById(User user, Subject subject)
        {
            _mock.Setup(repository => repository.GetSubjectById(subject.Id, user)).ReturnsAsync(subject);

            return this;
        }

        public ISubjectReadOnlyRepository Build() => _mock.Object;
    }
}
