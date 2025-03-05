using StudyFlow.Domain.Enums;

namespace StudyFlow.Domain
{
    public static class ReviewScheduler
    {
        public static int GetDaysUntilNextReview(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.High => 1,
                DifficultyLevel.Medium => 3,
                DifficultyLevel.Low => 7,
                _ => 0
            };
        }
    }
}
