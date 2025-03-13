using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.Update;
using StudyFlow.Exceptions;

namespace Validators.Test.Subject.Update
{
    public class UpdateSubjectValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();

            var validator = new RequestUpdateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Empty_Name()
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var validator = new RequestUpdateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Max_Length_Name()
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();
            request.Name = new string('a', 256);

            var validator = new RequestUpdateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_MAX_LENGTH));
        }

        [Fact]
        public void Error_Active_Status_Invalid()
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();
            request.Active = null;

            var validator = new RequestUpdateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.ACTIVE_STATUS_INVALID));
        }
    }
}
