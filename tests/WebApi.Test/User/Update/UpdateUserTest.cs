using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "user";
        private readonly Guid _userIdentifier;

        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Success(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Name_Empty(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPut(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectdeMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectdeMessage));
        }
    }
}
