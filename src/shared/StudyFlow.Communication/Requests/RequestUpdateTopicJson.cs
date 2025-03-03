namespace StudyFlow.Communication.Requests
{
    public class RequestUpdateTopicJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SubjectId { get; set; } = string.Empty;
        public bool? Active { get; set; }
    }
}
