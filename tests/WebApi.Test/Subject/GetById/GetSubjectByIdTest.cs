using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Subject.GetById
{
    public class GetSubjectByIdTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "subject";
        private readonly Guid _userIdentifier;
        private readonly string _subjectId;
        private readonly string _subjectName;

        public GetSubjectByIdTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _subjectId = factory.GetSubjectId();
            _subjectName = factory.GetSubjectName();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoGet(endpoint: $"{_endpoint}/{_subjectId}", token: token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetString().Should().Be(_subjectId);
            responseData.RootElement.GetProperty("name").GetString().Should().Be(_subjectName);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Subject_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoGet(endpoint: $"{_endpoint}/{id}", token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("SUBJECT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
