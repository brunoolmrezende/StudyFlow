using Moq;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Services.LoggedUser;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();

            mock.Setup(loggedUser => loggedUser.GetLoggedUser()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
