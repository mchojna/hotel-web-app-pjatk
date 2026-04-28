using HotelWebApp.Data;
using HotelWebApp.Dto;
using HotelWebApp.Model;
using HotelWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controller;

// api/rooms
[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private List<Reservation> _reservations = ReservationRepository.Reservations;

    // GET /api/reservations Zwraca wszystkie rezerwacje.
    // GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2 Zwraca rezerwacje przefiltrowane po query stringu.
    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = _reservations.AsQueryable();

        if (date.HasValue) reservations = reservations.Where(r => r.Date.Equals(date));

        if (status != null) reservations = reservations.Where(r => r.Status.Equals(status));

        if (roomId.HasValue) reservations = reservations.Where(r => r.RoomId == roomId);

        return Ok(reservations.ToList());
    }

    // GET /api/reservations/{id} Zwraca jedną rezerwację.
    [Route("{id:int}")]
    [HttpGet]
    public IActionResult GetReservationById([FromRoute] int id)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null) return NotFound();

        return Ok(reservation);
    }

    // POST /api/reservations Tworzy nową rezerwację.
    [HttpPost]
    public IActionResult CreateReservation([FromBody] CreateReservationDto createReservationDto)
    {
        var reservation = new Reservation
        {
            Id = _reservations.Count > 0 ? _reservations.Max(r => r.Id) + 1 : 1,
            OrganizerName = createReservationDto.OrganizerName,
            Topic = createReservationDto.Topic,
            Date = createReservationDto.Date,
            StartTime = createReservationDto.StartTime,
            EndTime = createReservationDto.EndTime,
            Status = createReservationDto.Status
        };

        _reservations.Add(reservation);
        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
    }

    // PUT /api/reservations/{id} Aktualizuje istniejącą rezerwację.

    // DELETE /api/reservations/{id} Usuwa rezerwację.
    [Route("{id:int")]
    [HttpDelete]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null) return NotFound();

        _reservations.Remove(reservation);
        return NoContent();
    }
}