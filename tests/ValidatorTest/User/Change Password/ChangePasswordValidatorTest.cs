using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.ChangePassword;
using StudyFlow.Exceptions;

namespace Validators.Test.User.Change_Password
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Password_Empty()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMPTY_PASSWORD));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Error_Password_Invalid(int passwordLength)
        {
            var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_LENGTH));
        }
    }
}
