using CommonTestUtilities.Security;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "user";
        private readonly Guid _userIdentifier;
        private readonly string _userEmail;
        private readonly string _userName;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _userEmail = factory.GetEmail();
            _userName = factory.GetUserName();  
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Success(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoGet(_endpoint, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(_userName);
            responseData.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
        }


    }
}
