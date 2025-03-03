using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Update
{
    public class UpdateTopicTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly string _subjectId;
        private readonly string _topicId;
        private readonly string _topic2Name;
        private readonly Guid _userIdentifier;

        public UpdateTopicTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _subjectId = factory.GetSubjectId();
            _userIdentifier = factory.GetUserIdentifier();
            _topicId = factory.GetTopicId();
            _topic2Name = factory.GetSecondTopicName();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);

            var response = await DoPut($"{_endpoint}/{_topicId}", request, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPut($"{_endpoint}/{id}", request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessages = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessages));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Already_Created(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateTopicJsonBuilder.Build(_subjectId);
            request.Name = _topic2Name;

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPut($"{_endpoint}/{_topicId}", request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessages = ResourceMessagesException.ResourceManager.GetString("TOPIC_ALREADY_CREATED", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessages));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Subject_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var subjectId = IdEncrypterBuilder.Build().Encode(1000);

            var request = RequestUpdateTopicJsonBuilder.Build(subjectId);

            var response = await DoPut($"{_endpoint}/{_topicId}", request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessages = ResourceMessagesException.ResourceManager.GetString("SUBJECT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessages));
        }
    }
}
