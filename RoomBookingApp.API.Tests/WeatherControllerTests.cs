using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.API.Controllers;
using Shouldly;

namespace RoomBookingApp.API.Tests
{
    public class WeatherControllerTests
    {
        [Fact]
        public void Should_Return_Forecast_Results()
        {
            // Arrange
            var logger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(logger.Object);

            // Act
            var result = controller.Get();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
        }
    }
}
