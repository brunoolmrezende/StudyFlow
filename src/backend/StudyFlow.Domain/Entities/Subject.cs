namespace StudyFlow.Domain.Entities
{
    public class Subject : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public long UserId { get; set; }
    }
}
