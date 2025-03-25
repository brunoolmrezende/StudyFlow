namespace StudyFlow.Domain.Services.Email
{
    public interface ISendReviewReminderMail
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
