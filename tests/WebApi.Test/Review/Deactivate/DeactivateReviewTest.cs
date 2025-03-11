using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Review.Deactivate
{
    public class DeactivateReviewTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "review";
        private readonly string _reviewId;
        private readonly Guid _userIdentifier;

        public DeactivateReviewTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _reviewId = factory.GetReviewId();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var response = await DoPatch($"{_endpoint}/{_reviewId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Review_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPatch($"{_endpoint}/{id}", token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("REVIEW_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
