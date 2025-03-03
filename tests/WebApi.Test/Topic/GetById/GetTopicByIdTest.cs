using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Topic.GetById
{
    public class GetTopicByIdTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly string _topicId;
        private readonly string _topicName;
        private readonly Guid _userIdentifier;

        public GetTopicByIdTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _topicId = factory.GetTopicId();
            _topicName = factory.GetTopicName();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoGet($"{_endpoint}/{_topicId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetString().Should().Be(_topicId);
            responseData.RootElement.GetProperty("name").GetString().Should().Be(_topicName);
            responseData.RootElement.GetProperty("subjectId").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var topicId = 1000;

            var response = await DoGet($"{_endpoint}/{topicId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(ResourceMessagesException.TOPIC_NOT_FOUND));
        }
    }
}
