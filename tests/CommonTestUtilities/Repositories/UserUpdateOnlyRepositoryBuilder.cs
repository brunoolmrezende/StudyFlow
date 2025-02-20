using Moq;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _repository;

        public UserUpdateOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserUpdateOnlyRepository>();
        }

        public UserUpdateOnlyRepositoryBuilder GetById(User user)
        {
            _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

            return this;
        }

        public IUserUpdateOnlyRepository Build() => _repository.Object;
    }
}
