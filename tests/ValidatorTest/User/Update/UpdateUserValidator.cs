using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.Update;
using StudyFlow.Exceptions;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidator
    {
        [Fact]
        public void Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var validator = new RequestUpdateUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Empty_Name()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var validator = new RequestUpdateUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Empty_Email()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = string.Empty;

            var validator = new RequestUpdateUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Invalid_Email()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "emaail.com";

            var validator = new RequestUpdateUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(errors => errors.ErrorMessage.Equals(ResourceMessagesException.INVALID_EMAIL));
        }
    }
}
