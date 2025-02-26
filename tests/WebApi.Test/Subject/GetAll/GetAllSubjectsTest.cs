using CommonTestUtilities.Security;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Subject.GetAll
{
    public class GetAllSubjectsTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "subject";
        private readonly Guid _userIdentifier;

        public GetAllSubjectsTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoGet(_endpoint, token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("subjects").GetArrayLength().Should().BeGreaterThan(0);
        }
    }
}
