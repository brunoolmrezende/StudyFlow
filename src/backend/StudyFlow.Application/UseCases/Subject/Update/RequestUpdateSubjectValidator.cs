using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.Subject.Update
{
    public class RequestUpdateSubjectValidator : AbstractValidator<RequestUpdateSubjectJson>
    {
        public RequestUpdateSubjectValidator()
        {
            RuleFor(subject => subject.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);

            RuleFor(subject => subject.Active)
                .NotNull()
                .WithMessage(ResourceMessagesException.ACTIVE_STATUS_INVALID);
        }
    }
}
