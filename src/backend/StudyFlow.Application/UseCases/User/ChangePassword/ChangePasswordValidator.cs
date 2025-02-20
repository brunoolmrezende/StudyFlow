using FluentValidation;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;

namespace StudyFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(user => user.NewPassword)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMPTY_PASSWORD);

            When(user => !string.IsNullOrWhiteSpace(user.NewPassword), () =>
            {
                RuleFor(user => user.NewPassword)
                    .MinimumLength(8)
                    .WithMessage(ResourceMessagesException.PASSWORD_LENGTH);
            });
        }
    }
}
