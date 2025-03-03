namespace StudyFlow.Domain.Entities
{
    public class Topic : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long UserId { get; set; }
        public long SubjectId { get; set; }
    }
}
