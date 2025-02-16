using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.User.GetProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
