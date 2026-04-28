using System.ComponentModel.DataAnnotations;

namespace HotelWebApp.Dto;

public class CreateReservationDto
{
    [Required] public int RoomId { get; set; }

    [Required] [MaxLength(120)] public string OrganizerName { get; set; } = string.Empty;

    [Required] [MaxLength(120)] public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
}