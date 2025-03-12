using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Deactivate
{
    public class DeactivateTopicTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly string _topicId;
        private readonly Guid _userIdentifier;

        public DeactivateTopicTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _topicId = factory.GetTopicId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoPatch($"{_endpoint}/{_topicId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPatch($"{_endpoint}/{id}", token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
