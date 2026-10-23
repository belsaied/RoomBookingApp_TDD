using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Test
{
    public class RoomBookingServiceTest
    { 

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // arrange
            var date = new DateTime(2026, 10, 23);
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase(databaseName: "AvailableRoomsTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { Id = 1, RoomId = 1, Date = date });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, Date = date.AddDays(-1) });
            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // assert
            Assert.Equal(2, availableRooms.Count());
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
        }


        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldSaveTest")
            .Options;

            var roomBooking = new RoomBooking
            {
                RoomId = 1,
                Date = new DateTime(2026, 10, 23)
            };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);
            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            // assert
            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
        }


    }
}
