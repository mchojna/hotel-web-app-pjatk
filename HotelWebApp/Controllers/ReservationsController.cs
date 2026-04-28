using HotelWebApp.Data;
using HotelWebApp.Dto;
using HotelWebApp.Model;
using HotelWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

// api/reservations
[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly List<Reservation> _reservations = ReservationRepository.Reservations;
    private readonly List<Room> _rooms = RoomRepository.Rooms;

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
        var room = _rooms.FirstOrDefault(r => r.Id == createReservationDto.RoomId);
        if (room == null) return NotFound($"Sala o Id {createReservationDto.RoomId} nie istnieje.");

        if (!room.IsActive)
            return Conflict($"Sala o Id {room.Id} jest nieaktywna.");

        if (HasTimeConflict(createReservationDto.RoomId, createReservationDto.Date,
                createReservationDto.StartTime, createReservationDto.EndTime))
            return Conflict("Rezerwacja koliduje czasowo z inną rezerwacją tej sali.");

        var reservation = new Reservation
        {
            Id = _reservations.Count > 0 ? _reservations.Max(r => r.Id) + 1 : 1,
            RoomId = createReservationDto.RoomId,
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
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult Update([FromRoute] int id, [FromBody] CreateReservationDto updateReservationDto)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        var room = _rooms.FirstOrDefault(r => r.Id == updateReservationDto.RoomId);
        if (room == null) return NotFound($"Sala o Id {updateReservationDto.RoomId} nie istnieje.");

        if (!room.IsActive)
            return Conflict($"Sala o Id {room.Id} jest nieaktywna.");

        if (HasTimeConflict(updateReservationDto.RoomId, updateReservationDto.Date,
                updateReservationDto.StartTime, updateReservationDto.EndTime, id))
            return Conflict("Rezerwacja koliduje czasowo z inną rezerwacją tej sali.");

        reservation.RoomId = updateReservationDto.RoomId;
        reservation.OrganizerName = updateReservationDto.OrganizerName;
        reservation.Topic = updateReservationDto.Topic;
        reservation.Date = updateReservationDto.Date;
        reservation.StartTime = updateReservationDto.StartTime;
        reservation.EndTime = updateReservationDto.EndTime;
        reservation.Status = updateReservationDto.Status;

        return Ok(reservation);
    }


    // DELETE /api/reservations/{id} Usuwa rezerwację.
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult DeleteReservation([FromRoute] int id)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null) return NotFound();

        _reservations.Remove(reservation);
        return NoContent();
    }

    private bool HasTimeConflict(int roomId, DateOnly date, TimeOnly start, TimeOnly end, int? ignoreId = null)
    {
        return _reservations.Any(r =>
            r.RoomId == roomId &&
            r.Date == date &&
            (ignoreId == null || r.Id != ignoreId.Value) &&
            r.StartTime < end &&
            start < r.EndTime);
    }
}