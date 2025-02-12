using FluentAssertions;
using StudyFlow.Communication.Requests;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _userName;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _userName = factory.GetUserName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestDoLoginJson
            {
                Email = _email,
                Password = _password,
            };

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var result = responseData.RootElement.GetProperty("name").GetString();

            result.Should().Be(_userName);
        }
    }
}
