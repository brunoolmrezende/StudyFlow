using System.Net.Http.Json;

namespace WebApi.Test
{
    public class StudyFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public StudyFlowClassFixture(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(string endpoint, object request)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, request);
        }
    }
}
