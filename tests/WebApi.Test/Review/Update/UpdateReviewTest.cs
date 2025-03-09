using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Review.Update
{
    public class UpdateReviewTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "review";
        private readonly Guid _userIdentifier;
        private readonly string _reviewId;

        public UpdateReviewTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _reviewId = factory.GetReviewId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateReviewJsonBuilder.Build();

            var response = await DoPut($"{_endpoint}/{_reviewId}", request, token);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Review_Not_Found(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestUpdateReviewJsonBuilder.Build();

            var id = IdEncrypterBuilder.Build().Encode(1000);

            var response = await DoPut($"{_endpoint}/{id}", request, token, culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("REVIEW_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
