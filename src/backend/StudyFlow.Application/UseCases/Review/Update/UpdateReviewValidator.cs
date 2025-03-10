using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.Review.Update
{
    public class UpdateReviewValidator : AbstractValidator<RequestUpdateReviewJson>
    {
        public UpdateReviewValidator()
        {
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

            RuleFor(review => review.Active)
               .NotNull()
               .WithMessage(ResourceMessagesException.ACTIVE_STATUS_INVALID);
        }
    }
}
