using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Update
{
    public class UpdateTopicInvalidTokenTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly string _subjectId;
        private readonly string _topicId;

        public UpdateTopicInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _subjectId = factory.GetSubjectId();
            _topicId = factory.GetTopicId();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Token(string culture)
        {
            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: string.Empty, culture: culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

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
            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: "InvalidToken", culture: culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

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
            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);

            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(Guid.NewGuid());

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
