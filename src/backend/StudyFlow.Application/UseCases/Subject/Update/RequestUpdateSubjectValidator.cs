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
                .WithMessage(ResourceMessagesException.NAME_EMPTY);

            RuleFor(subject => subject.Active)
                .NotNull()
                .WithMessage(ResourceMessagesException.ACTIVE_STATUS_INVALID);
        }
    }
}
