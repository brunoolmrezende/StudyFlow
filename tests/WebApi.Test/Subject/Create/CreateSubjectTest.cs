using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Subject.Create
{
    public class CreateSubjectTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "subject";
        private readonly Guid _userIdentifier;

        public CreateSubjectTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestCreateSubjectJsonBuilder.Build();

            var response = await DoPost(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
            responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Subject_Name_Empty(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestCreateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);
            
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
