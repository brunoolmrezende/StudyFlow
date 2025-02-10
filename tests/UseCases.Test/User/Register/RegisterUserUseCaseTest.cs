using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.Register;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(errors => errors.GetErrorMessages().Count == 1 
                    && errors.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Empty_Name()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(errors => errors.GetErrorMessages().Count == 1
                    && errors.GetErrorMessages().Contains(ResourceMessagesException.NAME_EMPTY));
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var encryption = new PasswordEncryptionBuilder().Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (!string.IsNullOrWhiteSpace(email))
            {
                readOnlyRepository.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(mapper, encryption, readOnlyRepository.Build(), writeOnlyRepository, unitOfWork);
        }
    }
}
