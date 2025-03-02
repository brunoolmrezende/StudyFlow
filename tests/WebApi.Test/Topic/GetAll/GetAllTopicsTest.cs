using CommonTestUtilities.Security;
using FluentAssertions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Topic.GetAll
{
    public class GetAllTopicsTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly Guid _userIdentifier;

        public GetAllTopicsTest(CustomWebApplicationFactory factory) : base(factory)
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

            responseData.RootElement.GetProperty("topics").GetArrayLength().Should().BeGreaterThan(0);
        }
    }
}
