using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Review.Create
{
    public class CreateReviewTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "review";
        private readonly Guid _userIdentifier;
        private readonly long _longTopicId;
        private readonly string _topicName;

        public CreateReviewTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _longTopicId = factory.GetLongTopicId();
            _topicName = factory.GetTopicName();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestCreateReviewJsonBuilder.Build(_longTopicId);

            var response = await DoPost(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetString().Should().NotBeEmpty();
            responseData.RootElement.GetProperty("topicName").GetString().Should().Be(_topicName);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var topicId = 1000;

            var request = RequestCreateReviewJsonBuilder.Build(topicId);

            var response = await DoPost(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
