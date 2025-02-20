
using StudyFlow.Communication.Requests;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(
            ILoggedUser loggedUser, 
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {
           var loggedUser = await _loggedUser.GetLoggedUser();

            await Validate(request, loggedUser.Email);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            user.Email = request.Email;
            user.Name = request.Name;

            _userUpdateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new RequestUpdateUserValidator();

            var result = validator.Validate(request);

            if (currentEmail != request.Email)
            {
                var emailIsRegistered = await _userReadOnlyRepository.IsEmailRegisteredAndActive(request.Email);

                if (emailIsRegistered)
                {
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email already registered.", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
                }
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
