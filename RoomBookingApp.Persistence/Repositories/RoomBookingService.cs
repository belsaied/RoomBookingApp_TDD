using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService (RoomBookingAppDbContext _context): IRoomBookingService
    {
        public IEnumerable<Room> GetAvailableRooms(DateTime date)
            => _context.Rooms
                .Where(r => !r.RoomBookings.Any(rb => rb.Date == date))
                .ToList();

        public void Save(RoomBooking roomBooking)
        {
            _context.RoomBookings.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
