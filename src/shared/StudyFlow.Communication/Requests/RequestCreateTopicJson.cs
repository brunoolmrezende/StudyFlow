namespace StudyFlow.Communication.Requests
{
    public class RequestCreateTopicJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
    }
}
