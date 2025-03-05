using StudyFlow.Communication.Enums;

namespace StudyFlow.Communication.Requests
{
    public class RequestCreateReviewJson
    {
        public string TopicId { get; set; } = string.Empty;
        public DifficultyLevel Difficulty { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
