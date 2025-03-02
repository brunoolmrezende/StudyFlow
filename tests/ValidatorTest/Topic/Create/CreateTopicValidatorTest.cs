using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic;
using StudyFlow.Application.UseCases.Topic.Create;
using StudyFlow.Exceptions;

namespace Validators.Test.Topic.Create
{
    public class CreateTopicValidatorTest
    {
        [Fact]
        public void Success()
        {
            var subjectId = IdEncrypterBuilder.Build().Encode(1);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);

            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var subjectId = IdEncrypterBuilder.Build().Encode(1);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);
            request.Name = string.Empty;

            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Name_Max_Length()
        {
            var subjectId = IdEncrypterBuilder.Build().Encode(1);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);
            request.Name = new string('a', 256);

            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_MAX_LENGTH));
        }

        [Fact]
        public void Error_Description_Max_Length()
        {
            var subjectId = IdEncrypterBuilder.Build().Encode(1);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);
            request.Description = new string('a', 256);

            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DESCRIPTION_MAX_LENGTH));
        }

        [Fact]
        public void Error_SubjectId_Empty()
        {
            var subjectId = IdEncrypterBuilder.Build().Encode(1);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);
            request.SubjectId = string.Empty;

            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.SUBJECT_ID_EMPTY));
        }
    }
}
