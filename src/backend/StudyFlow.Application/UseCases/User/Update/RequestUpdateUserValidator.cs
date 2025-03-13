using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.User.Update
{
    public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
    {
        public RequestUpdateUserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.EMAIL_MAX_LENGTH);
                

            When(user => !string.IsNullOrWhiteSpace(user.Email), () =>
            {
                RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.INVALID_EMAIL);
            });

            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);
        }
    }
}
