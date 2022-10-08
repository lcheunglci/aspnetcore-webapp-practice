using Microsoft.AspNetCore.Mvc.Testing;

namespace GigHub.IntegrationTests
{
    public class UnitTest1
    {
        // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0

        [Fact]
        public async Task HelloWorldTest()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });

            var client = application.CreateClient();
            //...

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

        }

    }
}
