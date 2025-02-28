using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Subject.Update
{
    public class UpdateSubjectInvalidTokenTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "subject";
        private readonly string _subjectId;

        public UpdateSubjectInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _subjectId = factory.GetSubjectId();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Token(string culture)
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_subjectId}", request: request, token: string.Empty, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_subjectId}", request: request, token: "InvalidToken", culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_With_User_Not_Found(string culture)
        {
            var request = RequestUpdateSubjectJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(Guid.NewGuid());

            var response = await DoPut(endpoint: $"{_endpoint}/{_subjectId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
