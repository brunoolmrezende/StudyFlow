using Cronos;
using StudyFlow.Application.UseCases.Review.Reminder;

namespace StudyFlow.API.Services.ReviewReminder
{
    public class ReviewReminderService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ReviewReminderService> _logger;
        private readonly string _cronExpression = "0 7,19 * * *";

        public ReviewReminderService(
            IServiceProvider services,
            ILogger<ReviewReminderService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cronSchedule = CronExpression.Parse(_cronExpression);
            var nextRunTime = cronSchedule.GetNextOccurrence(DateTime.UtcNow);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (nextRunTime.HasValue) 
                { 
                    var waitTime = nextRunTime.Value - DateTime.UtcNow;
                    _logger.LogInformation($"Próxima execução agendada para {nextRunTime.Value}.");

                    await Task.Delay(waitTime, stoppingToken);

                    await DoWork(stoppingToken);
                }

                nextRunTime = cronSchedule.GetNextOccurrence(DateTime.UtcNow);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de envio de lembretes iniciado.");

            using var scope = _services.CreateScope();

            var reviewReminderService = scope.ServiceProvider.GetRequiredService<IReviewReminderUseCase>();

            await reviewReminderService.Execute();

            _logger.LogInformation("Serviço de envio de lembretes finalizado.");
        }
    }
}