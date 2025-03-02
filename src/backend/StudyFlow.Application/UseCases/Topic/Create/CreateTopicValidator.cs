using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.Topic.Create
{
    public class CreateTopicValidator : AbstractValidator<RequestCreateTopicJson>
    {
        public CreateTopicValidator()
        {
            RuleFor(topic => topic.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);

            RuleFor(topic => topic.Description)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.DESCRIPTION_MAX_LENGTH);

            RuleFor(topic => topic.SubjectId)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.SUBJECT_ID_EMPTY);
        }
    }
}
