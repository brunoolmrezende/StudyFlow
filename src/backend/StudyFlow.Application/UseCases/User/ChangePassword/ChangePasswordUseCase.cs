using StudyFlow.Communication.Requests;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IPasswordEncryption _encryption;
        private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser, 
            IPasswordEncryption encryption, 
            IUserUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _encryption = encryption;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            Validate(request, loggedUser);

            var user = await _updateOnlyRepository.GetById(loggedUser.Id);

            user.Password = _encryption.Encrypt(request.NewPassword);

            _updateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
        {
            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            var passwordMatch = _encryption.Decrypt(request.CurrentPassword, loggedUser.Password);

            if (!passwordMatch)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Password not matching.", ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
