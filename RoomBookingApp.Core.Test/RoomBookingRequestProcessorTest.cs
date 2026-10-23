using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;

namespace RoomBookingApp.Core.Test
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {
            // Arrange
            _request = new RoomBookingRequest
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Date = new DateTime(2024, 6, 1)
            };

            _availableRooms = new List<Room>() { new Room() { Id = 1 } };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();

            _roomBookingServiceMock.Setup(x => x.GetAvailableRooms(_request.Date))
                .Returns(_availableRooms);

            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
            // Object is to reach the actual concrete implementation of the IRoomBookingService interface, which is mocked using Moq. The mock allows us to simulate the behavior of the service without relying on a real implementation, making it easier to test the RoomBookingRequestProcessor in isolation.
        }

        [Fact]
        public void Should_Return_RoomBooking_Response_With_Request_Values()
        {
            // Arrange globally

            // Act
            RoomBookingResult result = _processor.BookRoom(_request);
            // Assert
            #region using built in Assertion.
            //Assert.NotNull(result);
            //Assert.Equal(request.FullName, result.FullName);
            //Assert.Equal(request.Email, result.Email);
            //Assert.Equal(request.Date, result.Date); 
            #endregion

            // or i can use Shouldly assertion library to make it more readable
            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.Date.ShouldBe(_request.Date);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Request_Is_Null()
        {
            // here the arrange is not needed because we are testing the null case, so we can directly call the method with null and assert the exception. as all the arrange we needed is the processor object and we have done it once using the constructor.

            // Act & Assert
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_RoomBooking_Request()
        {
            // Arrange
          RoomBooking savedBooking = null;

            // Setup the mock to capture the saved booking
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking => savedBooking = booking);

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);
            // Assert
            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_request.FullName);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.Date.ShouldBe(_request.Date);
            savedBooking.RoomId.ShouldBe(_availableRooms.First().Id); 
        }

        [Fact]
        public void Should_Not_Save_RoomBooking_Request_If_Room_Is_Unavailable()
        {
            _availableRooms.Clear(); // Simulate no available rooms
            // Act
            _processor.BookRoom(_request);
            // Assert
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure , false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingResultFlag , bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableRooms.Clear(); // Simulate no available rooms
            }
            // Act
            var result = _processor.BookRoom(_request);
            // Assert
            bookingResultFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1,true)]
        [InlineData(null,false)]
        public void Should_Return_BookingId_In_Result(int? roomBookingId , bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableRooms.Clear(); // Simulate no available rooms
            }
            else
            {
                _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking => booking.Id = roomBookingId.Value);
            }
            var result = _processor.BookRoom(_request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
