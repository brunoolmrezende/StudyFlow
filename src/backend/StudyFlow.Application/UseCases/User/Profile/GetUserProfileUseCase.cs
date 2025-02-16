
using AutoMapper;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Services.LoggedUser;

namespace StudyFlow.Application.UseCases.User.GetProfile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;

        public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {
            var user = await _loggedUser.GetLoggedUser();

            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
