using StudyFlow.Communication.Requests;

namespace StudyFlow.Application.UseCases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
