using HotelWebApp.Data;
using HotelWebApp.Model;

namespace HotelWebApp.Repository;

// Room repository mockup
public static class RoomRepository
{
    public static List<Room> Rooms { get; }= DataInitializer.InitializeRooms();
}