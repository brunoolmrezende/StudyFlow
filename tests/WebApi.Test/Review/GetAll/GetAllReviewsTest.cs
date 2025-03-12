using System.Net;
using System.Text.Json;
using CommonTestUtilities.Security;
using FluentAssertions;

namespace WebApi.Test.Review.GetAll
{
    public class GetAllReviewsTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "review";
        private readonly Guid _userIdentifier;

        public GetAllReviewsTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoGet(_endpoint, token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("reviews").GetArrayLength().Should().BeGreaterThan(0);
        }
    }
}
