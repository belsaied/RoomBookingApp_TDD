using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }
        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if(bookingRequest is null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);

            var result = CreateRoomBookingObj<RoomBookingResult>(bookingRequest);
            if (availableRooms.Any())
            {
                var rooms = availableRooms.First();
                var roomBooking = CreateRoomBookingObj<RoomBooking>(bookingRequest);
                roomBooking.RoomId = rooms.Id;
                // Save the booking request to the database using the IRoomBookingService
                _roomBookingService.Save(roomBooking);

                result.RoomBookingId = roomBooking.Id;
                result.Flag = BookingResultFlag.Success;
            }
            else
            {
                result.Flag = BookingResultFlag.Failure;
            }

            // Return a RoomBookingResult object with the same values as the bookingRequest
            return result;
        }

        private static TRoomBooking CreateRoomBookingObj<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking : RoomBookingBase , new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date
            };
        }
    }
}
