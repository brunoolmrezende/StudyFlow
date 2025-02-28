using Moq;
using StudyFlow.Domain.Repositories.Subject;

namespace CommonTestUtilities.Repositories
{
    public class SubjectUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ISubjectUpdateOnlyRepository> _mock;

        public SubjectUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ISubjectUpdateOnlyRepository>();
        }

        public SubjectUpdateOnlyRepositoryBuilder GetSubjectById(StudyFlow.Domain.Entities.User loggedUser, StudyFlow.Domain.Entities.Subject? subject)
        {
            if (subject is not null)
            {
                _mock.Setup(repository => repository.GetSubjectById(subject.Id, loggedUser)).ReturnsAsync(subject);
            }

            return this;
        }

        public ISubjectUpdateOnlyRepository Build() => _mock.Object;
    }
}
