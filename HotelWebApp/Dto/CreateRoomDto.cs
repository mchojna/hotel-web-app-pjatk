using System.ComponentModel.DataAnnotations;

namespace HotelWebApp.Dto;

public class CreateRoomDto
{
    [Required] [MaxLength(128)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(4)] public string BuildingCode { get; set; } = string.Empty;

    [Range(1, 8)] public int Floor { get; set; }

    [Range(1, 64)] public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}