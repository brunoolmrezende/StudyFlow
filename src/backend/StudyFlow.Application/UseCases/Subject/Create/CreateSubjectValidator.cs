using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.Subject.Create
{
    public class CreateSubjectValidator : AbstractValidator<RequestCreateSubjectJson>
    {
        public CreateSubjectValidator()
        {
            RuleFor(subject => subject.Name).NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY);
        }
    }
}
