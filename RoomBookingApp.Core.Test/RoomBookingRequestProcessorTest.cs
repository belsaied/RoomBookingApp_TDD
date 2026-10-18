using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core.Test
{
    public class RoomBookingRequestProcessorTest
    {
        [Fact]
        public void Should_Return_RoomBooking_Response_With_Request_Values()
        {
            // Arrange
            var request = new RoomBookingRequest
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Date = new DateTime(2024, 6, 1)
            };
            var processor = new RoomBookingRequestProcessor();
            // Act
            RoomBookingResult result = processor.BookRoom(request);
            // Assert
            #region using built in Assertion.
            //Assert.NotNull(result);
            //Assert.Equal(request.FullName, result.FullName);
            //Assert.Equal(request.Email, result.Email);
            //Assert.Equal(request.Date, result.Date); 
            #endregion

            // or i can use Shouldly assertion library to make it more readable
            result.ShouldNotBeNull();
            result.FullName.ShouldBe(request.FullName);
            result.Email.ShouldBe(request.Email);
            result.Date.ShouldBe(request.Date);
        }
    }
}
