using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Create
{
    public class CreateTopicTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "topic";
        private readonly Guid _userIdentifier;
        private readonly string _subjectId;

        public CreateTopicTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _subjectId = factory.GetSubjectId();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestCreateTopicJsonBuilder.Build(_subjectId);

            var response = await DoPost(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetString().Should().NotBeEmpty();
            responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Subject_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var subjectId = IdEncrypterBuilder.Build().Encode(1000);

            var request = RequestCreateTopicJsonBuilder.Build(subjectId);

            var response = await DoPost(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("SUBJECT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
