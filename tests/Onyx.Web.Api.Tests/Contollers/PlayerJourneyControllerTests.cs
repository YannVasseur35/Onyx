using Onyx.Application.Dtos;
using System.Diagnostics;
using System.Net.Http;

namespace Onyx.Web.Api.Tests.Contollers
{
    public class PlayerJourneyControllerTests : IDisposable
    {
        //https://code-maze.com/dotnet-test-rest-api-xunit/

        //https://timdeschryver.dev/blog/how-to-test-your-csharp-web-api#a-simple-test

        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7006") };
        private const string baseEndPoint = "/api/playerjourneys";

        [Fact]
        public async Task TestUnTruc()
        {
            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent = new[]
            {
                new PlayerJourneyDto(),
            };
            var stopwatch = Stopwatch.StartNew();

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}");

            //Assert
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        public void Dispose()
        {
            //rollback
        }
    }
}