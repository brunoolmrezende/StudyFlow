using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Change_Password
{
    public class ChangePasswordTest : StudyFlowClassFixture
    {
        private readonly string _endpoint = "user/change-password";
        private readonly Guid _userIdentifier;
        private readonly string _userCurrentPassword;
        private readonly string _userEmail;

        public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
            _userCurrentPassword = factory.GetPassword();
            _userEmail = factory.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestChangePasswordJsonBuilder.Build();
            request.CurrentPassword = _userCurrentPassword;

            var response = await DoPut(_endpoint, request, token);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var loginRequest = new RequestDoLoginJson
            {
                Email = _userEmail,
                Password = _userCurrentPassword,
            };

            response = await DoPost("login", loginRequest, token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;

            response = await DoPost("login", loginRequest, token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Current_Password_Invalid(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().GenerateToken(_userIdentifier);

            var request = RequestChangePasswordJsonBuilder.Build();

            var response = await DoPut(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
