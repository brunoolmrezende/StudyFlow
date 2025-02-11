using AutoMapper;
using FluentValidation.Results;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncryption _encryption;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserUseCase(
            IMapper mapper,
            IPasswordEncryption encryption,
            IUserReadOnlyRepository readOnlyRepository,
            IUserWriteOnlyRepository writeOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _encryption = encryption;
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _encryption.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = request.Name
            };
            
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailAlreadyRegistered = await _readOnlyRepository.IsEmailRegisteredAndActive(request.Email);

            if (emailAlreadyRegistered)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
