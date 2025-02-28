using CommonTestUtilities.IdEncrypter;
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
    public class UpdateSubjectTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "subject";
        private readonly string _subjectId;
        private readonly Guid _userIdentifier;

        public UpdateSubjectTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _subjectId = factory.GetSubjectId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateSubjectJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_subjectId}", request: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Subject_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateSubjectJsonBuilder.Build();

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPut(endpoint: $"{_endpoint}/{id}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var experctedMessage = ResourceMessagesException.ResourceManager.GetString("SUBJECT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(experctedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Name_Empty(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPut(endpoint: $"{_endpoint}/{_subjectId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
