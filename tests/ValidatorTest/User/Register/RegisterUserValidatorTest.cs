using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.Register;
using StudyFlow.Exceptions;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Empty_Name()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Empty_Email()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = string.Empty;

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Invalid_Email()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "emaail.com";

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.INVALID_EMAIL));
        }

        [Fact]
        public void Error_Empty_Pasword()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Password = string.Empty;

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.EMPTY_PASSWORD));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Error_Password_Length(int passwordLength)
        {
            var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_LENGTH));
        }
    }
}
