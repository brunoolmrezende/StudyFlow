using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.Create;
using StudyFlow.Communication.Enums;
using StudyFlow.Exceptions;

namespace Validators.Test.Review.Create
{
    public class CreateReviewValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestCreateReviewJsonBuilder.Build(1);

            var validator = new CreateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_TopicId_Empty()
        {
            var request = RequestCreateReviewJsonBuilder.Build(1);
            request.TopicId = string.Empty;

            var validator = new CreateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.TOPIC_ID_EMPTY));
        }

        [Fact]
        public void Error_Difficulty_Level_Not_Supported()
        {
            var request = RequestCreateReviewJsonBuilder.Build(1);
            request.Difficulty = (DifficultyLevel)1000;

            var validator = new CreateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Status_Value_Not_Supported()
        {
            var request = RequestCreateReviewJsonBuilder.Build(1);
            request.Status = (ReviewStatus)1000;

            var validator = new CreateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.STATUS_VALUE_NOT_SUPPORTED));
        }
    }
}
