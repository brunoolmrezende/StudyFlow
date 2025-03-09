using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.Review.Create
{
    public class CreateReviewValidator : AbstractValidator<RequestCreateReviewJson>
    {
        public CreateReviewValidator()
        {
            RuleFor(review => review.TopicId)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.TOPIC_ID_EMPTY);

            RuleFor(review => review.Difficulty)
                .IsInEnum()
                .WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);

            RuleFor(review => review.Status)
                .IsInEnum()
                .WithMessage(ResourceMessagesException.STATUS_VALUE_NOT_SUPPORTED);

            RuleFor(review => review.ScheduledDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(ResourceMessagesException.DATE_CANNOT_BE_IN_THE_PAST)
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage(ResourceMessagesException.DATE_CANNOT_BE_MORE_THAN_1_YEAR_AHEAD)
                .When(review => review.ScheduledDate.HasValue);
        }
    }
}
