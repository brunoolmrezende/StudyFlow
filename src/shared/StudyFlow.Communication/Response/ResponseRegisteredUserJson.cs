namespace StudyFlow.Communication.Response
{
    public class ResponseRegisteredUserJson
    {
        public string Name { get; set; } = string.Empty;
        public ResponseTokenJson Tokens { get; set; } = default!;
    }
}
