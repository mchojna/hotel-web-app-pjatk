using HotelWebApp.Data;
using HotelWebApp.Model;

namespace HotelWebApp.Repository;

// Reservation repository mockup
public static class ReservationRepository
{
    public static List<Reservation> Reservations { get; }= DataInitializer.InitializeReservations();
}