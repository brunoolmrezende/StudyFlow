using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Exceptions;

namespace Validators.Test.Subject.Create
{
    public class CreateSubjectValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestCreateSubjectJsonBuilder.Build();

            var validator = new CreateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Empty_Name()
        {
            var request = RequestCreateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var validator = new CreateSubjectValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }
    }
}
