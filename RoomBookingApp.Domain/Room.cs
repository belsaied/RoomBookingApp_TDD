namespace RoomBookingApp.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<RoomBooking> RoomBookings { get; set; } = new List<RoomBooking>();
        // 1:M relationship with RoomBooking
    }
}
