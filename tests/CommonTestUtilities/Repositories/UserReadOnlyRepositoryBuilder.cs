using Moq;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistActiveUserWithEmail(string email)
        {
            _repository.Setup(repository => repository.IsEmailRegisteredAndActive(email)).ReturnsAsync(true);
        }

        public void GetUserByEmail(User user)
        {
            _repository.Setup(respository => respository.GetUserByEmail(user.Email)).ReturnsAsync(user);
        }

        public IUserReadOnlyRepository Build() => _repository.Object;
    }
}
