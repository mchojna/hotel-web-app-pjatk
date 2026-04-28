using HotelWebApp.Model;

namespace HotelWebApp.Data;

public static class DataInitializer
{
    public static List<Room> InitializeRooms()
    {
        var rooms = new List<Room>
        {
            new() { Id = 1, Name = "Sala Konferencyjna A", Capacity = 20 },
            new() { Id = 2, Name = "Sala Konferencyjna B", Capacity = 10 },
            new() { Id = 3, Name = "Sala VIP", Capacity = 5 }
        };

        return rooms;
    }

    public static List<Reservation> InitializeReservations()
    {
        var reservations = new List<Reservation>
        {
            new()
            {
                Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Spotkanie zarządu",
                Date = new DateOnly(2025, 5, 10), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0),
                Status = "confirmed"
            },
            new()
            {
                Id = 2, RoomId = 2, OrganizerName = "Anna Nowak", Topic = "Szkolenie HR",
                Date = new DateOnly(2025, 5, 11), StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(15, 0),
                Status = "planned"
            },
            new()
            {
                Id = 3, RoomId = 1, OrganizerName = "Piotr Wiśniewski", Topic = "Prezentacja projektu",
                Date = new DateOnly(2025, 5, 12), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0),
                Status = "cancelled"
            },
            new()
            {
                Id = 4, RoomId = 3, OrganizerName = "Maria Dąbrowska", Topic = "Rozmowa kwalifikacyjna",
                Date = new DateOnly(2025, 5, 13), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 0),
                Status = "confirmed"
            },
            new()
            {
                Id = 5, RoomId = 2, OrganizerName = "Tomasz Lewandowski", Topic = "Retrospektywa sprintu",
                Date = new DateOnly(2025, 5, 14), StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 0),
                Status = "planned"
            }
        };

        return reservations;
    }
}