using StudyFlow.Communication.Enums;

namespace StudyFlow.Communication.Requests
{
    public class RequestUpdateReviewJson
    {
        public DateTime? ScheduledDate { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
