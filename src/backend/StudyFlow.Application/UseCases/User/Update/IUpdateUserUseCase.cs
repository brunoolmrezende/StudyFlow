using StudyFlow.Communication.Requests;

namespace StudyFlow.Application.UseCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        Task Execute(RequestUpdateUserJson request);
    }
}
