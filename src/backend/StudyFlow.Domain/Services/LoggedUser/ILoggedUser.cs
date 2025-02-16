using StudyFlow.Domain.Entities;

namespace StudyFlow.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        Task<User> GetLoggedUser();
    }
}
