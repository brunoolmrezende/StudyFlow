using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Token.RefreshToken
{
    public interface IUseRefreshTokenUseCase
    {
        Task<ResponseTokenJson> Execute(RequestNewTokenJson request);
    }
}
