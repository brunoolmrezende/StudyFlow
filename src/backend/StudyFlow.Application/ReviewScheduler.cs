namespace StudyFlow.Application
{
    public static class ReviewScheduler
    {
        public static int GetDaysUntilNextReview(Domain.Enums.DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                Domain.Enums.DifficultyLevel.High => 1,
                Domain.Enums.DifficultyLevel.Medium => 3,
                Domain.Enums.DifficultyLevel.Low => 7,
                _ => 0
            };
        }
    }
}
