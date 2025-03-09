using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Review.Update;
using StudyFlow.Communication.Enums;
using StudyFlow.Exceptions;

namespace Validators.Test.Review.Update
{
    public class UpdateReviewValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestUpdateReviewJsonBuilder.Build();

            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Difficulty_Level_Not_Supported()
        {
            var request = RequestUpdateReviewJsonBuilder.Build();
            request.Difficulty = (DifficultyLevel)1000;

            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Status_Value_Not_Supported()
        {
            var request = RequestUpdateReviewJsonBuilder.Build();
            request.Status = (ReviewStatus)1000;

            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.STATUS_VALUE_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Date_Cannot_Be_In_The_Past()
        {
            var request = RequestUpdateReviewJsonBuilder.Build();
            request.ScheduledDate = DateTime.UtcNow.AddDays(-1);

            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DATE_CANNOT_BE_IN_THE_PAST));
        }

        [Fact]
        public void Error_Date_Cannot_Be_More_Than_1_Year_Ahead()
        {
            var request = RequestUpdateReviewJsonBuilder.Build();
            request.ScheduledDate = DateTime.UtcNow.AddYears(2);

            var validator = new UpdateReviewValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DATE_CANNOT_BE_MORE_THAN_1_YEAR_AHEAD));
        }
    }
}
