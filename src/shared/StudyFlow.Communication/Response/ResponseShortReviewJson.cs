using StudyFlow.Communication.Enums;

namespace StudyFlow.Communication.Response
{
    public class ResponseShortReviewJson
    {
        public string Id { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
