
using Microsoft.Extensions.Logging;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Domain.Services.Email;

namespace StudyFlow.Application.UseCases.Review.Reminder
{
    public class ReviewReminderUseCase : IReviewReminderUseCase
    {
        private readonly IReviewReadOnlyRepository _readOnlyRepository;
        private readonly ILogger<ReviewReminderUseCase> _logger;
        private readonly ISendReviewReminderMail _mail;

        public ReviewReminderUseCase(
            IReviewReadOnlyRepository readOnlyRepository,
            ILogger<ReviewReminderUseCase> logger,
            ISendReviewReminderMail mail)
        {
            _readOnlyRepository = readOnlyRepository;
            _logger = logger;
            _mail = mail;
        }

        public async Task Execute()
        {
            var reviews = await _readOnlyRepository.GetReviewsForReminderAsync();

            if (!reviews.Any())
            {
                _logger.LogInformation("Nenhuma revisão para enviar lembrete no intervalo especificado.");
                return;
            }

            var taskList = new List<Task>();

            foreach (var review in reviews)
            {
                var emailTask = _mail.SendEmailAsync(
                    review.User.Email,
                    "Lembrete de Revisão",
                    $"Sua revisão para o tópico {review.Topic.Name} está programada para as {review.ScheduledDate:dd/MM/yyyy 'às' HH:mm}."
                );

                taskList.Add(emailTask);
            }

            await Task.WhenAll(taskList);

            var succesSend = taskList.Where(x => x.IsCompletedSuccessfully);

            _logger.LogInformation($"{succesSend.Count()} e-mails enviados com sucesso.");
        }
    }
}
