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

        protected async Task<HttpResponseMessage> DoPost(string endpoint, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(endpoint, request);
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Accept-anguage");
            }

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }
    }
}
