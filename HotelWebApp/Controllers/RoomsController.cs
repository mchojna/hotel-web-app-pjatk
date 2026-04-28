using HotelWebApp.Data;
using HotelWebApp.Dto;
using HotelWebApp.Model;
using HotelWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

// api/rooms
[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly List<Room> _rooms = RoomRepository.Rooms;
    private readonly List<Reservation> _reservations = ReservationRepository.Reservations;

    // GET /api/rooms Zwraca wszystkie sale.
    // GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true Zwraca sale przefiltrowane po query stringu.
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = _rooms.AsQueryable();

        if (minCapacity.HasValue) rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue) rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue) rooms = rooms.Where(r => r.IsActive == activeOnly.Value);

        return Ok(rooms.ToList());
    }

    // GET /api/rooms/{id} Zwraca pojedynczą salę po identyfikatorze.
    [Route("{id:int}")]
    [HttpGet]
    public IActionResult GetRoomById([FromRoute] int id)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);

        if (room == null) return NotFound();

        return Ok(room);
    }

    // GET /api/rooms/building/{buildingCode} Zwraca sale z wybranego budynku. Parametr buildingCode ma być pobierany z trasy.
    [Route("building/{buildingCode}")]
    [HttpGet]
    public IActionResult GetRoomsByBuildingCode([FromRoute] string buildingCode)
    {
        var rooms = _rooms.Where(r => r.BuildingCode == buildingCode).ToList();

        return Ok(rooms);
    }

    // POST /api/rooms Dodaje nową salę.
    [HttpPost]
    public IActionResult CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        var room = new Room
        {
            Id = _rooms.Count > 0 ? _rooms.Max(r => r.Id) + 1 : 1,
            Name = createRoomDto.Name,
            BuildingCode = createRoomDto.BuildingCode,
            Capacity = createRoomDto.Capacity,
            Floor = createRoomDto.Floor,
            HasProjector = createRoomDto.HasProjector,
            IsActive = createRoomDto.IsActive
        };

        _rooms.Add(room);
        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    // PUT /api/rooms/{id} Aktualizuje pełne dane sali.
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult UpdateRoom([FromRoute] int id, [FromBody] CreateRoomDto updateRoomDto)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);

        if (room == null) return NotFound();

        if (room.IsActive && !updateRoomDto.IsActive)
        {
            var futureReservations = _reservations.Any(r =>
                r.RoomId == id && r.Date > DateOnly.FromDateTime(DateTime.Now));

            if (futureReservations)
                return Conflict("Nie można dezaktywować sali z przyszłymi rezerwacjami.");
        }

        room.Name = updateRoomDto.Name;
        room.BuildingCode = updateRoomDto.BuildingCode;
        room.Capacity = updateRoomDto.Capacity;
        room.Floor = updateRoomDto.Floor;
        room.HasProjector = updateRoomDto.HasProjector;
        room.IsActive = updateRoomDto.IsActive;

        return Ok(room);
    }

    // DELETE /api/rooms/{id} Usuwa salę.
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);

        if (room == null) return NotFound();

        var reservations = _reservations.Where(r => r.RoomId == id && r.Date > DateOnly.FromDateTime(DateTime.Now));

        if (reservations.Any()) return Conflict();

        _rooms.Remove(room);
        return NoContent();
    }
}