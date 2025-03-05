using StudyFlow.Domain.Enums;

namespace StudyFlow.Domain.Entities
{
    public class Review : EntityBase
    {
        public long UserId { get; set; }  
        public long TopicId { get; set; }
        public DateTime ScheduledDate { get; private set; }
        public DifficultyLevel Difficulty { get; set; }  
        public ReviewStatus Status { get; set; } 

        public Topic Topic { get; set; } = null!;

        public void ScheduleNextReview()
        {
            ScheduledDate = DateTime.UtcNow.AddDays(ReviewScheduler.GetDaysUntilNextReview(Difficulty));
        }
    }
}
