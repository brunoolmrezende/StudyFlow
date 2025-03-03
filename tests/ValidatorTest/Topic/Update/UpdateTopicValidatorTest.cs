using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.Update;
using StudyFlow.Exceptions;

namespace Validators.Test.Topic.Update
{
    public class UpdateTopicValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));
            request.Name = string.Empty;

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Name_Max_Length()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));
            request.Name = new string('a', 256);

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_MAX_LENGTH));
        }

        [Fact]
        public void Error_Description_Max_Length()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));
            request.Description = new string('a', 256);

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public void Error_SubjectId_Empty()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));
            request.SubjectId = string.Empty;

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.SUBJECT_ID_EMPTY));
        }

        [Fact]
        public void Error_Active_Status_Invalid()
        {
            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(1));
            request.Active = null;

            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.ACTIVE_STATUS_INVALID));
        }
    }
}
